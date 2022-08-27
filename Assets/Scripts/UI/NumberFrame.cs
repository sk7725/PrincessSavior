using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberFrame : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timet, coint;
    [SerializeField] private RectTransform coinRect;
    public static double time = 0;

    void Start() {
        time = 0;
    }

    void LateUpdate() {
        time += Time.deltaTime;
        timet.text = time.ToTimeString();
        coint.text = GameControl.main.player.coins.ToString();
        coinRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, coint.preferredWidth);
    }
}
