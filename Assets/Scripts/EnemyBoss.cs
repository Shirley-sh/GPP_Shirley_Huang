using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBoss : EnemyBase {
    public GameObject phase1GO;
    public GameObject phase2GO;

    enum Phase : byte {
        Default,
        Appear,
        Spawn,
        Fire,
        Chase,
        Dead
    }

    Phase currentPhase;
    float scale;
    int totalHp;

    TaskManager taskManager;

    ActionTask enterAppearPhaseTask;
    ActionTask enterSpawnPhaseTask;
    ActionTask enterFirePhaseTask;
    ActionTask enterChasePhaseTask;
    ActionTask enterDeadPhaseTask;
    ProgressionTask scaleUpTask;
    ProgressionTask spawnTask;
    WaitTask idleTask;
    ProgressionTask moveTask;
    ProgressionTask attackTask;


    // Use this for initialization
    protected override void Start() {
        base.Start();

        scale = 0;
        totalHp = hp;
        currentPhase = Phase.Default;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);

        taskManager = new TaskManager();

        scaleUpTask = new ProgressionTask(ScaleUp, 0);
        enterAppearPhaseTask = new ActionTask(EnterAppearPhase);
        enterSpawnPhaseTask = new ActionTask(EnterSpawnPhase);
        enterFirePhaseTask = new ActionTask(EnterFirePhase);
        enterChasePhaseTask = new ActionTask(EnterChasePhase);
        enterDeadPhaseTask = new ActionTask(Die);
        spawnTask = new ProgressionTask(Spawn,0);
        idleTask =new WaitTask(3f);
        moveTask = new ProgressionTask(Move,0);
        attackTask = new ProgressionTask(Attack,0);

        Services.EnemyManager.AddBoss(gameObject);
    }


    protected override void Update() {
        Phase nextPhase = GetPhaseByStatus();
        SetPhase(nextPhase);
        taskManager.Update();
    }

    void SwitchAction() {
        switch (currentPhase) {
            case Phase.Appear:
                AppearAction();
                break;
            case Phase.Spawn:
                SpawnAction();
                break;
            case Phase.Fire:
                FireAction();
                break;
            case Phase.Chase:
                ChaseAction();
                break;
            case Phase.Dead:
                DeadAction();
                break;
            default:
                break;
        }
    }

    void SetPhase(Phase newPhase) {
        if (currentPhase != newPhase) {
            taskManager.AbortAll();
            Debug.Log("AbortCurrent;"+newPhase);
            currentPhase = newPhase;
            SwitchAction();
        }
    }

    Phase GetPhaseByStatus() {
        if (scale < 0.99) {
            return Phase.Appear;
        } else {
            if (hp > totalHp * 0.5) {
                return Phase.Spawn;
            } else if (hp > totalHp * 0.15) {
                return Phase.Fire;
            } else if (hp > 0) {
                return Phase.Chase;
            } else {
                return Phase.Dead;
            }
        }
    }

    void EnterAppearPhase() {
        SetPhase(Phase.Appear);
        Debug.Log("Appear");
    }
    void EnterSpawnPhase() {
        SetPhase(Phase.Spawn);
        Debug.Log("Spawn");
    }
    void EnterFirePhase() {
        SetPhase(Phase.Fire);
        phase1GO.SetActive(false);
        Debug.Log("Fire");
    }
    void EnterChasePhase() {
        SetPhase(Phase.Chase);
        phase2GO.SetActive(false);
        Debug.Log("Chase");
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

    void AppearAction() {
        taskManager.AddTask(enterAppearPhaseTask);
        taskManager.AddTask(scaleUpTask);
    }

    void SpawnAction() {
        taskManager.AddTask(enterSpawnPhaseTask);
        taskManager.AddTask(spawnTask);
    }

    void FireAction() {
        taskManager.AddTask(enterFirePhaseTask);
        taskManager.AddTask(attackTask);
    }

    void ChaseAction() {
        taskManager.AddTask(enterChasePhaseTask);
        taskManager.AddTask(moveTask);
        taskManager.AddTask(spawnTask);
        taskManager.AddTask(attackTask);
    }

    void DeadAction() {
        taskManager.AddTask(enterDeadPhaseTask);
    }

    void ScaleUp(){
        scale = Mathf.Lerp(scale, 1f, Time.deltaTime);
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        //Debug.Log("Scale Up");
    }

    void Spawn(){
        if(!Services.EnemyManager.hasEnemy()){
            Services.EnemyManager.InitNewWave();
        }
    }

}



