using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyOverheadUI : MonoBehaviour
{

    [SerializeField] private Enemy enemy = null;
    [SerializeField] private Image healthBar;

    private void Start() {
        enemy.OnDamaged += Enemy_OnDamaged;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        healthBar.fillAmount = enemy.GetCurrentHP();
    }

    private void LateUpdate() {
        //Make UI always face the camera with a 90 degree x-axis.
        transform.rotation = Quaternion.Euler(90,0,0);
    }

    private void Enemy_OnDamaged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

}
