using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the current level played. Is created on level select; is destroyed when EndingData is instantiated (by EndingData).
/// </summary>
public class LevelData : MonoBehaviour
{
    public Level level;

    public static void New(Level level) {
        GameObject e = GameObject.FindGameObjectWithTag("LastLevelData");
        if (e != null) Destroy(e);
        e = new GameObject("LevelData");
        e.tag = "LastLevelData";
        e.AddComponent<LevelData>().level = level;
        DontDestroyOnLoad(e);
    }

    public static LevelData Current() {
        return GameObject.FindGameObjectWithTag("LastLevelData").GetComponent<LevelData>();
    }
}
