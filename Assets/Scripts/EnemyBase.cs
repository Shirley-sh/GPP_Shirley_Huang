using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDie : GameEvent {
    public readonly GameObject Enemy;

    public EnemyDie(GameObject e) {
        Enemy = e;
    }
}

public abstract class EnemyBase : MonoBehaviour{


    public float moveSpeed;
    public float closestDistance;
    public float rotationSpeed;
    public float shootForce;
    public float attackInterval;
    public int hp;

    GameObject bulletGO;
    Bullet bullet;
    Rigidbody2D rd;
    GameObject player;
    float attackTimer;
    protected bool isAlive=true;


    // Use this for initialization
    protected virtual void Start(){
        bulletGO = Resources.Load("Enemy Bullet") as GameObject;
        Services.EventManager.Register<PlayerPoweredUp>(OnPlayerPowerUp);
        bullet = bulletGO.GetComponent<Bullet>();
        player = GameObject.FindWithTag("Player");
        rd = gameObject.GetComponent<Rigidbody2D>();
        attackTimer = 0;
        isAlive = true;
    }

    // Update is called once per frame
    protected virtual void Update(){
        Move();
        if(CheckAttackTimer()){
            Attack();
        }
    }

    protected abstract void Move();

    protected abstract void Attack();

    protected virtual bool CheckAttackTimer(){
        attackTimer += Time.deltaTime;
        if(attackTimer>attackInterval){
            attackTimer = 0;
            return true;
        }else{
            return false;
        }
    }

    protected virtual void LookPlayer(){
        Vector3 lookPos = player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
    }

    protected virtual void FollowPlayer(){
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        dir *= moveSpeed;
        rd.AddForce(dir * moveSpeed);
    }

    protected virtual void ShootBullet(){
        GameObject bulletInstance = Instantiate(bulletGO, transform.position, transform.rotation);
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.right * shootForce, ForceMode2D.Impulse);
    }

    protected virtual void ShootBullet(float offsetDegree){//accept offset on Z axis
        GameObject bulletInstance = Instantiate(bulletGO, transform.position, transform.rotation);
        Vector3 shootDir = Quaternion.AngleAxis(offsetDegree, Vector3.forward) * transform.right;
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootDir*shootForce, ForceMode2D.Impulse);
    }

    protected virtual void DistanceFollow(){
        Vector3 dir = player.transform.position - transform.position;
        float dist = dir.magnitude;
        if(dist<= closestDistance){
            dir.Normalize();
            dir *= -moveSpeed;
            rd.AddForce(dir * moveSpeed);
        }else{
            FollowPlayer();
        }
       
    }

    protected virtual bool PlayerInRange() {
        Vector3 dir = player.transform.position - transform.position;
        float dist = dir.magnitude;
        return dist <= closestDistance;
    }

    protected virtual bool TargetedByPlayer(){
        float angle = 20;
        return Vector3.Angle(player.transform.right, transform.position - player.transform.position) < angle;
    }

    protected virtual void  Flee() {
        var fleeDirection = (transform.position - player.transform.position).normalized;
        fleeDirection = fleeDirection*30+ new Vector3(Random.value*10, Random.value*10,0);
        rd.AddForce(fleeDirection * moveSpeed);
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("PlayerBullet")) {
            TakeDamage(collision.gameObject.GetComponent<BulletPlayer>().power);
        }
    }

    protected virtual void Die(){
        isAlive = false;
        Services.EventManager.UnRegister<PlayerPoweredUp>(OnPlayerPowerUp);
        Services.EventManager.Fire(new EnemyDie(gameObject));
    }

    protected virtual void UpdateAliveStatus(){
        if (hp <= 0){
            Die();
        }
    }

    protected virtual void TakeDamage(int attackPower){
        hp -= attackPower;
        UpdateAliveStatus();

    }

    public bool GetIsAlive(){
        return isAlive;
    }

    protected virtual void OnPlayerPowerUp(GameEvent e){
        var poweredUpEvent = e as PlayerPoweredUp;
        Debug.Log("Catched powered up: "+poweredUpEvent.Player.name);
    }

}
