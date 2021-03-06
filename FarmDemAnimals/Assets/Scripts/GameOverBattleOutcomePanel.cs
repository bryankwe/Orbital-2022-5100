using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverBattleOutcomePanel : MonoBehaviour {

    private int currentWins;
    private int totalWinsNeeded;
    private int currentLives;
    private int totalLives;

    public Image[] wins; // Contains the base trophy positions
    public Image[] lives; // Contains the base heart positions
    private Color filledTrophyColor = new Color(1.0f, 1.0f, 1.0f); // Original color (Yellow?)
    private Color emptyTrophyColor = new Color(0f, 0f, 0f); // Color.black;
    private Color filledHeartColor = new Color(1.0f, 0f, 0f); // Color.red;
    private Color emptyHeartColor = new Color(0f, 0f, 0f);//new Color(233.0f/255.0f, 233.0f/255.0f, 233.0f/255.0f); // Color.grey;

    public Transform[] warbandTrans; // Position of warband
    public TextMeshProUGUI outcomeText;
    public Transform currentPanel; // Reference to current canvas
    public bool outcomeIsWin = false; // Possibly for future animation for win / lose
    public ParticleSystem confettiParticleSystem; // variable to signal if the panel is supposed to show win or lose effects
    bool hasPlayedAnimations = false; // variable to signal whether effects have been played

    void Start() {
        
        // Reference Wins to Trophies in PlayerData
        currentWins = PlayerData.Instance.Trophies;
        totalWinsNeeded = PlayerData.Instance.maxTrophies;
        
        // Reference Lives to Lives in Player Data
        currentLives = PlayerData.Instance.Lives;
        totalLives = PlayerData.Instance.maxLives;

        //Debug.Log("Current wins: " + currentWins);
        //Debug.Log("Current lives: " + currentLives);
    }

    void OnEnable() {

        // Reference Wins to Trophies in PlayerData
        currentWins = PlayerData.Instance.Trophies;
        totalWinsNeeded = PlayerData.Instance.maxTrophies;
        
        // Reference Lives to Lives in Player Data
        currentLives = PlayerData.Instance.Lives;
        totalLives = PlayerData.Instance.maxLives;
        
        InstantiatePlayerWarband();
    }

    /// <summary>
    /// Plays animations and sound effects accordingly to the battle outcome
    /// Played only ONCE
    /// </summary>
    public void PlayAnimations() {
        if (outcomeIsWin == true && !hasPlayedAnimations) {
            confettiParticleSystem.transform.GetComponent<UnityEngine.UI.Extensions.UIParticleSystem>().StartParticleEmission();
            SoundManager.Instance.Play("WinGame");
            hasPlayedAnimations = true;
        } else if (outcomeIsWin == false && !hasPlayedAnimations) {
            SoundManager.Instance.Play("Lose");
            hasPlayedAnimations = true;
        }
    }
    
    void Update() {

        // Logic for trophies display
        for (int i = 0; i < wins.Length; i++) {

            // Set trophy color based on number of wins
            if (i < currentWins) {
                wins[i].color = filledTrophyColor;
            } else {
                wins[i].color = emptyTrophyColor;
            }
        }

        // Logic for hearts display
        for (int i = 0; i < lives.Length; i++) {

            // Set heart color based on number of lives
            if (i < currentLives) {
                lives[i].color = filledHeartColor;
            } else {
                lives[i].color = emptyHeartColor;
            }
        }

        PlayAnimations();
    }

    /// <summary>
    /// Instantiate the original player warband (before battling) to the outcome panel
    /// 90% similar code to BattleManager.InstantiatePlayerWarband()
    /// </summary>
    private void InstantiatePlayerWarband() {
        foreach (WarbandDataSO.EntityData animalInfo in GameManager.Instance.warbandData.warbandEntities) {

            // Grab the base Prefab based on animalID from entitiesDatabaseSO
            BaseEntity actualPrefab = GameManager.Instance.entitiesDatabase.allEntities[animalInfo.animalID - 1].prefab;
            // Instantiate at respective position
            BaseEntity newCard = Instantiate(actualPrefab, 
                                            new Vector2(warbandTrans[animalInfo.position].position.x, warbandTrans[animalInfo.position].position.y), 
                                            Quaternion.identity);
            newCard.transform.SetParent(currentPanel);
            // Edit instantiated animal to suit Battle Scene
            newCard.transform.localScale = new Vector3(0.85f, 0.95f, 0.85f); // Change Scale
            Destroy(newCard.GetComponent<TooltipTrigger>()); // Remove tooltips
            Destroy(newCard.GetComponent<DragHandler>()); // Remove DragHandler
            newCard.transform.Find("TierBG").gameObject.SetActive(false); // Remove TierBG
            newCard.SetStats(animalInfo.attack, animalInfo.health); // Update Stats Accordingly
        }
    }

    /// <summary>
    /// Helper function to set the outcome text correctly in BattleManager
    /// </summary>
    public void SetOutcomeText(string outcome, string connector, int turnNumber, string punctuation) {
        outcomeText.text = string.Format("You {0} the game {1} {2} turns{3}", outcome, connector, turnNumber, punctuation);
    }

    /// <summary>
    /// Loads the Main Menu Scene on click
    /// Resets all stats
    /// </summary>
    public void OnMainMenuClick() {
        SoundManager.Instance.Play("Click");
        // Reset all the stats in PlayerData
        PlayerData.Instance.ResetAllStats();
        // Load Main Menu Scene
        SceneController.Instance.LoadScene("Scenes/Main Menu");
    }

    /// <summary>
    /// Quits the application on click
    /// Resets all stats
    /// </summary>
    public void OnQuitGameClick() {
        SoundManager.Instance.Play("Click");
        // Reset all the stats in PlayerData
        PlayerData.Instance.ResetAllStats();
        // Quit application, but won't work in Unity Play Mode (need to build externally)
        Application.Quit();
        Debug.Log("Exited Game");
    }
}
