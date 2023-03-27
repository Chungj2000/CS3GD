using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    //Enemy parameters.
    [SerializeField] 
    private float health, maxHealth = 100f;
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    private float attackDelay = 2f;
    [SerializeField]
    private int enemyPoints = 10;

    private EnemyAi enemyAi;
    private ParticleSystem bloodSplatter;

    private void Start() {

        health = maxHealth;
        enemyAi = GetComponent<EnemyAi>();
        bloodSplatter = GetComponent<ParticleSystem>();
        
    }

    public void TakeDamage() {

        //Health needs refactoring later.
        health -= 25f;
        Debug.Log("Enemy Health At: " + health);
        bloodSplatter.Play();
        enemyAi.CheckHealth();

    }
    
    public float GetHealth() {
        return health;
    }

    public float GetMaxHealth() {
        return maxHealth;
    }

    public float GetAttackRange() {
        return attackRange;
    }

    public float GetAttackDelay() {
        return attackDelay;
    }

    public int GetEnemyPoints() {
        return enemyPoints;
    }

    //public void SetHealth(float health) {
    //    this.health = health;
    //}

}
