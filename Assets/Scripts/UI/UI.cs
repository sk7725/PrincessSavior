using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI main;
    public UIPrefabs prefabs;

    private void Awake() {
        main = this;
    }

    public static void PartPopup(SwordPart part) {
        PartPopup o = Instantiate(main.prefabs.partPopup).GetComponent<PartPopup>();
        o.Set(part);
    }

    [System.Serializable]
    public class UIPrefabs {
        public GameObject partPopup;
    }
}
