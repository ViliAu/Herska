using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour {
    [HideInInspector] private float speed = 100;
    [HideInInspector] private float lifeTime = 3;
    [HideInInspector] private float spawnTime = 0;

    private Vector3 vel = Vector3.zero;

    private void Update() {
        MoveTrail();
    }

    private void MoveTrail() {
        if (Time.time > lifeTime + spawnTime) {
            Destroy(gameObject);
        }
        transform.position += vel * Time.deltaTime;
    }

    public void setupTrail(float speed, float lifeTime) {
        spawnTime = Time.time;
        this.speed = speed;
        this.lifeTime = lifeTime == 0 ? this.lifeTime : lifeTime < 0.1f ? 0.1f : lifeTime;
        vel = transform.forward * speed + new Vector3(EntityManager.LocalPlayer.Player_Controller.velocity.x, 0, EntityManager.LocalPlayer.Player_Controller.velocity.z);
    }
}
