using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyOverheadUI : AbstractOverheadUI {

    [SerializeField] private Enemy enemy = null;

    protected override void Start() {
        enemy.OnDamaged += Enemy_OnDamaged;
        //Initialise base function settings when overriding function.
        base.Start();
    }

    protected override void UpdateHealthBar() {
        healthBar.fillAmount = enemy.GetCurrentHP();
    }

    private void Enemy_OnDamaged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

}
