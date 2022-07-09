using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Manager<BattleManager> {

    private EntitiesDatabaseSO entitiesDatabase;
    private WarbandDataSO warbandData;
    private EnemyDatabaseSO enemyDatabase;
    public Transform[] playerTrans; // Contains positions to instantiate the player animals
    public Transform[] enemyTrans; // Contains positions to instantiate the enemy animals

    private List<BaseEntity> playerTeam = new List<BaseEntity>(); // Contains animals in player team
    private List<BaseEntity> enemyTeam = new List<BaseEntity>(); // Contains animals in enemy team

    public Transform canvas;
    public CurrentState currentState; // To change according to game flow
    
    public NormalBattleOutcomePanel normalBattleOutcomePanel; // Reference to NormalBattleOutcomePanel
    public GameOverBattleOutcomePanel gameOverBattleOutcomePanel; // Reference to GameOverBattleOutcomePanel
    public BattleOutcome battleOutcome; // To change according to battle outcome. Always instantiated as PLAYING (placeholder)
    
    private void Start() {
        // Reference relevant databases from GameManager Instance
        warbandData = GameManager.Instance.warbandData;
        entitiesDatabase = GameManager.Instance.entitiesDatabase;
        enemyDatabase = GameManager.Instance.enemyDatabase;
        // Deactivate Battle Outcome Panels
        normalBattleOutcomePanel.gameObject.SetActive(false);
        gameOverBattleOutcomePanel.gameObject.SetActive(false);
        battleOutcome = BattleOutcome.PLAYING;
        // Set current state to "before battle"
        ChangeState(CurrentState.BEFOREBATTLE);
    }
    
    /// <summary>
    /// Instantiates the player's warband taken from the Preparation Phase
    /// Adds each animal into the playerTeam List in correct position
    /// </summary>
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
            
            // Add the animal to playerTeam
            playerTeam.Add(newCard);
        }
    }
    
    private void InstantiateEnemyWarband() {
        // Logic
        /*EnemyDatabaseSO.TeamData randomTeam = enemyDatabase.pastTeams[Random.Range(0, enemyDatabase.pastTeams.Count)];
        foreach (EnemyDatabaseSO.TeamData teamInfo in enemyDatabase.pastTeams) {
            
        }*/
        // Change State from 'Before Battle' to 'Battle'
        //ChangeState(CurrentState.BATTLE);
    }

    /// <summary>
    /// Displays the correct outcome panel with correct information.
    /// battleOutcome variable should have been set before calling this function
    /// Panel is transition between Battle and Preparation Phase / Main Menu (if Game Over)
    /// </summary>
    public void DisplayBattleOutcomePanel() {
        if (battleOutcome == BattleOutcome.NORMALWIN) {
            // Increment win by 1 (and reset money)
            PlayerData.Instance.Victory();
            // Set relevant text
            normalBattleOutcomePanel.SetOutcomeText("won", "!");
            normalBattleOutcomePanel.SetContinueText(PlayerData.Instance.TurnNumber + 1);
            // Set active panel
            normalBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcome == BattleOutcome.NORMALDRAW) {
            // Do nothing (and reset money)
            PlayerData.Instance.Draw();
            // Set relevant text
            normalBattleOutcomePanel.SetOutcomeText("drew", ".");
            normalBattleOutcomePanel.SetContinueText(PlayerData.Instance.TurnNumber + 1);
            // Set active panel
            normalBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcome == BattleOutcome.NORMALLOSE) {
            // Decrement life by 1 (and reset money)
            PlayerData.Instance.Lose();
            // Set relevant text
            normalBattleOutcomePanel.SetOutcomeText("lost", "...");
            normalBattleOutcomePanel.SetContinueText(PlayerData.Instance.TurnNumber + 1);
            // Set active panel
            normalBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcome == BattleOutcome.GAMEOVERWIN) {
            // Increment win by 1 (and reset money, although not needed)
            PlayerData.Instance.Victory();
            // Set relevant text
            gameOverBattleOutcomePanel.SetOutcomeText("won", "in", PlayerData.Instance.TurnNumber, "!");
            // Set active panel
            gameOverBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcome == BattleOutcome.GAMEOVERLOSE) {
            // Decrement life by 1 (and reset money, although not needed)
            PlayerData.Instance.Lose();
            // Set relevant text
            gameOverBattleOutcomePanel.SetOutcomeText("lost", "after", PlayerData.Instance.TurnNumber, "...");
            // Set active panel
            gameOverBattleOutcomePanel.gameObject.SetActive(true);
        }
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

    public enum BattleOutcome {
        PLAYING, // Placeholder
        NORMALWIN,
        NORMALDRAW,
        NORMALLOSE,
        GAMEOVERWIN,
        GAMEOVERLOSE
    }

    // ------------------------ DEBUGGING FUNCTIONS ----------------------------
    
    public void OnNormalWin() {
        battleOutcome = BattleOutcome.NORMALWIN;
        DisplayBattleOutcomePanel();
    }

    public void OnNormalDraw() {
        battleOutcome = BattleOutcome.NORMALDRAW;
        DisplayBattleOutcomePanel();
    }

    public void OnNormalLose() {
        battleOutcome = BattleOutcome.NORMALLOSE;
        DisplayBattleOutcomePanel();
    }

    public void OnGameOverWin() {
        battleOutcome = BattleOutcome.GAMEOVERWIN;
        DisplayBattleOutcomePanel();
    }

    public void OnGameOverLose() {
        battleOutcome = BattleOutcome.GAMEOVERLOSE;
        DisplayBattleOutcomePanel();
    }

    public void onGoBackClick() {
        PlayerData.Instance.Victory(); // For DEBUG purpose ONLY
        PlayerData.Instance.IncreaseTurnNumber(); // For DEBUG purpose ONLY
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }
}
