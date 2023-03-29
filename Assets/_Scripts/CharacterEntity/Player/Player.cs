using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterEntity {

    public static Player INSTANCE {get; private set;}

    public event EventHandler OnDamaged;

    protected override void Awake() {
        //Initialise base function settings when overriding function.
        base.Awake();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("Player instance created.");
        } else {
            Debug.LogError("More than one Player instance created.");
            Destroy(gameObject);
            return;
        }
    }

    public override void TakeDamage(int enemyParamATK) {

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
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }

}
