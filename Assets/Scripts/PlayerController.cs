using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool debug;
    public float moveSpeed;
    public float rotationSpeed;
    public float shootForce;
    public GameObject bullet;
    Rigidbody2D rd;
    Vector2 AxisInput;


    void Start(){
        rd = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update(){
        if (Input.GetMouseButtonDown(0))
            ShootBullet();
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
        if(debug){
            gameObject.SetActive(false);
        }
      
    }

    void OnTriggerEnter2D (Collider2D collision){
        if(collision.gameObject.CompareTag("EnemyBullet")){
            Die();
        }    
    }
}
