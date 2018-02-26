using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : Bullet {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
