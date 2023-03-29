using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour {
    
    private Animator enemyAnimator;
    private NavMeshAgent agent;

    private const string isWalking = "IsWalking";
    private const string isAttacking = "IsAttacking";
    private const string isDead = "Dead";

    private void Start() {
        enemyAnimator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        enemyAnimator.SetBool(isWalking, agent.velocity.magnitude > 0.01f);
    }

    public void PlayDead() {
        enemyAnimator.SetTrigger(isDead);
    }

    public void PlayAttack(bool hasAttacked) {
        enemyAnimator.SetBool(isAttacking, hasAttacked);
    }


}
