using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private static string _prefsKey = null;

    private static float? _flingSensitivity = null;

    private static string PrefsKey {
        get {
            if (string.IsNullOrEmpty(_prefsKey)) {
                _prefsKey = $"{SystemInfo.deviceUniqueIdentifier}.{Application.companyName}.{Application.productName}.settings";
            }

            return _prefsKey;
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
