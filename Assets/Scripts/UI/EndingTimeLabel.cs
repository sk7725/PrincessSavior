using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingTimeLabel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] [Multiline] private string format = "{0}\n<color=yellow>{1}</color>";
    [SerializeField] private FlipbookPlayer flipbook;

    void Start() {
        time.enabled = false;
        foreach (Transform t in transform) {
            t.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (flipbook.ended && !time.enabled) {
            time.enabled = true;
            foreach (Transform t in transform) {
                t.gameObject.SetActive(true);
            }
            EndingData endingData = EndingData.GetEndingData();

            string f = format;
            string bestTime = "";
            if (endingData.accessory.name == "TrialPearl") {
                f = f.Replace("<color=yellow>", "<color=red>");
            }
            bestTime = LevelUtils.GetRecord(endingData.level.id, endingData.GetRecordType()).ToTimeString();
            time.text = string.Format(f, Settings.TimeRecord.ToTimeString(), bestTime, endingData.gems, endingData.maxGems);
        }
    }
}
