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
        int hours = (int)(time / 60 / 60);
        //todo append zeros
        timet.text = hours > 0 ? string.Format("{0}:{1,2:D2}:{2,2:D2}.{3,3:D3}", hours, (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000))
            : string.Format("{0}:{1,2:D2}.{2,3:D3}", (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000));
        coint.text = GameControl.main.player.coins.ToString();
        coinRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, coint.preferredWidth);
    }
}
