using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour {

    private GameObject playerEntity;
    private Enemy enemyEntity;
    private EnemyAnimator enemyAnimator;
    private NavMeshAgent agent;

    private bool playerInAttackRange, hasAttacked;
    private bool isDead;

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

        //Check for attack range.
        playerInAttackRange = Physics.CheckSphere(transform.position, enemyEntity.GetParamATK_RANGE(), LayerMask.GetMask("Player"));
        //Debug.Log("Player in range: " + playerInAttackRange);

        enemyAnimator.PlayAttack(hasAttacked);

        if(playerEntity != null) {
            MoveToPlayer();
            
            if(playerInAttackRange) {
                AttackPlayer();
            }

        }

    }

    public void MoveToPlayer() {

        agent.SetDestination(playerEntity.transform.position);
        
    }

    public void AttackPlayer() {

        agent.SetDestination(transform.position);
        transform.LookAt(playerEntity.transform.position);

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

    public void KillEnemy() {

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

    public bool IsDead() {
        return isDead;
    }

    public NavMeshAgent GetAgent() {
        return agent;
    }

    private void ChanceDropHealthPotion() {

        float randomValue = Random.Range(0f, 1f);

        //Debug.Log("Rolled a: " + randomValue);

        if(randomValue <= enemyEntity.GetHealthPotionDropRate()) {
            //Debug.Log("Health Potion dropped.");
            Instantiate(healthPotionPrefab, enemyEntity.transform.position, Quaternion.identity);
        }

    }

}
