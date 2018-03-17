using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBattery : EnemyBase {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        Services.EventManager.Register<EnemyDie>(OnEnemyDie);
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
        Services.EventManager.UnRegister<EnemyDie>(OnEnemyDie);
        isAlive = false;
    }


    void OnEnemyDie(GameEvent e){
        Attack();
    }
}
