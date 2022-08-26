using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour {
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private FlipbookPlayer flipbook;
    [SerializeField] private string stringEnded = "START";

    [SerializeField] private string scene;
    [SerializeField] private bool fade = false;

    private bool clicked = false;

    private float time;

    void Awake() {
        group.alpha = 0;
        time = 0;
        GetComponent<Button>().onClick.AddListener(Clicked);
        clicked = false;
    }

    void Update() {
        time += Time.unscaledDeltaTime;
        group.alpha = Mathf.Clamp01((time - 1.5f) / 2f);

        if (flipbook.ended) {
            text.text =stringEnded;
            text.color = Color.yellow;
        }
    }

    private void Clicked() {
        if(flipbook.ended) NextScene();
        flipbook.ended = true;
        flipbook.End();
    }

    private void NextScene() {
        if (clicked) return;
        clicked = true;

        if (fade) {
            if (AudioControl.main.music != null) AudioControl.main.music.FadeOut(1.2f, this);
            UI.CircleFade(false, 1.5f, () => {
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
