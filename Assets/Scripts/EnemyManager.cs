using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager{
    GameObject enemyBomber;
    GameObject enemyShooter;
    GameObject enemyBattery;
    private List<GameObject> enemies;

    public EnemyManager() {
        enemies = new List<GameObject>();
        enemyBomber = Resources.Load("Enemy Bomber") as GameObject;
        enemyShooter = Resources.Load("Enemy Shooter") as GameObject;
        enemyBattery  = Resources.Load("Enemy Battery") as GameObject;
    }

    public void Update() {

        if(enemies.Count == 0){
            InitNewWave();
        }
        //update enemies

        //remove dead enemies
        for (int i = enemies.Count - 1; i >= 0; i--) {
            GameObject e = enemies[i];
            if (!e.GetComponent<EnemyBase>().GetIsAlive()) {
                enemies.Remove(e);
                Object.Destroy(e);
            }
        }
    }

    void InitNewWave(){
        if (Random.Range(0f, 1f) < 0.5) {
            InstantiateBomberWave();
        } else {
            InstantiateShooterWave();
        }
        if (Random.Range(0f, 1f) < 0.5) {
            InstantiateBatteryWaveHorizontal();
        } else {
            InstantiateBatteryWaveVertical();
        }
    }

    void InstantiateBomberWave() {
        float xOffset = 4;
        float yOffset = 3;
        GameObject e1 = Object.Instantiate(enemyBomber,
                            new Vector3(xOffset, yOffset, 0),
                                               Quaternion.identity);
        GameObject e2 = Object.Instantiate(enemyBomber,
                                    new Vector3(xOffset, -yOffset, 0),
                                               Quaternion.identity);
        GameObject e3 = Object.Instantiate(enemyBomber,
                                    new Vector3(-xOffset, yOffset, 0),
                                               Quaternion.identity);
        GameObject e4 = Object.Instantiate(enemyBomber,
                                    new Vector3(-xOffset, -yOffset, 0),
                                               Quaternion.identity);
        enemies.Add(e1);
        enemies.Add(e2);
        enemies.Add(e3);
        enemies.Add(e4);

    }

    void InstantiateShooterWave() {
        float xOffset = 6;
        float yOffset = 4;


        GameObject e1 = Object.Instantiate(enemyShooter,
                            new Vector3(0, yOffset, 0),
                                               Quaternion.identity);
        GameObject e2 = Object.Instantiate(enemyShooter,
                                    new Vector3(0, -yOffset, 0),
                                               Quaternion.identity);
        GameObject e3 = Object.Instantiate(enemyShooter,
                                    new Vector3(-xOffset, 0, 0),
                                               Quaternion.identity);
        GameObject e4 = Object.Instantiate(enemyShooter,
                                    new Vector3(xOffset, 0, 0),
                                               Quaternion.identity);
        enemies.Add(e1);
        enemies.Add(e2);
        enemies.Add(e3);
        enemies.Add(e4);
    }

    void InstantiateBatteryWaveVertical() {
        float xOffset = 3;
        GameObject e2 = Object.Instantiate(enemyBattery,
                                    new Vector3(xOffset, 0, 0),
                                    Quaternion.identity);
        GameObject e3 = Object.Instantiate(enemyBattery,
                                    new Vector3(-xOffset, 0, 0),
                                    Quaternion.identity);
        enemies.Add(e2);
        enemies.Add(e3);
    }

    void InstantiateBatteryWaveHorizontal() {
        float yOffset = 2;
                           
        GameObject e2 = Object.Instantiate(enemyBattery,
                                    new Vector3(0, yOffset, 0),
                                    Quaternion.identity);
        GameObject e3 = Object.Instantiate(enemyBattery,
                                    new Vector3(0, -yOffset, 0),
                                    Quaternion.identity);
        enemies.Add(e2);
        enemies.Add(e3);
    }
     
    int GetTotalNumber(){
        return enemies.Count;
    }
}