using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, costText;
    [SerializeField] private Button yesb, nob;
    [SerializeField] private AudioClip yesSound;

    private SwordPart part = null;
    private bool clicked = false;
    private bool wasIdle = false;

    private void Awake() {
        transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>().transform, false);
    }

    private void Start() {
        yesb.onClick.AddListener(ClickedYes);
        nob.onClick.AddListener(ClickedNo);
        wasIdle = false;
    }

    public void Set(SwordPart part) {
        this.part = part;
        clicked = false;
        nameText.text = part.localized;
        costText.text = part.cost.ToString();
        yesb.interactable = GameControl.main.player.coins >= part.cost && GameControl.main.player.state == PlayerControl.State.idle;
        nob.interactable = GameControl.main.player.state == PlayerControl.State.idle;
        StartCoroutine(IOpen());
    }

    private void Update() {
        if (part == null || clicked) return;
        yesb.interactable = GameControl.main.player.coins >= part.cost && GameControl.main.player.state == PlayerControl.State.idle;
        nob.interactable = GameControl.main.player.state == PlayerControl.State.idle;
        if (GameControl.main.player.dead) ClickedNo();

        if (wasIdle) {
            if (GameControl.main.player.state != PlayerControl.State.idle) ClickedNo();
        }
        else if (GameControl.main.player.state == PlayerControl.State.idle) wasIdle = true;
    }

    private void ClickedYes() {
        if (part == null || clicked) return;
        clicked = true;
        GameControl.main.player.swordPopupActive = false;
        GameControl.main.player.coins -= part.cost;
        part.EquipPlayer();
        GameControl.main.player.OnPartUpdate();
        GameControl.main.player.audios.PlayOneShot(yesSound);

        StartCoroutine(IClose());
    }

    private void ClickedNo() {
        if (part == null || clicked) return;
        clicked = true;
        GameControl.main.player.swordPopupActive = false;
        StartCoroutine(IClose());
    }

    private IEnumerator IOpen() {
        transform.localScale = Vector3.zero;
        float duration = 0.3f;
        float t = 0;
        while (t < duration) {
            yield return null;
            t += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Clamp01(t / duration);
        }

        transform.localScale = Vector3.one;
    }

    private IEnumerator IClose() {
        float duration = 0.2f;
        float t = 0;
        while(t < duration) {
            yield return null;
            t += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Clamp01(1 - t / duration);
        }

        Destroy(gameObject);
    }
}
