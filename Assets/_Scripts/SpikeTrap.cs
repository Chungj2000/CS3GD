using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {
    
    [SerializeField] private int cooldownTime = 15;
    private const string playerTag = "Player";
    private float damageInflicted;
    private bool trapOnCooldown;
    private Animator spikeTrapAnimator;
    private AudioSource audioSource;

    void Awake() {
        spikeTrapAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        trapOnCooldown = false;
    }

    //When a Player enters the collider.
    private void OnTriggerEnter(Collider collider) {

        Debug.Log("Trap collider triggered.");

        //Trap can only again trigger after a cooldown, and only on the Player.
        if(!trapOnCooldown && collider.gameObject.CompareTag(playerTag)) {

            //Damage = 20% of Player's MAX HP, not taking into account their DEF.
            damageInflicted = (collider.gameObject.GetComponent<Player>().GetParamMAX_HP())/5;

            Debug.Log("Trap has been triggered.");

            trapOnCooldown = true;

            //Inflict damage to player.
            collider.gameObject.GetComponent<Player>().TakeDamage(damageInflicted);

            //SFX
            SoundSystem.INSTANCE.PlaySFX(SoundSystem.INSTANCE.GetTrapTriggerSFX(), audioSource);

            StartCoroutine(AnimateTrap());
            StartCoroutine(CooldownTrap());

        }
        
    }

    private IEnumerator AnimateTrap() {
        spikeTrapAnimator.SetTrigger("open");
        yield return new WaitForSeconds(2);
        spikeTrapAnimator.SetTrigger("close");
    }

    private IEnumerator CooldownTrap() {
        Debug.Log("Trap now on cooldown.");
        yield return new WaitForSeconds(cooldownTime);
        trapOnCooldown = false;
    }



}
