using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum GameState { MENU, PREPARE, BATTLE, GAMEOVER }

public class GameManager : Manager<GameManager> {

    public EntitiesDatabaseSO entitiesDatabase;
    //Dictionary<Team, List<???>>
    
    //public GameState state;

    void Start()
    {
        //state = GameState.MENU;
        //InstantiateUnits();
    }

    void Update()
    {
        
    }
}
