using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum GameState { MENU, PREPARE, BATTLE, GAMEOVER }

public class GameManager : SingletonManager<GameManager> {
    
    public EntitiesDatabaseSO entitiesDatabase;
    public WarbandDataSO warbandData;
    public ShopDataSO shopData;
    public EnemyDatabaseSO enemyDatabase;

    private void Start() {
        warbandData.warbandEntities = new List<WarbandDataSO.EntityData>();
        shopData.frozenShopEntities = new List<ShopDataSO.EntityData>();
        // This ensures any changes to the List<TeamData> pastTeams in enemyDatabase is saved even after we relaunch Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(enemyDatabase);
        #endif
    }

    public IEnumerator WaitForXSeconds(float time) {
        yield return new WaitForSecondsRealtime(time);
    } 

}
