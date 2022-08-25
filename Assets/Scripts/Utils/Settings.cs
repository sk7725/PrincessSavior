using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private static string _prefsKey = null;

    private static int? _vMusic = null; //0-100
    private static int? _vSound = null; //0-100
    private static float? _flingSensitivity = null;

    private static string PrefsKey {
        get {
            if (string.IsNullOrEmpty(_prefsKey)) {
                _prefsKey = $"{SystemInfo.deviceUniqueIdentifier}.{Application.companyName}.{Application.productName}.settings";
            }

            return _prefsKey;
        }
    }

    public static int VolumeSound {
        get {
            if (_vSound == null) {
                _vSound = PlayerPrefs.GetInt($"{PrefsKey}.vsound", 100);
            }

            return _vSound.Value;
        }
        set {
            if (_vSound != value) {
                _vSound = value;
                PlayerPrefs.SetInt($"{PrefsKey}.vsound", value);
                AudioControl.main.SetSound(_vSound.Value / 100f);
            }
        }
    }

    public static int VolumeMusic {
        get {
            if (_vMusic == null) {
                _vMusic = PlayerPrefs.GetInt($"{PrefsKey}.vmusic", 100);
            }

            return _vMusic.Value;
        }
        set {
            if (_vMusic != value) {
                _vMusic = value;
                PlayerPrefs.SetInt($"{PrefsKey}.vmusic", value);
                AudioControl.main.SetMusic(-_vMusic.Value / 100f);
            }
        }
    }

    public static float FlingSensitivity {
        get {
            if (_flingSensitivity == null) {
                _flingSensitivity = PlayerPrefs.GetFloat($"{PrefsKey}.flings", 0.4f);
            }

            return _flingSensitivity.Value;
        }
        set {
            if (_flingSensitivity != value) {
                _flingSensitivity = value;
                PlayerPrefs.SetFloat($"{PrefsKey}.flings", value);
            }
        }
    }
}
