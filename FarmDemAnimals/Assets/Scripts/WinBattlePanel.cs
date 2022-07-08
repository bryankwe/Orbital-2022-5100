using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinBattlePanel : MonoBehaviour {
    private int currentWins;
    private int totalWinsNeeded;
    private int currentLives;
    private int totalLives;

    public Image[] wins;
    public Image[] lives;
    private Color filledTrophyColor = new Color(1.0f, 1.0f, 1.0f); // Original color (Yellow?)
    private Color emptyTrophyColor = new Color(0f, 0f, 0f); // Color.black;
    private Color filledHeartColor = new Color(1.0f, 0f, 0f); // Color.red;
    private Color emptyHeartColor = new Color(233.0f/255.0f, 233.0f/255.0f, 233.0f/255.0f); // Color.grey;

    void Start() {
        
        // Reference Wins to Trophies in PlayerData
        currentWins = PlayerData.Instance.Trophies;
        totalWinsNeeded = PlayerData.Instance.maxTrophies;
        
        // Reference Lives to Lives in Player Data
        currentLives = PlayerData.Instance.Lives;
        totalLives = PlayerData.Instance.maxLives;
    }
    
    void Update() {

        // Logic for trophies display
        for (int i = 0; i < wins.Length; i++) {

            if (i < currentWins) {
                wins[i].color = filledTrophyColor;
            } else {
                wins[i].color = emptyTrophyColor;
            }

            if (i < totalWinsNeeded) {
                wins[i].enabled = true;
            } else {
                wins[i].enabled = false;
            }
        }

        // Logic for hearts display
        for (int i = 0; i < lives.Length; i++) {

            if (i < currentLives) {
                lives[i].color = filledHeartColor;
            } else {
                lives[i].color = emptyHeartColor;
            }

            if (i < totalLives) {
                lives[i].enabled = true;
            } else {
                lives[i].enabled = false;
            }
        }
    }

    public void OnContinueClick() {
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }
}
