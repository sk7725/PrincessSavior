using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    [SerializeField] private float zoomFOV = 120f;
    [SerializeField] private float zoomSpeed = 6f; 

    private bool pressed = false;
    private float prevFOV, current;


    void Start() {
        prevFOV = GameControl.main.cam.fieldOfView;
        current = prevFOV;
    }

    void Update() {
        current = Mathf.Lerp(current, pressed ? zoomFOV : prevFOV, zoomSpeed * Time.deltaTime);
        GameControl.main.cam.fieldOfView = current;
    }

    public void OnPointerDown(PointerEventData eventData) {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        pressed = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
        pressed = false;
    }
}
