using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashProjector : MonoBehaviour
{
    [SerializeField] private Projector projector;

    private float time = 0f, duration = 1f;
    private Gradient color;

    private void Awake() {
        projector.material = new Material(projector.material);
    }

    void Update()
    {
        if(time <= 0f) {
            if (projector.enabled) {
                projector.enabled = false;
                gameObject.SetActive(false);
            }
        }
        else {
            if (!projector.enabled) projector.enabled = true;
            float f = 1f - time / duration;
            projector.material.color = color.Evaluate(Mathf.Clamp01(f));
            time -= Time.deltaTime;
        }
    }

    public void Set(Gradient color, float duration) {
        gameObject.SetActive(true);
        this.duration = time = duration;
        this.color = color;
    }
}
