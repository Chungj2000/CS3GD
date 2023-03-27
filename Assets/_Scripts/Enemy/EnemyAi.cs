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
    
    private void Start() {

        isDead = false;
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyEntity = GetComponent<Enemy>();
        playerEntity = InputManager.INSTANCE.GetPlayer();

    }

    private void Update() {

        //Check for attack range.
        playerInAttackRange = Physics.CheckSphere(transform.position, enemyEntity.GetAttackRange(), LayerMask.GetMask("Player"));
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
            Invoke(nameof(CooldownAttack), enemyEntity.GetAttackDelay());

        }

    }

    private void CooldownAttack() {
        //Debug.Log("Enemy attack on cooldown.");
        hasAttacked = false;
    }

    public void CheckHealth() {

        //If enemy health is below 0, kill the enemy.
        if(enemyEntity.GetHealth() <= 0) {

            KillEnemy();
            Destroy(GetComponent<BoxCollider>());
            StartCoroutine(ClearCorpse());

        }

    }

    public void KillEnemy() {

        isDead = true;
        enemyAnimator.PlayDead();
        agent.isStopped = true;
        Debug.Log("Enemy Killed!");
        ScoreTracker.INSTANCE.AddScore(enemyEntity.GetEnemyPoints());

    }

    //Delayed clean up of gameObject.
    IEnumerator ClearCorpse() {

        yield return new WaitForSeconds(15);
        Destroy(gameObject);
        Debug.Log("Corpse Removed!");

    }

    public bool IsDead() {
        return isDead;
    }

}
