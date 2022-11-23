using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour {
    public GameObject logo;
    public GameObject[] afterStart;
    public RawImage flash;
    //public Button startButton;
    //public Sprite startPressed;
    public Image arrow;
    public AudioSource audios, bgm, drumroll; //todo sound
    public AudioClip startSound, titleAppearSound;

    public float blinkRate = 1f;
    private bool started = false;

    private void Awake() {
        Time.timeScale = 1f;
        started = false;
        foreach (GameObject go in afterStart) {
            go.SetActive(false);
        }
        logo.transform.localScale = Vector3.zero;

        //startButton.onClick.AddListener(StartClicked);
        StartCoroutine(IStart());
    }

    void Update() {
        if (!started) return;

        arrow.enabled = Time.time % (blinkRate * 2f) > blinkRate;
    }

    private void StartSequence() {
        started = true;
        foreach (GameObject go in afterStart) {
            go.SetActive(true);
        }
        StartCoroutine(IFlash(1f));
        drumroll.Stop();
        audios.PlayOneShot(titleAppearSound);
        bgm.Play();
    }

    /*private void StartClicked() {
        if (pressedStart) return;
        pressedStart = true;
        startButton.GetComponent<Image>().sprite = startPressed;
        audios.PlayOneShot(startSound);
        started = false;

        bgm.FadeOut(1.2f, this);
        UI.CircleFade(false, 1.5f, () => {
            //SceneManager.LoadSceneAsync(skipIntro ? "GameScene" : "IntroScene"); //todo
            SceneManager.LoadSceneAsync("IntroScene");
        });
    }*/

    IEnumerator IStart() {
        float duration = 1.5f;
        float time = 0f;
        while(time < duration) {
            time += Time.deltaTime;
            logo.transform.localScale = Vector3.one * Logof(time / duration);
            yield return null;
        }
        logo.transform.localScale = Vector3.one;
        StartSequence();
    }

    float Logof(float f) {
        float s = f * (1 - f) * 4f; //0 -> 1 -> 0
        if (f < 0.5f) return s * 1.5f;
        return 1 + s * 0.5f;
    }

    IEnumerator IFlash(float duration) {
        flash.gameObject.SetActive(true);
        flash.color = Color.white;

        float t = 0;
        while (t < duration) {
            yield return null;
            t += Time.deltaTime;
            Color c = Color.white;
            c.a = 1 - t / duration;
            flash.color = c;
        }
        flash.gameObject.SetActive(false);
    }
}
