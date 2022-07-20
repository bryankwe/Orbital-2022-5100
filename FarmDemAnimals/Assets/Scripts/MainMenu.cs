using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TutorialPanel;
    
    public void Start() {
        TutorialPanel.SetActive(false);
    }

    /// <summary>
    /// Changes scene to Preparation Scene upon click
    /// </summary>
    public void OnPlayClick() {
        SoundManager.Instance.Play("Click");
        SceneController.Instance.LoadScene("Scenes/Preparation Scene");
    }

    /// <summary>
    /// Quits the application upon click
    /// </summary>
    public void OnQuitGameClick() {
        SoundManager.Instance.Play("Click");
        Application.Quit();
        Debug.Log("Exited the game...");
    }

    /// <summary>
    /// Launches the Help Panel upon click
    /// </summary>
    public void OnHelpClick() {
        SoundManager.Instance.Play("Click");
        TutorialPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the Help Panel upon click
    /// </summary>
    /// <remarks>
    /// Will still be at the Main Menu Scene
    /// </remarks>
    public void OnDoneClick() {
        SoundManager.Instance.Play("Click");
        TutorialPanel.SetActive(false);
    }
}
