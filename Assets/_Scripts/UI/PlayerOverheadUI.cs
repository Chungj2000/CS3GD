using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverheadUI : AbstractOverheadUI {

    protected override void Start() {
        Player.INSTANCE.OnDamaged += Player_OnDamaged;
        //Initialise base function settings when overriding function.
        base.Start();
    }

    protected override void UpdateHealthBar() {
        healthBar.fillAmount = Player.INSTANCE.GetCurrentHP();
    }

    private void Player_OnDamaged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

}