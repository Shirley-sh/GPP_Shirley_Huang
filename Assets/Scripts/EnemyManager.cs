using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public GameObject enemyBomber;
    public GameObject enemyShooter;
    public float waveInterval;
    private float waveTimer;
    private List<GameObject> enemies;
    // Use this for initialization
    void Start() {
        waveTimer = 0;
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {
        waveTimer += Time.deltaTime;
        if(waveTimer>=waveInterval){
            waveTimer = 0;
            InitNewWave();
        }

        if(enemies.Count == 0){
            InitNewWave();
        }
        //update enemies

        //remove dead enemies
        for (int i = enemies.Count - 1; i >= 0; i--) {
            GameObject e = enemies[i];
            if (!e.GetComponent<EnemyBase>().GetIsAlive()) {
                enemies.Remove(e);
                Destroy(e);
            }
        }
    }

    void InitNewWave(){
        if (Random.Range(0f, 1f) < 0.5) {
            InstantiateBomberWave();
        } else {
            InstantiateShooterWave();
        }
    }

    void InstantiateBomberWave() {
        float xOffset = 4;
        float yOffset = 3;
        GameObject e1 = Instantiate(enemyBomber,
                            new Vector3(xOffset, yOffset, 0),
                            transform.rotation);
        GameObject e2 = Instantiate(enemyBomber,
                                    new Vector3(xOffset, -yOffset, 0),
                                    transform.rotation);
        GameObject e3 = Instantiate(enemyBomber,
                                    new Vector3(-xOffset, yOffset, 0),
                                    transform.rotation);
        GameObject e4 = Instantiate(enemyBomber,
                                    new Vector3(-xOffset, -yOffset, 0),
                                    transform.rotation);
        enemies.Add(e1);
        enemies.Add(e2);
        enemies.Add(e3);
        enemies.Add(e4);

    }

    void InstantiateShooterWave() {
        float xOffset = 6;
        float yOffset = 4;


        GameObject e1 = Instantiate(enemyShooter,
                            new Vector3(0, yOffset, 0),
                            transform.rotation);
        GameObject e2 = Instantiate(enemyShooter,
                                    new Vector3(0, -yOffset, 0),
                                    transform.rotation);
        GameObject e3 = Instantiate(enemyShooter,
                                    new Vector3(-xOffset, 0, 0),
                                    transform.rotation);
        GameObject e4 = Instantiate(enemyShooter,
                                    new Vector3(xOffset, 0, 0),
                                    transform.rotation);
        enemies.Add(e1);
        enemies.Add(e2);
        enemies.Add(e3);
        enemies.Add(e4);
    }

}