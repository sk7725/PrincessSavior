using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour {
    public GameObject[] afterStart;
    public RawImage flash;
    public Button startButton;
    public Sprite startPressed;
    public Image arrow;
    public AudioSource audios; //todo sound
    public AudioClip startSound;

    public float blinkRate = 1f;
    private bool started = false;

    private void Awake() {
        started = false;
        foreach (GameObject go in afterStart) {
            go.SetActive(false);
        }

        startButton.onClick.AddListener(StartClicked);
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
    }

    private void StartClicked() {
        startButton.GetComponent<Image>().sprite = startPressed;
        audios.PlayOneShot(startSound);
        started = false;
        StartCoroutine(IStartClicked());
    }

    IEnumerator IStart() {
        //todo logo movin'
        yield return new WaitForSeconds(2f);
        StartSequence();
    }

    IEnumerator IStartClicked() {
        //todo start black circle thingy, then transition scene
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("GameScene");
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
