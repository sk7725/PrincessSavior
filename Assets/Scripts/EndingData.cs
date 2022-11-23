using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static LevelUtils;

/// <summary>
/// Stored data about the last game. Is created on level clear (by DanaDoor); is destroyed when a new EndingData is instantiated.
/// </summary>
public class EndingData : MonoBehaviour {
    public Blade blade;
    public Handle handle;
    public Accessory accessory;
    public int gems, maxGems;
    public int swordParts;
    public Level level;

    public static void NewEndingData() {
        GameObject e = GameObject.FindGameObjectWithTag("LastEndingData");
        if (e != null) Destroy(e);
        e = new GameObject("EndingData");
        e.tag = "LastEndingData";
        e.AddComponent<EndingData>();
        DontDestroyOnLoad(e);
    }

    public static EndingData GetEndingData() {
        return GameObject.FindGameObjectWithTag("LastEndingData").GetComponent<EndingData>();
    }

    public void Start() {
        PlayerControl player = GameControl.main.player;
        blade = player.blade;
        handle = player.handle;
        accessory = player.accessory;
        gems = player.gems;
        maxGems = player.maxGems;

        LevelData ld = LevelData.Current();
        level = ld.level;
        Destroy(ld.gameObject);
    }

    public RecordType GetRecordType() {
        if (accessory.name == "TrialPearl") {
            if (gems >= maxGems) return RecordType.pearlPerfect;
            return RecordType.pearl;
        }
        else {
            if (gems >= maxGems) return RecordType.perfect;
            return RecordType.normal;
        }
    }
}
