using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingTimeLabel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI time;

    void Start() {
        time.enabled = false;
    }
}
