using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour{

    public GameObject bulletGO;
    public float moveSpeed;
    public float rotationSpeed;
    public float shootForce;
    public float attackInterval;

    Bullet bullet;
    Rigidbody2D rd;
    GameObject player;
    float attackTimer;
    bool isAlive=true;


    // Use this for initialization
    protected virtual void Start(){
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

    protected virtual void KeepDistance(GameObject target){
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("PlayerBullet")) {
            Die();
        }
    }

    protected virtual void Die(){
        isAlive = false;
    }

    public bool GetIsAlive(){
        return isAlive;
    }
}
