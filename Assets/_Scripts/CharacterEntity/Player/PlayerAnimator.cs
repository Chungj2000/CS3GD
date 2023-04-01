using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    [SerializeField] private PlayerController player;
    private Animator playerAnimator;

    private const string isWalking = "IsWalking";
    private const string isDead = "IsDead";

    private void Awake() {
        playerAnimator = GetComponent<Animator>();
        
    }

    private void Update() {
        playerAnimator.SetBool(isWalking, player.IsWalking());
        playerAnimator.SetBool(isDead, Player.INSTANCE.IsDead());
    }

}
