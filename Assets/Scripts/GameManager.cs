using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {

	void Awake () {
        Services.TaskManager = new TaskManager();
        Services.EventManager = new EventManager();
        Services.EnemyManager = new EnemyManager();
	}
	
	// Update is called once per frame
	void Update () {
        Services.TaskManager.Update();
        Services.EnemyManager.Update();
	}

    void OnEnterDemoMode() {
    }

    void OnDestroy() {
    }

}
