using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterEntity {

    public static Player INSTANCE {get; private set;}

    private bool isDead;

    public event EventHandler OnDamaged;

    protected override void Awake() {
        //Initialise base function settings when overriding function.
        base.Awake();

        isDead = false;

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("Player instance created.");
        } else {
            Debug.LogError("More than one Player instance created.");
            Destroy(gameObject);
            return;
        }
    }

    public override void TakeDamage(float enemyParamATK) {

        paramHP -= CalculateDamage(enemyParamATK);

        //Debug.Log("Player has taken " + CalculateDamage(enemyParamATK) + " damage.");

        OnDamaged?.Invoke(this, EventArgs.Empty);

        //Debug.Log("Player Health At: " + paramHP);

        CheckHealth();
        
    }

    private void CheckHealth() {

        //If player health is below 0, kill the player.
        if(paramHP <= 0) {
            KillPlayer();
        }

    }

    private void KillPlayer() {
        isDead  = true;
        Debug.Log("Player has died.");
        //Destroy(gameObject);
    }

    public bool IsDead() {
        return isDead;
    }

    public void InteractWithItem(ItemHandler item) {

        //Update Player parameters.
        ModifyParameters(item.GetParameters());
        MultiplierParameters(item.GetParameters());

        //Modify HealthBar visual if there is a change in HP.
        if(item.GetParameters()["paramMAX_HP"] != 0 || item.GetParameters()["paramHP"] != 0) {
            OnDamaged?.Invoke(this, EventArgs.Empty);
        } 
        
    }

}
