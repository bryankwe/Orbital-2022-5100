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

    public void ResumeGame() {
        if (!Input.GetKeyDown(KeyCode.Escape)) {
            SoundManager.Instance.Play("Click");
        }
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        PlayerPrefs.SetFloat(SoundManager.MUSIC_KEY, musicSlider.value);
    }

    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu() {
        SoundManager.Instance.Play("Click");
        PlayerData.Instance.ResetAllStats();
        SceneController.Instance.LoadScene("Scenes/Main Menu");
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        SoundManager.Instance.Play("Click");
        PlayerData.Instance.ResetAllStats();
        Application.Quit(); // Won't work in Unity. Need to build the application properly
        Debug.Log("Exited Game");
    }

    public void SetMusicVolume(float value) {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value)*20);
    }

}