using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum GameState { MENU, PREPARE, BATTLE, GAMEOVER }

public class GameManager : SingletonManager<GameManager> {

    //public EntitiesDatabaseSO entitiesDatabase;
    //Dictionary<Team, List<???>>
    public List<BaseEntity> playerWarband = new List<BaseEntity>(); // Only updated when clicking "End Turn" in Preparation Phase
    //public System.Action OnUpdateWarband;
    
    public EntitiesDatabaseSO entitiesDatabase;
    public WarbandDataSO warbandData;

    private void Start() {
        warbandData.warbandEntities = new List<WarbandDataSO.EntityData>();
    }

}
