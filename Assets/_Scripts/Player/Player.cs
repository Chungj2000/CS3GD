using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player INSTANCE {get; private set;}
    
    [SerializeField] private static int paramMAX_HP = 100;
    [SerializeField] private int paramHP = paramMAX_HP;
    [SerializeField] private int paramATK = 30;
    [SerializeField] private float paramATK_SPD = 1;
    [SerializeField] private float paramATK_RANGE = 15;
    [SerializeField] private float paramMOVE_SPD = 3f;
    [SerializeField] private int paramDEF = 5;

    public event EventHandler OnDamaged;

    private void Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("Player instance created.");
        } else {
            Debug.LogError("More than one Player instance created.");
            Destroy(gameObject);
            return;
        }
    }

    public void TakeDamage(int enemyParamATK) {

        int damage = 1;

        //Damage calculated by player ATK - enemy DEF.
        if(enemyParamATK - paramDEF > 0) {
            damage = enemyParamATK - paramDEF;
        } else {
            //If damage is 0 or below default the damage received by enemy to 1.
            damage = 1;
        }

        //Debug.Log("Player has taken " + damage + " damage.");

        paramHP -= damage;

        //Debug.Log("Player Health At: " + paramHP);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        CheckHealth();
        
    }

    private void CheckHealth() {

        //If player health is below 0, kill the player.
        if(paramHP <= 0) {
            KillPlayer();
        }

    }

    private void KillPlayer() {
        Debug.Log("Player has died.");
        Destroy(gameObject);
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

    public float GetParamATK_SPD() {
        return paramATK_SPD;
    }

    public float GetParamATK_RANGE() {
        return paramATK_RANGE;
    }

    public float GetParamMOVE_SPD() {
        return paramMOVE_SPD;
    }

    public int GetParamDEF() {
        return paramDEF;
    }

}
