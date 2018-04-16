using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {
    public PrefabDB prefabDB;
	void Awake () {
        Services.TaskManager = new TaskManager();
        Services.EventManager = new EventManager();
        Services.EnemyManager = new EnemyManager();
        Services.Prefabs = prefabDB;
        Services.SceneManager = new SceneManager<TransitionData>(gameObject, Services.Prefabs.Levels);
        Services.SceneManager.PushScene<SplashScene>();
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
