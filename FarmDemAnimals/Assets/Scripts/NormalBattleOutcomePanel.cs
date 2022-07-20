using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NormalBattleOutcomePanel : MonoBehaviour {

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

    public TextMeshProUGUI outcomeText;
    public TextMeshProUGUI continueText;
    
    public bool outcomeIsWin = false; // variable to signal if the panel is supposed to show win or lose effects
    bool hasPlayedAnimations = false; // variable to signal whether effects have been played

    void Start() {
        
        // Reference Wins to Trophies in PlayerData
        currentWins = PlayerData.Instance.Trophies;
        totalWinsNeeded = PlayerData.Instance.maxTrophies;
        
        // Reference Lives to Lives in Player Data
        currentLives = PlayerData.Instance.Lives;
        totalLives = PlayerData.Instance.maxLives;
    }
    
    /// <summary>
    /// Plays animations and sound effects accordingly to the battle outcome
    /// Played only ONCE
    /// </summary>
    public void PlayAnimations() {
        if (outcomeIsWin == true && !hasPlayedAnimations) {
            SoundManager.Instance.Play("Win");
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
    /// Helper function to set the outcome text correctly in BattleManager
    /// </summary>
    public void SetOutcomeText(string outcome, string punctuation) {
        outcomeText.text = string.Format("You {0} the battle{1}", outcome, punctuation);
    }

    /// <summary>
    /// Helper function to set the continue text correctly in BattleManager
    /// </summary>
    public void SetContinueText(int turnNumber) {
        continueText.text = string.Format("Proceed to Turn {0}", turnNumber);
    }

    /// <summary>
    /// Loads the Preparation Scene on click
    /// Resets the Gold Amount and increases the Turn Number by 1
    /// </summary>
    public void OnContinueClick() {
        SoundManager.Instance.Play("Click");
        PlayerData.Instance.IncreaseTurnNumber();
        PlayerData.Instance.ResetMoney();
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }
}
