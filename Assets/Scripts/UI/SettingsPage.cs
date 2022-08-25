using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPage : MonoBehaviour {
    [SerializeField] private SliderLabel sound, music, sensitivity;
    [SerializeField] private CheckButton holdzoom;

    private void Start() {
        sound.slider.value = Settings.VolumeSound / 100f;
        music.slider.value = Settings.VolumeMusic / 100f;
        sensitivity.slider.value = Map(Settings.FlingSensitivity, 0.1f, 1f, 0, 1);
        holdzoom.check.SetActive(Settings.HoldZoomDown);

        sound.slider.onValueChanged.AddListener(OnSoundChange);
        music.slider.onValueChanged.AddListener(OnMusicChange);
        sensitivity.slider.onValueChanged.AddListener(OnSensivityChange);
        holdzoom.button.onClick.AddListener(ToggleHoldZoom);

        sound.label.text = string.Format(sound.name, Settings.VolumeSound);
        music.label.text = string.Format(music.name, Settings.VolumeMusic);
        sensitivity.label.text = string.Format(sensitivity.name, Settings.FlingSensitivity);
    }

    private void OnSoundChange(float a) {
        int i = Mathf.RoundToInt(a * 100f);
        Settings.VolumeSound = i;
        sound.label.text = string.Format(sound.name, i);
    }

    private void OnMusicChange(float a) {
        int i = Mathf.RoundToInt(a * 100f);
        Settings.VolumeMusic = i;
        music.label.text = string.Format(music.name, i);
    }

    private void OnSensivityChange(float a) {
        float f = Map(a, 0, 1, 0.1f, 1f);
        f = Mathf.Round(f * 100) / 100f;
        Settings.FlingSensitivity = f;
        sensitivity.label.text = string.Format(sensitivity.name, f);
    }

    private void ToggleHoldZoom() {
        Settings.HoldZoomDown = !Settings.HoldZoomDown;
        holdzoom.check.SetActive(Settings.HoldZoomDown);
    }

    private float Map(float x, float a, float b, float c, float d) {
        return (x - a) / (b - a) * (d - c) + c;
    }

    [System.Serializable]
    public class SliderLabel {
        public Slider slider;
        public TextMeshProUGUI label;
        public string name;
    }

    [System.Serializable]
    public class CheckButton {
        public Button button;
        public GameObject check;
    }
}
