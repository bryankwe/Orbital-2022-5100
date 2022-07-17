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
    public ParticleSystem confettiParticleSystem;
    bool hasPlayedAnimations = false;

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
    /// Not sure why I cannot call this from BattleManager / OnEnable(). Only works when I call in Update()
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

            // Trivial Check if Number of Trophies Displayed is more than 5 (Not needed)
            /*if (i < totalWinsNeeded) {
                wins[i].enabled = true;
            } else {
                wins[i].enabled = false;
            }*/
        }

        // Logic for hearts display
        for (int i = 0; i < lives.Length; i++) {

            // Set heart color based on number of lives
            if (i < currentLives) {
                lives[i].color = filledHeartColor;
            } else {
                lives[i].color = emptyHeartColor;
            }

            /*if (i < totalLives) {
                lives[i].enabled = true;
            } else {
                lives[i].enabled = false;
            }*/
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

    public void SetOutcomeText(string outcome, string connector, int turnNumber, string punctuation) {
        outcomeText.text = string.Format("You {0} the game {1} {2} turns{3}", outcome, connector, turnNumber, punctuation);
    }

    public void OnMainMenuClick() {
        SoundManager.Instance.Play("Click");
        // Reset all the stats in PlayerData
        PlayerData.Instance.ResetAllStats();
        // Load Main Menu Scene
        SceneController.Instance.LoadScene("Scenes/Main Menu");
    }

    public void OnQuitGameClick() {
        SoundManager.Instance.Play("Click");
        // Reset all the stats in PlayerData
        PlayerData.Instance.ResetAllStats();
        // Quit application, but won't work in Unity Play Mode (need to build externally)
        Application.Quit();
        Debug.Log("Exited Game");
    }
}
