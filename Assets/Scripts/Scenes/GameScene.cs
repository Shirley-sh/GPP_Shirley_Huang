using System.Collections;
using UnityEngine;

public class GameScene : Scene<TransitionData>{
    [SerializeField] private GameObject[] enemies;

    protected override void OnEnter(TransitionData data){


    }

    protected override void OnExit(){
        Services.EnemyManager.RemoveAll();
        Services.SceneManager.PopScene();
    }

}