using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    [SerializeField] private float zoomOffset = -10f;
    [SerializeField] private float extraOffset = -10f;
    [SerializeField] private float zoomSpeed = 6f;
    [SerializeField] private AudioClip clip;

    public static bool pressed = false;
    private bool toggled = false;
    private float prevOffset, current;


    void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
        prevOffset = GameControl.main.cam.transform.localPosition.z;
        current = prevOffset;
    }

    void Update() {
        bool extra = pressed && Settings.HoldZoomDown && current < zoomOffset + 0.01f;
        if (extra) {
            current -= Time.deltaTime * 2f;
            if(current < zoomOffset + extraOffset) current = zoomOffset + extraOffset;
        }
        else {
            current = Mathf.Lerp(current, Pressed() ? zoomOffset : prevOffset, zoomSpeed * Time.deltaTime);
        }
        
        Vector3 v = GameControl.main.cam.transform.localPosition;
        v.z = current;
        GameControl.main.cam.transform.localPosition = v;
    }

    private void Clicked() {
        toggled = !toggled;
        if (!Settings.HoldZoomDown) AudioControl.Broadcast(clip);
    }

    private bool Pressed() {
        return Settings.HoldZoomDown ? pressed : toggled;
    }

    public void OnPointerDown(PointerEventData eventData) {
        pressed = true;
        if (Settings.HoldZoomDown) AudioControl.Broadcast(clip);
    }

    public void OnPointerUp(PointerEventData eventData) {
        pressed = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
        //pressed = false;
    }
}
