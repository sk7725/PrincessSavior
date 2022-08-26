using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour {
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private FlipbookPlayer flipbook;

    private float time;

    void Awake() {
        group.alpha = 0;
        time = 0;
    }

    void Update() {
        time += Time.unscaledDeltaTime;
        group.alpha = Mathf.Clamp01((time - 1.5f) / 2f);

        if (flipbook.ended) {
            text.text = "START";
            text.color = Color.yellow;
        }
    }
}
