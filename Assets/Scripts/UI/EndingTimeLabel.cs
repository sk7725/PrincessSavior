using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingTimeLabel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private string format = "{0}\n<color=yellow>{1}</color>";
    [SerializeField] private FlipbookPlayer flipbook;

    void Start() {
        time.enabled = false;
    }

    void Update() {
        if (flipbook.ended && !time.enabled) {
            time.enabled = true;
            string f = format;
            if (EndingData.GetEndingData().accessory.name == "TrialPearl") {
                f = f.Replace("<color=yellow>", "<color=red>");
                time.text = string.Format(f, Settings.TimeRecord.ToTimeString(), Settings.BestPearlTimeRecord.ToTimeString());
            }
            else {
                time.text = string.Format(f, Settings.TimeRecord.ToTimeString(), Settings.BestTimeRecord.ToTimeString());
            }
        }
    }
}
