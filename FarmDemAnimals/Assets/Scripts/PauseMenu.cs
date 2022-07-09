using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Manager<PauseMenu> {
    
    public static bool isGamePaused = false;

    public GameObject pauseMenu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu() {
        SceneController.Instance.LoadScene("Scenes/Main Menu");
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit(); // Won't work in Unity. Need to build the application properly
        Debug.Log("Exited Game");
    }
}