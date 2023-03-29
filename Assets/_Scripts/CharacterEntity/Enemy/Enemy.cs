using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharacterEntity {

    [SerializeField] private int paramPOINTS = 10;

    private EnemyAi enemyAi;
    private ParticleSystem bloodSplatter;

    public event EventHandler OnDamaged;

    private void Start() {

        enemyAi = GetComponent<EnemyAi>();
        bloodSplatter = GetComponent<ParticleSystem>();
        
    }

    public override void TakeDamage(int playerParamATK) {

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

}
