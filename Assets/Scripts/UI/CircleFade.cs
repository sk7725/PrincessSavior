using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFade : MonoBehaviour {
    [SerializeField] private Image image;
    public bool fadeout = false;
    public float duration = 1.5f;
    public Action endAction = null;

    private Material mat;
    private float time;
    private bool ended = false;

    private void Awake() {
        transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>().transform, false);
        mat = image.material;
        mat.SetFloat("_Cutoff", fadeout ? 0 : 1);
        time = 0f;
        ended = false;
    }

    private void Update() {
        if (ended) return;
        time += Time.unscaledDeltaTime;
        if (time > duration) {
            mat.SetFloat("_Cutoff", fadeout ? 1 : 0);
            if (endAction != null) endAction();
            if(fadeout) Destroy(gameObject);
            ended = true;
        }
        else {
            mat.SetFloat("_Cutoff", fadeout ? time / duration : 1 - time / duration);
        }
    }
}
