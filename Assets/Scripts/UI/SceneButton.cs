using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour {
    [SerializeField] private string scene;
    [SerializeField] private bool fade = false;
    [SerializeField] private float fadeTime = 1.5f;

    private bool clicked = false;

    void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
        clicked = false;
    }

    private void Clicked() {
        if (clicked) return;
        clicked = true;

        if (fade) {
            if(AudioControl.main.music != null) AudioControl.main.music.FadeOut(1.2f, this);
            UI.CircleFade(false, fadeTime, () => {
                Time.timeScale = 1f;
                SceneManager.LoadSceneAsync(scene);
            });
        }
        else {
            Time.timeScale = 1f;
            SceneManager.LoadScene(scene);
        }
    }
}
