using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneController : SingletonManager<SceneController> {

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image progressBar;
    private float target;

    public async void LoadScene(string sceneName) {
        target = 0;
        progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loaderCanvas.SetActive(true);

        do {
            await Task.Delay(100);
            // If Task.Delay() causes error in webGL, try Yield()
            //await Task.Yield();
            target = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(500);
        // If Task.Delay() causes error in webGL, try Yield()
        //await Task.Yield();

        scene.allowSceneActivation = true;
        loaderCanvas.SetActive(false);
        //Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name);
    }

    void Update() {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }

    /*public Image fader;

    void Start() {
        fader.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
        fader.gameObject.SetActive(false);
    }
    
    public static void LoadScene(int index, float duration = 1, float waitTime = 0) {
        Instance.StartCoroutine(Instance.FadeScene(index, duration, waitTime));
    }

    private IEnumerator FadeScene(int index, float duration, float waitTime) {
        
        fader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / duration) {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }

        AsyncOperation ao = SceneManager.LoadSceneAsync(index);

        while (!ao.isDone) {
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        for (float t = 0; t < 1; t += Time.deltaTime / duration) {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        fader.gameObject.SetActive(false);
    }*/
}