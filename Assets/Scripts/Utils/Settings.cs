using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private static string _prefsKey = null;

    private static float? _vMusic = null;
    private static float? _vSound = null;
    private static float? _flingSensitivity = null;

    private static string PrefsKey {
        get {
            if (string.IsNullOrEmpty(_prefsKey)) {
                _prefsKey = $"{SystemInfo.deviceUniqueIdentifier}.{Application.companyName}.{Application.productName}.settings";
            }

            return _prefsKey;
        }
    }

    public static float VolumeSound {
        get {
            if (_vSound == null) {
                _vSound = PlayerPrefs.GetFloat($"{PrefsKey}.vsound", 1f);
            }

            return _vSound.Value;
        }
        set {
            if (_vSound != value) {
                _vSound = value;
                PlayerPrefs.SetFloat($"{PrefsKey}.vsound", value);
            }
        }
    }

    public static float VolumeMusic {
        get {
            if (_vMusic == null) {
                _vMusic = PlayerPrefs.GetFloat($"{PrefsKey}.vmusic", 1f);
            }

            return _vMusic.Value;
        }
        set {
            if (_vMusic != value) {
                _vMusic = value;
                PlayerPrefs.SetFloat($"{PrefsKey}.vmusic", value);
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
