using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, costText;
    [SerializeField] private Button yesb, nob;

    private SwordPart part = null;
    private bool clicked = false;

    private void Start() {
        yesb.onClick.AddListener(ClickedYes);
        nob.onClick.AddListener(ClickedNo);
    }
    public void Set(SwordPart part) {
        this.part = part;
        clicked = false;
        nameText.text = part.localized;
        costText.text = part.cost.ToString();
        yesb.interactable = GameControl.main.player.coins >= part.cost;
    }

    private void Update() {
        if (part == null || clicked) return;
        yesb.interactable = GameControl.main.player.coins >= part.cost;
    }

    private void ClickedYes() {
        if (part == null || clicked) return;
        clicked = true;
        GameControl.main.player.swordPopupActive = false;
        GameControl.main.player.coins -= part.cost;
        part.EquipPlayer();
        
        StartCoroutine(IClose());
    }

    private void ClickedNo() {
        if (part == null || clicked) return;
        clicked = true;
        GameControl.main.player.swordPopupActive = false;
        StartCoroutine(IClose());
    }

    private IEnumerator IClose() {
        float duration = 0.5f;
        float t = 0;
        while(t < duration) {
            yield return null;
            t += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Clamp01(1 - t / duration);
        }

        Destroy(gameObject);
    }
}
