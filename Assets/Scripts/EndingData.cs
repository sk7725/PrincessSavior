using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingData : MonoBehaviour {
    public Blade blade;
    public Handle handle;
    public Accessory accessory;
    public int gems;
    public int swordParts;

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
    }
}
