using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour {

    private Vector3 shootDirection;

    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float bulletDestroyDistance;

    public void Setup(Vector3 shootDirection, float attackRange) {
        //Move the bullet without transforming the height.
        this.shootDirection = new Vector3(shootDirection.x, 0f, shootDirection.z);
        bulletDestroyDistance = attackRange;
    }

    private void Update() {
        transform.position += shootDirection * Time.deltaTime * projectileSpeed;

        //Debug.Log("Bullet At: " + transform.position);

        //Destroy the bullet after reaching the maximum attack range value.
        if(Vector3.Distance(Player.INSTANCE.transform.position, transform.position) > bulletDestroyDistance) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(Player.INSTANCE.GetParamATK());

            //Bullet hit register SFX.
            SoundSystem.INSTANCE.PlaySFX(SoundSystem.INSTANCE.GetBulletImpactSFX(), 
                                         collider.gameObject.GetComponent<EnemyAi>().GetAudioSource());

            Destroy(gameObject);
        }
    }

}
