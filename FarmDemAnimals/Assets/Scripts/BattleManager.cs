using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public BattleOutcomePanel battleOutcomePanel; // To change according to battle outcome. Always instantiated as PLAYING (placeholder)
    public BattleOutcome battleOutcome; // Update after battle ends. Always instantiated as PLAYING (placeholder)
    
    private void Start() {
        // Reference relevant databases from GameManager Instance
        warbandData = GameManager.Instance.warbandData;
        entitiesDatabase = GameManager.Instance.entitiesDatabase;
        enemyDatabase = GameManager.Instance.enemyDatabase;
        // Deactivate Battle Outcome Panels
        normalBattleOutcomePanel.gameObject.SetActive(false);
        gameOverBattleOutcomePanel.gameObject.SetActive(false);
        battleOutcomePanel = BattleOutcomePanel.PLAYING;
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
        List<EnemyDatabaseSO.TeamData> enemyTeams = new List<EnemyDatabaseSO.TeamData>();
        foreach (EnemyDatabaseSO.TeamData rTeam in enemyDatabase.pastTeams) {
            if (rTeam.turnNumber == PlayerData.Instance.TurnNumber) {
                enemyTeams.Add(rTeam);
            }
        }
        EnemyDatabaseSO.TeamData randomTeam = enemyTeams[Random.Range(0, enemyTeams.Count)];
        foreach (WarbandDataSO.EntityData animalInfo in randomTeam.warbandTeam) {
            BaseEntity actualPrefab = entitiesDatabase.allEntities[animalInfo.animalID - 1].prefab;

            BaseEntity newCard = Instantiate(actualPrefab,
                                            new Vector2(enemyTrans[animalInfo.position].position.x, enemyTrans[animalInfo.position].position.y),
                                            Quaternion.identity);
            newCard.transform.SetParent(canvas);
            // Edit instantiated animal to suit Battle Scene
            newCard.transform.localScale = new Vector3(0.85f, 0.95f, 0.85f); // Change Scale
            Destroy(newCard.GetComponent<TooltipTrigger>()); // Remove tooltips
            Destroy(newCard.GetComponent<DragHandler>()); // Remove DragHandler
            newCard.transform.Find("TierBG").gameObject.SetActive(false); // Remove TierBG
            newCard.SetStats(animalInfo.attack, animalInfo.health); // Update Stats Accordingly

            enemyTeam.Add(newCard);
        }
        // Change State from 'Before Battle' to 'Battle'
        ChangeState(CurrentState.BATTLE);
    }

    /// <summary>
    /// Handles battle between player and enemy warbands
    /// Set the battleOutcome variable correctly, based on playerTeam.count
    /// This function is called before DecideCorrectPanelToDisplay()
    /// </summary>
    private void Battle() {
        // Make both teams battle
        while (playerTeam.Count > 0 && enemyTeam.Count > 0) {
            // Fight -> Use xxTeam.RemoveAt(0) to remove first animal in the list
            BaseEntity player1 = playerTeam[0];
            BaseEntity enemy1 = enemyTeam[0];
            player1.transform.DOMove(new Vector3(1,0,0), 2);
            enemy1.transform.DOMove(new Vector3(0,0,1), 2);
            player1.SetStats(player1.GetAttack(), player1.GetHealth() - enemy1.GetAttack());
            enemy1.SetStats(enemy1.GetAttack(), enemy1.GetHealth() - player1.GetAttack());
            if (player1.GetHealth() < 0) {
                
                playerTeam.RemoveAt(0);
            }
            if (enemy1.GetHealth() < 0) {
                enemyTeam.RemoveAt(0);
            }  
        }
        
        
        // At this point, at least one of the teams should be empty
        if (playerTeam.Count > 0) {
            // battle outcome is win
            battleOutcome = BattleOutcome.WIN;
        } else if (enemyTeam.Count > 0) {
            // battle outcome is lose
            battleOutcome = BattleOutcome.LOSE;
        } else {
            // battle outcome is draw
            battleOutcome = BattleOutcome.DRAW;
        }
        ChangeState(CurrentState.AFTERBATTLE);
        
    }

    /// <summary>
    /// Set the battleOutcomePanel variable correctly, based on battleOutcome
    /// Also handles changes to wins / lives accordingly
    /// This function is called before DisplayBattleOutcomePanel()
    /// </summary>
    private void DecideCorrectPanelToDisplay() {

        if (battleOutcome == BattleOutcome.WIN) {
            // Win => Increase trophy by 1
            PlayerData.Instance.IncreaseTrophies();
            // Check whether the player has won the game
            if (PlayerData.Instance.HasWonGame()) {
                // Won the game (Trophy == 5 after +1) => To Display Game Over (Win) screen
                battleOutcomePanel = BattleOutcomePanel.GAMEOVERWIN;
            } else {
                // Have not won the game (Trophy < 5 after + 1) => To Display Normal (Win) screen
                battleOutcomePanel = BattleOutcomePanel.NORMALWIN;
            }
        } else if (battleOutcome == BattleOutcome.LOSE) {
            // Lose => Decrease Life by 1
            PlayerData.Instance.LoseLife();
            // Check whether the player has lost the game
            if (PlayerData.Instance.HasLostGame()) {
                // Lost the game (Lives == 0 after -1) => To Display Game Over (Lose) screen
                battleOutcomePanel = BattleOutcomePanel.GAMEOVERLOSE;
            } else {
                // Have not lost the game (Lives > 0 after - 1) => To Display Normal (Lose) screen
                battleOutcomePanel = BattleOutcomePanel.NORMALLOSE;
            }
        } else if (battleOutcome == BattleOutcome.DRAW) {
            // Draw => Do Nothing => To Display Normal (Draw) screen
            battleOutcomePanel = BattleOutcomePanel.NORMALDRAW;
        }
    }

    /// <summary>
    /// Displays the correct outcome panel with correct information.
    /// battleOutcomePanel variable should have been set before calling this function
    /// Panel is transition between Battle and Preparation Phase / Main Menu (if Game Over)
    /// </summary>
    public void DisplayBattleOutcomePanel() {
        if (battleOutcomePanel == BattleOutcomePanel.NORMALWIN) {
            // Set relevant text
            normalBattleOutcomePanel.SetOutcomeText("won", "!");
            normalBattleOutcomePanel.SetContinueText(PlayerData.Instance.TurnNumber + 1);
            // Set active panel
            normalBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcomePanel == BattleOutcomePanel.NORMALDRAW) {
            // Set relevant text
            normalBattleOutcomePanel.SetOutcomeText("drew", ".");
            normalBattleOutcomePanel.SetContinueText(PlayerData.Instance.TurnNumber + 1);
            // Set active panel
            normalBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcomePanel == BattleOutcomePanel.NORMALLOSE) {
            // Set relevant text
            normalBattleOutcomePanel.SetOutcomeText("lost", "...");
            normalBattleOutcomePanel.SetContinueText(PlayerData.Instance.TurnNumber + 1);
            // Set active panel
            normalBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcomePanel == BattleOutcomePanel.GAMEOVERWIN) {
            // Set relevant text
            gameOverBattleOutcomePanel.SetOutcomeText("won", "in", PlayerData.Instance.TurnNumber, "!");
            // Set active panel
            gameOverBattleOutcomePanel.gameObject.SetActive(true);
        } else if (battleOutcomePanel == BattleOutcomePanel.GAMEOVERLOSE) {
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
                Battle();
                break;
            case CurrentState.AFTERBATTLE:
                // Add Functions Here
                //DecideCorrectPanelToDisplay();
                //DisplayBattleOutcomePanel();
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

    public enum BattleOutcomePanel {
        PLAYING, // Placeholder
        NORMALWIN,
        NORMALDRAW,
        NORMALLOSE,
        GAMEOVERWIN,
        GAMEOVERLOSE
    }

    public enum BattleOutcome {
        PLAYING, // Placeholder
        WIN,
        LOSE,
        DRAW
    }

    // ------------------------ DEBUGGING FUNCTIONS ----------------------------
    
    /*public void OnNormalWin() {
        battleOutcomePanel = BattleOutcomePanel.NORMALWIN;
        DisplayBattleOutcomePanel();
    }

    public void OnNormalDraw() {
        battleOutcomePanel = BattleOutcomePanel.NORMALDRAW;
        DisplayBattleOutcomePanel();
    }

    public void OnNormalLose() {
        battleOutcomePanel = BattleOutcomePanel.NORMALLOSE;
        DisplayBattleOutcomePanel();
    }

    public void OnGameOverWin() {
        battleOutcomePanel = BattleOutcomePanel.GAMEOVERWIN;
        DisplayBattleOutcomePanel();
    }

    public void OnGameOverLose() {
        battleOutcomePanel = BattleOutcomePanel.GAMEOVERLOSE;
        DisplayBattleOutcomePanel();
    }*/
    
    public void OnWinBattle() {
        battleOutcome = BattleOutcome.WIN;
        DecideCorrectPanelToDisplay();
        DisplayBattleOutcomePanel();
    }

    public void OnDrawBattle() {
        battleOutcome = BattleOutcome.DRAW;
        DecideCorrectPanelToDisplay();
        DisplayBattleOutcomePanel();
    }

    public void OnLoseBattle() {
        battleOutcome = BattleOutcome.LOSE;
        DecideCorrectPanelToDisplay();
        DisplayBattleOutcomePanel();
    }

    /*public void onGoBackClick() {
        PlayerData.Instance.Victory(); // For DEBUG purpose ONLY
        PlayerData.Instance.IncreaseTurnNumber(); // For DEBUG purpose ONLY
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }*/
}
