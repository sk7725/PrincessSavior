using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public static UI main;
    public UIPrefabs prefabs;

    private void Awake() {
        main = this;
    }

    public static void PartPopup(SwordPart part) {
        PartPopup o = Instantiate(main.prefabs.partPopup).GetComponent<PartPopup>();
        o.Set(part);
    }

    public static void CircleFade() {
        Instantiate(main.prefabs.circleFade);
    }

    public static void CircleFade(bool fadeout, float duration = 1.5f, System.Action endAction = null) {
        CircleFade o = Instantiate(main.prefabs.circleFade).GetComponent<CircleFade>();
        o.fadeout = fadeout;
        o.duration = duration;
        o.endAction = endAction;
    }

    [System.Serializable]
    public class UIPrefabs {
        public GameObject partPopup, circleFade;
    }
}
