using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private static string _prefsKey = null;

    private static int? _vMusic = null; //0-100
    private static int? _vSound = null; //0-100
    private static float? _flingSensitivity = null;
    private static bool? _holdZoom = null;
    private static float? _time = null;
    private static int? _lastLevel = null;

    public static string PrefsKey {
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
                AudioControl.main.SetMusic(_vMusic.Value / 100f);
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

    public static bool HoldZoomDown {
        get {
            if (_holdZoom == null) {
                _holdZoom = PlayerPrefs.GetInt($"{PrefsKey}.holdzoom", 0) == 1;
            }

            return _holdZoom.Value;
        }
        set {
            if (_holdZoom != value) {
                _holdZoom = value;
                PlayerPrefs.SetInt($"{PrefsKey}.holdzoom", value ? 1 : 0);
            }
        }
    }

    public static float TimeRecord {
        get {
            if (_time == null) {
                _time = PlayerPrefs.GetFloat($"{PrefsKey}.time", -1f);
            }

            return _time.Value;
        }
        set {
            if (_time != value) {
                _time = value;
                PlayerPrefs.SetFloat($"{PrefsKey}.time", value);
            }
        }
    }

    public static int LastLevel {
        get {
            if (_lastLevel == null) {
                _lastLevel = PlayerPrefs.GetInt($"{PrefsKey}.ll", 0);
            }

            return _lastLevel.Value;
        }
        set {
            if (_lastLevel != value) {
                _lastLevel = value;
                PlayerPrefs.GetInt($"{PrefsKey}.ll", value);
            }
        }
    }

    /*
    public static float BestTimeRecord {
        get {
            if (_bestTime == null) {
                _bestTime = PlayerPrefs.GetFloat($"{PrefsKey}.btime", -1f);
            }

            return _bestTime.Value;
        }
        set {
            if (_bestTime != value) {
                _bestTime = value;
                PlayerPrefs.SetFloat($"{PrefsKey}.btime", value);
            }
        }
    }

    public static float BestPearlTimeRecord {
        get {
            if (_bestTimePearl == null) {
                _bestTimePearl = PlayerPrefs.GetFloat($"{PrefsKey}.bptime", -1f);
            }

            return _bestTimePearl.Value;
        }
        set {
            if (_bestTimePearl != value) {
                _bestTimePearl = value;
                PlayerPrefs.SetFloat($"{PrefsKey}.bptime", value);
            }
        }
    }*/


    /*public static bool SawIntro {
        get {
            if (_intro == null) {
                _intro = PlayerPrefs.GetInt($"{PrefsKey}.sawintro", 0) == 1;
            }

            return _intro.Value;
        }
        set {
            if (_intro != value) {
                _intro = value;
                PlayerPrefs.SetInt($"{PrefsKey}.sawintro", value ? 1 : 0);
            }
        }
    }*/
}
