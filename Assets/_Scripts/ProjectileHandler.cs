using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour {

    private Vector3 shootDirection;
    [SerializeField] 
    private float projectileSpeed = 10f;
    [SerializeField] 
    private float bulletDestroyTime = 5f;
    public void Setup(Vector3 shootDirection) {
        this.shootDirection = shootDirection;
        Destroy(gameObject, bulletDestroyTime);
    }

    private void Update() {
        transform.position += shootDirection * Time.deltaTime * projectileSpeed;
        //Debug.Log("Bullet At: " + transform.position);
    }

    private void OnTriggerEnter(Collider collider) {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage();
            Destroy(gameObject);
        }
    }

}
