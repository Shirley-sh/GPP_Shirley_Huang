using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GC;

public class EnemyBoss : EnemyBase {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        EventManager.Instance.Register<EnemyDie>(OnEnemyDie);
	}


    // Update is called once per frame
    protected override void Update() {

    }


    protected override void Move() {
        LookPlayer();
        FollowPlayer();
    }

    protected override void Attack() {
        ShootBullet(0);
        ShootBullet(45);
        ShootBullet(90);
        ShootBullet(135);
        ShootBullet(180);
        ShootBullet(-45);
        ShootBullet(-90);
        ShootBullet(-135);

    }

    protected override void Die() {
        EventManager.Instance.UnRegister<EnemyDie>(OnEnemyDie);
        isAlive = false;
    }


    void OnEnemyDie(GameEvent e){
        Attack();
    }
}
