using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    
    public static bool isGamePaused = false;

    public GameObject pauseMenu;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    public const string MIXER_MUSIC = "MusicVolume";
    public static PauseMenu Instance;

    void Awake() {
        Instance = this;
        pauseMenu.SetActive(false);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }
    
    void Start() {
        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MUSIC_KEY, 1f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Deactivates Pause Menu and unfreezes background
    /// </summary>
    public void ResumeGame() {
        // If player enters Pause Menu through clicking button and NOT "escape" key
        if (!Input.GetKeyDown(KeyCode.Escape)) {
            SoundManager.Instance.Play("Click");
        }
        pauseMenu.SetActive(false);
        // Reset the timescale back to 1 (Unfreeze background)
        Time.timeScale = 1f;
        isGamePaused = false;
        PlayerPrefs.SetFloat(SoundManager.MUSIC_KEY, musicSlider.value);
    }

    /// <summary>
    /// Activates Pause Menu and freezes background
    /// </summary>
    public void PauseGame() {
        pauseMenu.SetActive(true);
        // Set the timescale to 0 (Freeze background)
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    /// <summary>
    /// Loads Main Menu On Click and resets player data
    /// </summary>
    public void LoadMenu() {
        SoundManager.Instance.Play("Click");
        PlayerData.Instance.ResetAllStats();
        SceneController.Instance.LoadScene("Scenes/Main Menu");
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Quits application and resets player data
    /// </summary>
    public void QuitGame() {
        SoundManager.Instance.Play("Click");
        PlayerData.Instance.ResetAllStats();
        Application.Quit(); // Won't work in Unity. Need to build the application properly
        Debug.Log("Exited Game");
    }

    /// <summary>
    /// Sets background volume according to slider
    /// </summary>
    public void SetMusicVolume(float value) {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value)*20);
    }

}