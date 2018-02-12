using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : EnemyBase {

    protected override void Start(){
        base.Start();

    }

    protected override void Update(){
        base.Update();
    }


    protected override void Move(){
        LookPlayer();
    }

    protected override void Attack(){
        ShootBullet(0);
        ShootBullet(45);
        ShootBullet(90);
        ShootBullet(135);
        ShootBullet(180);
        ShootBullet(-45);
        ShootBullet(-90);
        ShootBullet(-135);

    }



}
