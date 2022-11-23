using UnityEngine;
using UnityEngine.SceneManagement;

using static Settings;

public static class LevelUtils {
    public enum RecordType {
        normal,
        perfect,
        pearl,
        pearlPerfect
    }

    public static void StartLevel(Level level) {
        LevelData.New(level);
        SceneManager.LoadSceneAsync(level.introScene);
    }

    public static string RecordKey(int levelID, RecordType type) {
        return $"{PrefsKey}.{levelID}.{(int)type}.btime";
    }

    public static void TryUpdateRecord(float time, int levelID, RecordType type) {
        if (time <= 15f) return;
        if (levelID < 0) {
            Debug.LogError("Level with ID < 0 cannot be saved!");
            return;
        }
        float record = GetRecord(levelID, type);

        if (record < 15f || record > time) SetRecord(levelID, type, time);
    }

    public static float GetRecord(int levelID, RecordType type) {
        return PlayerPrefs.GetFloat(RecordKey(levelID, type), -1f);
    }

    public static void SetRecord(int levelID, RecordType type, float value) {
        if (levelID < 0) {
            Debug.LogError("Level with ID < 0 cannot be saved!");
            return;
        }
        PlayerPrefs.SetFloat(RecordKey(levelID, type), value);
    }

    public static bool GetCleared(int levelID) {
        return PlayerPrefs.GetInt($"{PrefsKey}.{levelID}.clr", 0) == 1;
    }

    public static void SetCleared(int levelID, bool value) {
        if (levelID < 0) {
            Debug.LogError("Level with ID < 0 cannot be saved!");
            return;
        }
        PlayerPrefs.SetInt($"{PrefsKey}.{levelID}.clr", value ? 1 : 0);
    }

    public static void TryUpdateGems(int levelID, int gems) {
        if (gems <= 0) return;
        if (levelID < 0) {
            Debug.LogError("Level with ID < 0 cannot be saved!");
            return;
        }
        int record = GetGems(levelID);

        if (record < gems) SetGems(levelID, gems);
    }
    public static int GetGems(int levelID) {
        return PlayerPrefs.GetInt($"{PrefsKey}.{levelID}.g", 0);
    }

    public static void SetGems(int levelID, int value) {
        if (levelID < 0) {
            Debug.LogError("Level with ID < 0 cannot be saved!");
            return;
        }
        PlayerPrefs.SetInt($"{PrefsKey}.{levelID}.g", value);
    }
}
