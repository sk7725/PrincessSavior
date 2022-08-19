using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    [SerializeField] private float zoomOffset = -10f;
    [SerializeField] private float zoomSpeed = 6f; 

    private bool pressed = false;
    private float prevOffset, current;


    void Start() {
        prevOffset = GameControl.main.cam.transform.localPosition.z;
        current = prevOffset;
    }

    void Update() {
        current = Mathf.Lerp(current, pressed ? zoomOffset : prevOffset, zoomSpeed * Time.deltaTime);
        Vector3 v = GameControl.main.cam.transform.localPosition;
        v.z = current;
        GameControl.main.cam.transform.localPosition = v;
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
