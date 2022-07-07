using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Manager<BattleManager> {

    private EntitiesDatabaseSO entitiesDatabase;
    private WarbandDataSO warbandData;
    public Transform[] playerTrans; // Contains positions to instantiate the player animals
    public Transform[] enemyTrans; // Contains positions to instantiate the enemy animals

    private List<BaseEntity> playerTeam = new List<BaseEntity>();
    private List<BaseEntity> enemyTeam = new List<BaseEntity>();

    public Transform canvas;
    public CurrentState currentState;
    
    private void Start() {
        warbandData = GameManager.Instance.warbandData;
        entitiesDatabase = GameManager.Instance.entitiesDatabase;
        ChangeState(CurrentState.BEFOREBATTLE);
    }
    
    private void InstantiatePlayerWarband() {
        foreach (WarbandDataSO.EntityData animalInfo in warbandData.warbandEntities) {

            // Grab the base Prefab based on animalID from entitiesDatabaseSO
            BaseEntity actualPrefab = entitiesDatabase.allEntities[animalInfo.animalID - 1].prefab;
            // Instantiate at respective position
            BaseEntity newCard = Instantiate(actualPrefab, 
                                            new Vector2(playerTrans[animalInfo.position].position.x, playerTrans[animalInfo.position].position.y), 
                                            Quaternion.identity);
            newCard.transform.SetParent(canvas);
            // Edit instantiated animal to suit Battle Scene
            newCard.transform.localScale = new Vector3(0.85f, 0.95f, 0.85f); // Change Scale
            Destroy(newCard.GetComponent<TooltipTrigger>()); // Remove tooltips
            Destroy(newCard.GetComponent<DragHandler>()); // Remove DragHandler
            newCard.transform.Find("TierBG").gameObject.SetActive(false); // Remove TierBG
            newCard.SetStats(animalInfo.attack, animalInfo.health); // Update Stats Accordingly
        }
    }
    
    private void InstantiateEnemyWarband() {
        // Logic
        
        // Change State from 'Before Battle' to 'Battle'
        //ChangeState(CurrentState.BATTLE);
    }

    public void onGoBackClick() {
        PlayerData.Instance.Victory(); // For DEBUG purpose ONLY
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }

    public void ChangeState(CurrentState newState) {
        currentState = newState;
        switch (newState) {
            case CurrentState.BEFOREBATTLE:
                // Add Functions Here
                InstantiatePlayerWarband();
                InstantiateEnemyWarband();
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
