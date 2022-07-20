using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : Manager<Stats> {

    public TextMeshProUGUI lives; // Display the number of remaining lives
    public TextMeshProUGUI wins; // Display the number of wins accummulated
    public TextMeshProUGUI turnNumber; // Displays the current turn
    public GameObject TutorialPanel;

    private void Start() {
        //Debug.Log("Enter Stats Start()");
        RefreshLives();
        RefreshWins();
        RefreshTurnNumber();
        TutorialPanel.SetActive(false);
    }

    /// <summary>
    /// Refreshes the Lives Count and Displays correctly on UI
    /// </summary>
    void RefreshLives() {
        lives.text = PlayerData.Instance.Lives.ToString();
    }

    /// <summary>
    /// Refreshes the Win Count and Displays correctly on UI
    /// </summary>
    void RefreshWins() {
        wins.text = PlayerData.Instance.Trophies.ToString();
    }

    /// <summary>
    /// Refreshes the Turn Number and Displays correctly on UI
    /// </summary>
    void RefreshTurnNumber() {
        turnNumber.text = "Turn: " + PlayerData.Instance.TurnNumber.ToString();
    }

    /// <summary>
    /// Opens Pause Menu on click
    /// </summary>
    public void OnPauseClick() {
        SoundManager.Instance.Play("Click");
        PauseMenu.Instance.PauseGame();
    }

    /// <summary>
    /// Opens Tutorial Panel on click
    /// </summary>
    public void OnHelpClick() {
        SoundManager.Instance.Play("Click");
        TutorialPanel.SetActive(true);
    }

    /// <summary>
    /// Closes Tutorial Panel on click
    /// </summary>
    public void OnDoneClick() {
        SoundManager.Instance.Play("Click");
        TutorialPanel.SetActive(false);
    }
}
