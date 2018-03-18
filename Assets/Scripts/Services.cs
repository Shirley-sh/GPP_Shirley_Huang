using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Services{

    private static TaskManager _taskManager;
    public static TaskManager TaskManager{
        get {
            return _taskManager;
        }
        set {
             _taskManager = value;
        }
    }

    private static EventManager _eventManager;
    public static EventManager EventManager {
        get {
            return _eventManager;
        }
        set {
            _eventManager = value;
        }
    }

    private static EnemyManager _enemyManager;
    public static EnemyManager EnemyManager {
        get {
            return _enemyManager;
        }
        set {
            _enemyManager = value;
        }
    }


}
