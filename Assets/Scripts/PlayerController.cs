using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPoweredUp : GameEvent{
    public readonly GameObject Player;

    public PlayerPoweredUp(GameObject player){
        Player = player;
    }
}

public class PlayerController : MonoBehaviour {
    public int life;
    public bool debug;
    public float moveSpeed;
    public float rotationSpeed;
    public float shootForce;
    public float shootInterval;
    public GameObject bullet;
    Rigidbody2D rd;
    Vector2 AxisInput;
    float shootTimer;
    int _life;

    void Start(){
        rd = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(PowerUp());
        _life = life;
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            shootTimer = shootInterval;
        }
        if(Input.GetMouseButton(0)){
            shootTimer += Time.deltaTime;
            if(shootTimer>=shootInterval){
                ShootBullet();
                shootTimer = 0;
            }

        }
    }

    void FixedUpdate(){
        Move();
        MouseLook();
    }

    void MouseLook(){
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookPos = lookPos - transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rd.angularVelocity = 0;
    }

    void Move(){
        AxisInput = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        rd.AddForce(AxisInput * moveSpeed);
        //rd.velocity = new Vector2(1,0);
    }

    void ShootBullet(){

        GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.right * shootForce,ForceMode2D.Impulse);

    }

    void Die(){
        Services.EnemyManager.RemoveAll();
         Services.SceneManager.Swap<GameOverScene>();

      
    }

    void OnTriggerEnter2D (Collider2D collision){
        if(collision.gameObject.CompareTag("EnemyBullet")){
            _life--;
            if(_life<=0){
                _life = 0;
                Die();

            }
        }    
    }

    IEnumerator PowerUp(){
        yield return new WaitForSeconds(1);
        Services.EventManager.Fire(new PlayerPoweredUp(gameObject));
    }

    public void Reset(){
        _life = life;
    }

    public int GetLife(){
        return _life;
    }
}
