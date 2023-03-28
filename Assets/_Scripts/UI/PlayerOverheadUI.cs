using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverheadUI : MonoBehaviour {

    [SerializeField] private Image healthBar;

    private void Start() {
        Player.INSTANCE.OnDamaged += Player_OnDamaged;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        healthBar.fillAmount = Player.INSTANCE.GetCurrentHP();
    }

    private void LateUpdate() {
        //Make UI always face the camera with a 90 degree x-axis.
        transform.rotation = Quaternion.Euler(90,0,0);
    }

    private void Player_OnDamaged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

}