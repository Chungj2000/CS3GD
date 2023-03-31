using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    [SerializeField] private PlayerController player;
    private Animator playerAnimator;

    private void Awake() {
        playerAnimator = GetComponent<Animator>();
        
    }

    private void Update() {
        playerAnimator.SetBool("IsWalking", player.IsWalking());
        playerAnimator.SetBool("IsDead", Player.INSTANCE.IsDead());
    }

}
