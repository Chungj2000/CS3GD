using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterEntity : MonoBehaviour {

    [SerializeField] protected float paramMAX_HP;
    protected float paramHP;
    [SerializeField] protected float paramATK;
    [SerializeField] protected float paramATK_SPD;
    [SerializeField] protected float paramATK_RANGE;
    //Please note that paramMOVE_SPD and NavMeshAgent speed property for Enemy entities.
    [SerializeField] protected float paramMOVE_SPD;
    [SerializeField] protected float paramDEF;

    protected virtual void Awake() {
        //Restrict the maximum amount of HP a entity can hold based on MAX_HP value.
        SetHealthLimit();

        //Ensure entity is set to full health when spawned.
        paramHP = paramMAX_HP;
    }

    public abstract void TakeDamage(float paramATK);

    //Add/Subtract parameters by given values.
    public void ModifyParameters(Dictionary<string, float> modifyingParameters) {

        paramMAX_HP += modifyingParameters["paramMAX_HP"];
        //Debug.Log("Current MAX HP: " + paramMAX_HP);

        paramHP += modifyingParameters["paramHP"];
        //Debug.Log("Current HP: " + paramHP);

        paramATK += modifyingParameters["paramATK"];
        //Debug.Log("Current ATK: " + paramATK);

        paramATK_SPD += modifyingParameters["paramATK_SPD"];
        //Debug.Log("Current ATK SPD: " + paramATK_SPD);

        paramATK_RANGE += modifyingParameters["paramATK_RANGE"];
        //Debug.Log("Current ATK RANGE: " + paramATK_RANGE);

        paramMOVE_SPD += modifyingParameters["paramMOVE_SPD"];
        //Debug.Log("Current MOVE SPD: " + paramMOVE_SPD);

        paramDEF += modifyingParameters["paramDEF"];
        //Debug.Log("Current DEF: " + paramDEF);
        
        //Debug.Log("Parameters modified.");

        SetHealthLimit();

        RoundToTwoDecimals();

    }

    //Multiply parameters by given values.
    public void MultiplierParameters(Dictionary<string, float> modifyingParameters) {

        paramMAX_HP = paramMAX_HP * modifyingParameters["multiplierMAX_HP"];
        //Debug.Log("Current MAX HP: " + paramMAX_HP);

        paramHP = paramHP * modifyingParameters["multiplierHP"];
        //Debug.Log("Current HP: " + paramHP);

        paramATK = paramATK * modifyingParameters["multiplierATK"];
        //Debug.Log("Current ATK: " + paramATK);

        paramATK_SPD = paramATK_SPD * modifyingParameters["multiplierATK_SPD"];
        //Debug.Log("Current ATK SPD: " + paramATK_SPD);

        paramATK_RANGE = paramATK_RANGE * modifyingParameters["multiplierATK_RANGE"];
        //Debug.Log("Current ATK RANGE: " + paramATK_RANGE);

        paramMOVE_SPD = paramMOVE_SPD * modifyingParameters["multiplierMOVE_SPD"];
        //Debug.Log("Current MOVE SPD: " + paramMOVE_SPD);

        paramDEF = paramDEF * modifyingParameters["multiplierDEF"];
        //Debug.Log("Current DEF: " + paramDEF);
        
        //Debug.Log("Parameters multiplied.");

        SetHealthLimit();

        RoundToTwoDecimals();

    }

    //Increases health of the entity by a fixed percentage of their MAX_HP.
    protected void RecoverHP(float recoveryPercentage) {
        paramHP += paramMAX_HP * (recoveryPercentage);
        SetHealthLimit();
    }

    protected float CalculateDamage(float paramATK_Value) {

        float damage = 1;

        //Damage calculated by ATK - DEF.
        if(paramATK_Value - paramDEF > 0) {
            damage = paramATK_Value - paramDEF;
        } else {
            //If damage is 0 or below default the damage received by entity to 1.
            damage = 1;
        }

        return damage;
    }

    //Call this whenever MAX_HP or HP is, or may be changed to restrict HP by MAX_HP.
    private void SetHealthLimit() {
        paramHP = Mathf.Clamp(paramHP, 0f, paramMAX_HP);
        //Debug.Log("New HP limit at: " + paramMAX_HP);
    }

    //Round all parameters to 2dp.
    protected void RoundToTwoDecimals() {
        paramMAX_HP = Mathf.Round(paramMAX_HP * 100f) / 100f;
        paramHP = Mathf.Round(paramHP * 100f) / 100f;
        paramATK = Mathf.Round(paramATK * 100f) / 100f;
        paramATK_SPD = Mathf.Round(paramATK_SPD * 100f) / 100f;
        paramATK_RANGE = Mathf.Round(paramATK_RANGE * 100f) / 100f;
        paramMOVE_SPD = Mathf.Round(paramMOVE_SPD * 100f) / 100f;
        paramDEF = Mathf.Round(paramDEF * 100f) / 100f;

        //Debug.Log("Parameters rounded.");
    }


    //Parameter Getters.

    public float GetParamHP() {
        return paramHP;
    }

    public float GetParamMAX_HP() {
        return paramMAX_HP;
    }

    public float GetCurrentHP() {
        //Debug.Log("Returned value for CurrentHP: " + (float) paramHP / paramMAX_HP);
        return (float) paramHP / paramMAX_HP;
    }

    public float GetParamATK() {
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

    public float GetParamDEF() {
        return paramDEF;
    }

}
