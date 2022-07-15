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

    public void OnPlayClick() 
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        SceneController.Instance.LoadScene("Scenes/Preparation Scene");

        //SceneController.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, 1, 2)
    }

    public void OnQuitGameClick() {
        Application.Quit();
        Debug.Log("Exited the game...");
    }

    public void OnHelpClick() {
        TutorialPanel.SetActive(true);
    }

    public void OnDoneClick() {
        TutorialPanel.SetActive(false);
    }
}
