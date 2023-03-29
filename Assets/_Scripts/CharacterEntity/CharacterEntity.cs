using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterEntity : MonoBehaviour {

    [SerializeField] protected int paramMAX_HP;
    protected int paramHP;
    [SerializeField] protected int paramATK;
    [SerializeField] protected float paramATK_SPD;
    [SerializeField] protected float paramATK_RANGE;
    //Please note that paramMOVE_SPD and NavMeshAgent speed property for Enemy entities.
    [SerializeField] protected float paramMOVE_SPD;
    [SerializeField] protected int paramDEF;

    protected virtual void Awake() {
        //Ensure entity is set to full health when spawned.
        paramHP = paramMAX_HP;
    }

    public abstract void TakeDamage(int paramATK);

    protected int CalculateDamage(int paramATK_Value) {

        int damage = 1;

        //Damage calculated by ATK - DEF.
        if(paramATK_Value - paramDEF > 0) {
            damage = paramATK_Value - paramDEF;
        } else {
            //If damage is 0 or below default the damage received by entity to 1.
            damage = 1;
        }

        return damage;
    }

    //Parameter Getters.

    public int GetParamHP() {
        return paramHP;
    }

    public int GetParamMAX_HP() {
        return paramMAX_HP;
    }

    public float GetCurrentHP() {
        //Debug.Log("Returned value for CurrentHP: " + (float) paramHP / paramMAX_HP);
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
