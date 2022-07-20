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

    public List<BaseEntity> playerTeam = new List<BaseEntity>(); // Contains animals in player team (no nulls)
    public List<BaseEntity> enemyTeam = new List<BaseEntity>(); // Contains animals in enemy team (no nulls)

    public Transform playerFightPos; // where the player's animal moves to fight
    public Transform enemyFightPos; // where the enemy's animal moves to fight
    
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
        normalBattleOutcomePanel.gameObject.SetActive(true);
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
            newCard.battleRef = Instance; // Set reference to BattleManager to current instance (this)
            
            // Add the animal to playerTeam
            playerTeam.Add(newCard);

            //Debug.Log("Player Warband Added: " + newCard.name);
        }
    }
    
    /// <summary>
    /// Instantiates the enemy's warband taken from the enemyDatbase
    /// Adds each animal into the enemyTeam List in correct position
    /// </summary>
    private void InstantiateEnemyWarband() {
        List<EnemyDatabaseSO.TeamData> enemyTeams = new List<EnemyDatabaseSO.TeamData>();
        foreach (EnemyDatabaseSO.TeamData rTeam in enemyDatabase.pastTeams) {
            if (rTeam.turnNumber == PlayerData.Instance.TurnNumber) {
                enemyTeams.Add(rTeam);
            }
        }
        EnemyDatabaseSO.TeamData randomTeam = enemyTeams[Random.Range(0, enemyTeams.Count - 1)];
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
            newCard.battleRef = Instance; // Set reference to BattleManager to current instance (this)
            newCard.team = BaseEntity.Team.ENEMY; // Set team to Enemy

            enemyTeam.Add(newCard);

            //Debug.Log("Enemy Warband Added: " + newCard.name);
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
        
        if (playerTeam.Count > 0 && enemyTeam.Count > 0) { // If there are still animals in both teams, animate battle
            StartCoroutine(AnimateBattle());
        } else if (playerTeam.Count == 0 || enemyTeam.Count == 0) { // If there is at least one team empty, stop the battle
            StopCoroutine(AnimateBattle());
            ChangeState(CurrentState.AFTERBATTLE); //Change State from 'Battle' to 'After Battle'
        }      
    }

    /// <summary>
    /// Animates the battle and holds battle logic
    /// Called repeatedly by Battle() until at least one team is empty
    /// </summary>
    private IEnumerator AnimateBattle() {

        // Create local variable for first animal of each team
        BaseEntity player1 = playerTeam[0];
        BaseEntity enemy1 = enemyTeam[0];

        // Set target reference
        player1.target = enemy1;
        enemy1.target = player1;

        // Store attack stats before fighting (in case it changes due to special ability)
        int playerAttack = player1.GetAttack();
        int enemyAttack = enemy1.GetAttack();

        // Set variable to reference current Tweeen
        Tween currentTween;
        
        // Move
        currentTween = player1.transform.DOMove(playerFightPos.position, 0.5f).SetEase(Ease.InOutSine);
        enemy1.transform.DOMove(enemyFightPos.position, 0.5f).SetEase(Ease.InOutSine);
        yield return currentTween.WaitForCompletion();

        // Pause before fight
        yield return new WaitForSecondsRealtime(0.5f);

        // Fight -> Use DecreaseBattleStats()
        //          SetStats() changes the Max (which shouldn't be touched in Battle Phase)
        // Fight (Punch -> Shake -> DecreaseBattleStats())
        currentTween = player1.transform.DOPunchPosition(Vector3.right * 45f, 0.5f, 0, 0);
        enemy1.transform.DOPunchPosition(Vector3.left * 45f, 0.5f, 0, 0);
        yield return currentTween.WaitForCompletion();
        SoundManager.Instance.Play("Hit");
        currentTween = player1.transform.DOShakePosition(0.5f, 20f, 50);
        enemy1.transform.DOShakePosition(0.5f, 20f, 50);
        yield return currentTween.WaitForCompletion();
        player1.DecreaseBattleStats(0, enemyAttack); // enemy1 attacks player1
        enemy1.DecreaseBattleStats(0, playerAttack); // player1 attacks enemy1

        // Pause before destroying (if dead)
        yield return new WaitForSeconds(1f);
        
        // Fought one round already -> Check whether any of them died (Animate Death)
        if (player1.IsDead() && enemy1.IsDead()) { //Check if both die at the same time (Required for some animals like 'Rhino' and 'Duck')
            currentTween = player1.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            enemy1.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            yield return currentTween.WaitForCompletion();
            player1.Die();
            enemy1.Die();
            playerTeam.RemoveAt(0);
            enemyTeam.RemoveAt(0);
            if (playerTeam.Count > 0) {
                playerTeam[0].transform.DOMove(playerTrans[0].position, 0.5f).SetEase(Ease.InOutSine); //Move animal up the line
            }
            if (enemyTeam.Count > 0) {
                enemyTeam[0].transform.DOMove(enemyTrans[0].position, 0.5f).SetEase(Ease.InOutSine); //Move animal up the line
            }
        }
        else if (player1.IsDead()) { //Only player animal dies
            currentTween = player1.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            yield return currentTween.WaitForCompletion();
            player1.Die();
            playerTeam.RemoveAt(0);
            if (playerTeam.Count > 0) {
                playerTeam[0].transform.DOMove(playerTrans[0].position, 0.5f).SetEase(Ease.InOutSine); //Move animal up the line
            }
            
        }
        else if (enemy1.IsDead()) { //Only enemy animal dies
            currentTween = enemy1.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            yield return currentTween.WaitForCompletion();
            enemy1.Die();
            enemyTeam.RemoveAt(0);
            if (enemyTeam.Count > 0) {
                enemyTeam[0].transform.DOMove(enemyTrans[0].position, 0.5f).SetEase(Ease.InOutSine); //Move animal up the line
            }
        }

        // Pause before next battle
        yield return new WaitForSecondsRealtime(1f);
        Battle(); //Call 'Battle' again to check whether the battle is over
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
    private void DisplayBattleOutcomePanel() {
        if (battleOutcomePanel == BattleOutcomePanel.NORMALWIN) {
            // Set relevant text
            normalBattleOutcomePanel.SetOutcomeText("won", "!");
            normalBattleOutcomePanel.SetContinueText(PlayerData.Instance.TurnNumber + 1);
            // Set outcomeIsWin variable to true to play correct sounds
            normalBattleOutcomePanel.outcomeIsWin = true;
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
            // Set outcomeIsWin variable to true to enable animations
            gameOverBattleOutcomePanel.outcomeIsWin = true;
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
                DecideCorrectPanelToDisplay();
                DisplayBattleOutcomePanel();
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
    
    /*public void OnWinBattle() {
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
    }*/

    /*public void onGoBackClick() {
        PlayerData.Instance.Victory(); // For DEBUG purpose ONLY
        PlayerData.Instance.IncreaseTurnNumber(); // For DEBUG purpose ONLY
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }*/

    /*private IEnumerator MoveAnimalsToFight(BaseEntity player, BaseEntity enemy) {
        Tween currentTween = player.transform.DOMove(playerFightPos.position, 0.5f);
        enemy.transform.DOMove(enemyFightPos.position, 0.5f);
        yield return currentTween.WaitForCompletion();
    }

    private IEnumerator MakeAnimalsFight(BaseEntity player, BaseEntity enemy) {
        Tween currentTween = player.transform.DOPunchPosition(Vector3.right * 1.5f, 0.3f, 0, 0).OnComplete(() => {player.transform.DOShakePosition(0.3f, 0.3f, 10);});
        enemy.transform.DOPunchPosition(Vector3.left * 1.5f, 0.3f, 0, 0).OnComplete(() => {player.transform.DOShakePosition(0.3f, 0.3f, 10);});
        yield return currentTween.WaitForCompletion();
    }

    private IEnumerator ActivateFightEffect(BaseEntity player, BaseEntity enemy) {
        player.DecreaseBattleStats(0, enemy.GetAttack()); // enemy1 attacks player1
        enemy.DecreaseBattleStats(0, player.GetAttack()); // player1 attacks enemy1
        yield return new WaitForSecondsRealtime(1f);
    }

    private IEnumerator CheckDeath(BaseEntity player, BaseEntity enemy) {
        if (player.IsDead()) {
            Tween currentTween = player.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            yield return currentTween.WaitForCompletion();
            player.Die();
            playerTeam.RemoveAt(0);
        }
        if (enemy.IsDead()) {
            Tween currentTween = player.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            yield return currentTween.WaitForCompletion();
            enemy.Die();
            enemyTeam.RemoveAt(0);
        }

        yield return new WaitForSecondsRealtime(1f);
    }*/
}
