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

    private void Awake() {
        main = this; //behold, the laziest singleton(tm) 2; electric boogaloo
        player = GameObject.FindObjectOfType<PlayerControl>();
        cam = Camera.main;
        camc = GameObject.FindObjectOfType<CameraControl>();
    }

    public bool DialogOpen() {
        foreach(GameObject dialog in dialogs) {
            if(dialog.activeInHierarchy) return true;
        }
        return false;
    }
}
