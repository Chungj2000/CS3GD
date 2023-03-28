using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitOverheadUI : MonoBehaviour {

    [SerializeField] private Enemy enemy = null;
    [SerializeField] private Image healthBar;

    private void Start() {
        Player.INSTANCE.OnDamaged += Unit_OnDamaged;
        enemy.OnDamaged += Unit_OnDamaged;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {

        //Identify if the health bar is for player or enemy entity.
        if(enemy == null) {
            //Debug.Log("Updating player health bar.");
            healthBar.fillAmount = Player.INSTANCE.GetCurrentHP();
        } else {
            //Debug.Log("Enemy player health bar.");
            healthBar.fillAmount = enemy.GetCurrentHP();
        }

    }

    private void LateUpdate() {
        //Make UI always face the camera with a 90 degree x-axis.
        transform.rotation = Quaternion.Euler(90,0,0);
    }

    private void Unit_OnDamaged(object sender, EventArgs e) {
        if(gameObject != null) {
            UpdateHealthBar();
        }
    }

}