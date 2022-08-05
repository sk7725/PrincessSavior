using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl main;

    [HideInInspector] public PlayerControl player;
    [HideInInspector] public Camera cam;

    public bool scrollCamera = false;

    private void Awake() {
        main = this; //behold, the laziest singleton(tm) 2; electric boogaloo
        player = GameObject.FindObjectOfType<PlayerControl>();
        cam = Camera.main;
    }
}
