using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    //Enemy parameters.
    [SerializeField] private static int paramMAX_HP = 100;
    [SerializeField] private int paramHP = paramMAX_HP;
    [SerializeField] private int paramATK = 10;
    [SerializeField] private float paramATK_RANGE = 1f;
    [SerializeField] private float paramATK_SPD = 2f;
    //Please note that paramMOVE_SPD and NavMeshAgent speed property must be changed for initial movespeed.
    [SerializeField] private float paramMOVE_SPD = 3f;
    [SerializeField] private int paramDEF = 5;
    [SerializeField] private int paramPOINTS = 10;

    private EnemyAi enemyAi;
    private ParticleSystem bloodSplatter;

    public event EventHandler OnDamaged;

    private void Start() {

        enemyAi = GetComponent<EnemyAi>();
        bloodSplatter = GetComponent<ParticleSystem>();
        
    }

    public void TakeDamage() {

        int damage = 1;

        //Damage calculated by player ATK - enemy DEF.
        if(Player.INSTANCE.GetParamATK() - paramDEF > 0) {
            damage = Player.INSTANCE.GetParamATK() - paramDEF;
        } else {
            //If damage is 0 or below default the damage received by enemy to 1.
            damage = 1;
        }

        paramHP -= damage;

        Debug.Log("Enemy has taken " + damage + " damage.");

        OnDamaged?.Invoke(this, EventArgs.Empty);

        Debug.Log("Enemy Health At: " + paramHP);
        bloodSplatter.Play();

        enemyAi.CheckHealth();

    }
    
    public int GetParamHP() {
        return paramHP;
    }

    public int GetParamMAX_HP() {
        return paramMAX_HP;
    }

    public float GetCurrentHP() {
        return (float) paramHP / paramMAX_HP;
    }

    public int GetParamATK() {
        return paramATK;
    }

    public float GetParamATK_RNG() {
        return paramATK_RANGE;
    }

    public float GetParamATK_SPD() {
        return paramATK_SPD;
    }

    public float GetParamMOVE_SPD() {
        return paramMOVE_SPD;
    }

    public int GetParamDEF() {
        return paramDEF;
    }

    public int GetParamPOINTS() {
        return paramPOINTS;
    }

}
