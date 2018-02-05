using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyBase {

    protected override void Start(){
        base.Start();
    }

    protected override void Update(){
        base.Update();
    }


    protected override void Move(){
        LookPlayer();
        FollowPlayer();
    }

    protected override void Attack(){
        ShootBullet();
    }

}
