using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum GameState { MENU, PREPARE, BATTLE, GAMEOVER }

public class GameManager : SingletonManager<GameManager> {

    //public EntitiesDatabaseSO entitiesDatabase;
    //Dictionary<Team, List<???>>
    public List<BaseEntity> playerWarband; // Only updated when clicking "End Turn" in Preparation Phase
}
