using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Manager<BattleManager> {

    private EntitiesDatabaseSO entitiesDatabase;
    private WarbandDataSO warbandData;
    public Transform[] playerTrans;
    public Transform[] enemyTrans;
    public Transform canvas;
    public CurrentState currentState;
    
    private void Start() {
        warbandData = GameManager.Instance.warbandData;
        entitiesDatabase = GameManager.Instance.entitiesDatabase;
        ChangeState(CurrentState.BEFOREBATTLE);
    }
    
    private void InstantiatePlayerWarband() {
        foreach (WarbandDataSO.EntityData animalInfo in warbandData.warbandEntities) {

            BaseEntity actualPrefab = entitiesDatabase.allEntities[animalInfo.animalID - 1].prefab;
            BaseEntity newCard = Instantiate(actualPrefab, 
                                            new Vector2(playerTrans[animalInfo.position].position.x, playerTrans[animalInfo.position].position.y), 
                                            Quaternion.identity);
            newCard.transform.SetParent(canvas);
            newCard.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
    }

    public void onGoBackClick() {
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }

    public void ChangeState(CurrentState newState) {
        currentState = newState;
        switch (newState) {
            case CurrentState.BEFOREBATTLE:
                // Add Functions Here
                InstantiatePlayerWarband();
                break;
            case CurrentState.BATTLE:
                // Add Functions Here
                break;
            case CurrentState.AFTERBATTLE:
                // Add Functions Here
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    
    public enum CurrentState { 
        BEFOREBATTLE,   // Instantiate Player's Team and Opponent's Team
        BATTLE,         // Animate Battle ==> DoTween .DOPunchPosition() / .DOShakePosition() / .DOJump()
        AFTERBATTLE     // Display Victory / Loss Panel?
    }

    public enum Team {
        PLAYER,
        OPPONENT
    }
}
