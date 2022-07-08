using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : Manager<Stats> {

    public TextMeshProUGUI lives; // Display the number of remaining lives
    public TextMeshProUGUI wins; // Display the number of wins accummulated
    public TextMeshProUGUI turnNumber; // Displays the current turn

    private void Start() {
        //Debug.Log("Enter Stats Start()");
        RefreshLives();
        RefreshWins();
        RefreshTurnNumber();
    }

    void RefreshLives() {
        lives.text = PlayerData.Instance.Lives.ToString();
    }

    void RefreshWins() {
        wins.text = PlayerData.Instance.Trophies.ToString();
    }

    void RefreshTurnNumber() {
        turnNumber.text = "Turn: " + PlayerData.Instance.TurnNumber.ToString();
    }

    public void OnPauseClick() {
        PauseMenu.Instance.PauseGame();
    }
}
