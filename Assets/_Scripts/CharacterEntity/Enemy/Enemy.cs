using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharacterEntity {

    [SerializeField] private int paramPOINTS = 10;
    [Tooltip("Between 0 - 1")] [SerializeField] private float healthPotionDropRate = 0.1f;

    private EnemyAi enemyAi;
    private ParticleSystem bloodSplatter;

    public event EventHandler OnDamaged;

    private void Start() {

        enemyAi = GetComponent<EnemyAi>();
        bloodSplatter = GetComponent<ParticleSystem>();
        
    }

    public override void TakeDamage(float playerParamATK) {

        paramHP -= CalculateDamage(playerParamATK);

        //Debug.Log("Enemy has taken " + CalculateDamage(playerParamATK) + " damage.");

        OnDamaged?.Invoke(this, EventArgs.Empty);

        //Debug.Log("Enemy Health At: " + paramHP);

        bloodSplatter.Play();

        enemyAi.CheckHealth();

    }

    public int GetParamPOINTS() {
        return paramPOINTS;
    }

    public float GetHealthPotionDropRate() {
        return healthPotionDropRate;
    }

}
