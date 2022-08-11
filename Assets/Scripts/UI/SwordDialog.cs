using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwordDialog : Dialog {
    [SerializeField] private TextMeshProUGUI bladen, bladeDesc, handlen, handleDesc, accn, accDesc;
    [SerializeField] private Button bgListener, closeButton;

    private void Start() {
        bgListener.onClick.AddListener(Close);
        closeButton.onClick.AddListener(Close);
    }

    public override void Build() {
        PlayerControl player = GameControl.main.player;

        bladen.text = player.blade.localized;
        bladeDesc.text = player.blade.Description();
        handlen.text = player.handle.localized;
        handleDesc.text = player.handle.Description();
        accn.text = player.accessory.localized;
        accDesc.text = player.accessory.Description();
        accn.color = player.accessory.color;
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
