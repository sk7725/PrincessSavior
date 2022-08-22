using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl main;

    [HideInInspector] public PlayerControl player;
    [HideInInspector] public Camera cam;
    [HideInInspector] public CameraControl camc;

    [HideInInspector] public bool scrollCamera = false;

    public Transform playerSpawn;
    public GameObject[] dialogs = { };

    private static float prevTimeScale;
    public static bool paused = false;

    private void Awake() {
        main = this; //behold, the laziest singleton(tm) 2; electric boogaloo
        player = GameObject.FindObjectOfType<PlayerControl>();
        cam = Camera.main;
        camc = GameObject.FindObjectOfType<CameraControl>();
        paused = false;
    }

    public bool DialogOpen() {
        foreach(GameObject dialog in dialogs) {
            if(dialog.activeInHierarchy) return true;
        }
        return false;
    }

    public static void Pause() {
        if(paused) return;
        paused = true;
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public static void Unpause() {
        if(!paused) return;
        paused = false;
        Time.timeScale = prevTimeScale;
    }
}
