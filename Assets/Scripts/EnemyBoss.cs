using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBoss : EnemyBase  {
    

    // Use this for initialization
    protected override void Start() {
        base.Start();
        Task appear = new AppearTask();
        Services.TaskManager.AddTask(appear);

    }


    // Update is called once per frame
    protected override void Update() {
    
    }


    protected override void Move() {
        LookPlayer();
        FollowPlayer();
    }

    protected override void Attack() {
        float direction = Random.Range(0, 360f);
        ShootBullet(direction);

    }

    protected override void Die() {
        isAlive = false;
    }

    void Appear(){
        
    }

    void Spawn() {

    }

    void Fire() {

    }

    void Chase() {

    }
}

class AppearTask : Task {
    protected override void OnAbort() { }
    protected override  void OnSuccess() { }
    protected override  void OnFail() { }

    protected override  void Init() { }
    internal override  void Update() {
        
    }
    protected override  void CleanUp() { }
}


