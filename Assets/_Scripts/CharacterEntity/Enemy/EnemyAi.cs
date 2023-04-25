using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour {

    private GameObject playerEntity;
    private Enemy enemyEntity;
    private EnemyAnimator enemyAnimator;
    private NavMeshAgent agent;

    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float breakingDistance = 1f;

    private Vector3 waypoint;
    private bool playerInSightRange, playerInAttackRange, hasAttacked, waypointSet, isDead;

    [SerializeField] private GameObject healthPotionPrefab;
    
    private void Start() {

        isDead = false;
        agent = GetComponent<NavMeshAgent>();

        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyEntity = GetComponent<Enemy>();
        playerEntity = InputManager.INSTANCE.GetPlayer();

    }

    private void Update() {

        //Dead enemies cannot perform actions;
        if(isDead) {
            return;
        }

        //Enemies cannot chase act against a dead Player.
        if(Player.INSTANCE.IsDead()) {
            agent.isStopped = true;
            return;
        }

        //Bool for whenever player in attack range.
        playerInAttackRange = Physics.CheckSphere(transform.position, enemyEntity.GetParamATK_RANGE(), LayerMask.GetMask("Player"));
        //Bool for whenever player in sight range.
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, LayerMask.GetMask("Player"));

        //Debug.Log("Player in range: " + playerInSightRange);
        //Debug.Log("Player in range: " + playerInAttackRange);

        enemyAnimator.PlayAttack(hasAttacked);

        if(playerEntity != null) {
            Patrol();
            ChasePlayer();
            ReadyAttack();
        }

    }

    private void Patrol() {
        //If no player in sight or attack range, move enemy to a random point.
        if(!playerInSightRange && !playerInAttackRange) {
            //Debug.Log("Enemy is patrolling.");
            MoveToPoint();
        }
    }

    private void MoveToPoint() {
        //Move enemy to a random point within patrol distance.
        if(!waypointSet) {
            SetRandomWaypoint();
        }

        if(waypointSet) {
            agent.SetDestination(waypoint);
        }

        Vector3 remainingDistance = transform.position - waypoint;

        if(remainingDistance.magnitude < breakingDistance) {
            waypointSet = false;
        }
    }

    private void SetRandomWaypoint() {
        //Create a random position on the map to patrol to.
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);

        waypoint = new Vector3(transform.position.x + randomX, transform.position.y, + transform.position.z + randomZ);

        if(Physics.Raycast(waypoint, -transform.up, 2f, floorLayer)) {
            waypointSet = true;
        }
    }

    private void ChasePlayer() {
        //If player in sight but not attack range, move enemy to player.
        if(playerInSightRange && !playerInAttackRange) {
            //Debug.Log("Enemy is chasing.");
            MoveToPlayer();
        }
    }

    private void MoveToPlayer() {
        agent.SetDestination(playerEntity.transform.position);
    }

    private void ReadyAttack() {
        //If player in sight and attack range, attack.
        if(playerInSightRange && playerInAttackRange) {
            //Debug.Log("Enemy is attacking.");
            AttackPlayer();
        }
    }

    private void AttackPlayer() {

        agent.SetDestination(transform.position);
        transform.LookAt(playerEntity.transform.position);
        //Fix rotation angle to exlusively the y-axis when attacking.
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));

        if(!hasAttacked) {

            //Debug.Log("Enemy has attacked.");
            hasAttacked = true;

            //Debug.Log("Player expected to take a maximum of " + enemyEntity.GetParamATK() + " damage.");
            Player.INSTANCE.TakeDamage(enemyEntity.GetParamATK());

            //Initiate an attack with attack cooldown based on enemy attack speed.
            Invoke(nameof(CooldownAttack), enemyEntity.GetParamATK_SPD());

        }

    }

    private void CooldownAttack() {
        //Debug.Log("Enemy attack on cooldown.");
        hasAttacked = false;
    }

    public void CheckHealth() {

        //If enemy health is below 0, kill the enemy.
        if(enemyEntity.GetParamHP() <= 0) {

            KillEnemy();
            Destroy(GetComponent<BoxCollider>());
            StartCoroutine(ClearCorpse());

        }

    }

    private void KillEnemy() {

        isDead = true;
        enemyAnimator.PlayDead();
        agent.isStopped = true;
        //Debug.Log("Enemy Killed!");
        ScoreTracker.INSTANCE.AddScore(enemyEntity.GetParamPOINTS());

        //Randomly drops a health potion on death.
        ChanceDropHealthPotion();

        //Rescale the collider so it is not treated as an obstacle.
        //agent.radius = 0f;
        //Unfortunately can still collide with enemies but to a lesser extent.

    }

    //Delayed clean up of gameObject.
    IEnumerator ClearCorpse() {

        yield return new WaitForSeconds(15);
        Destroy(gameObject);
        //Debug.Log("Corpse Removed!");

    }

    private void ChanceDropHealthPotion() {

        float randomValue = Random.Range(0f, 1f);

        //Debug.Log("Rolled a: " + randomValue);

        if(randomValue <= enemyEntity.GetHealthPotionDropRate()) {
            //Debug.Log("Health Potion dropped.");
            Instantiate(healthPotionPrefab, enemyEntity.transform.position, Quaternion.identity);
        }

    }

    public bool IsDead() {
        return isDead;
    }

    public NavMeshAgent GetAgent() {
        return agent;
    }

}
