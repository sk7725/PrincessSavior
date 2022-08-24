using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPlatform : MonoBehaviour {
    public GameObject render;

    public float duration = 3f, shakeDuration = 2f, respawn = 3f;
    public float shake = 0.2f;
    public GameObject breakFx;

    private float time;
    private bool playerOn = false, disabled = false;

    void Start() {
        time = 0f;
        playerOn = disabled = false;
        render.SetActive(true);
    }

    void Update() {
        if (disabled) return;

        if (playerOn) {
            time += Time.deltaTime;

            //shake
            if(time > duration - shakeDuration) {
                render.transform.localPosition = new Vector3(Random.Range(-shake, shake), Random.Range(-shake, shake), 0f);
            }

            if(time > duration) {
                disabled = true;
                playerOn = false;
                GameControl.main.player.Fx(breakFx, transform.position, transform.rotation);
                GetComponent<Collider>().enabled = false;
                render.SetActive(false);
                Invoke("Respawn", respawn);
            }
        }
        else{
            if (time > 0) {
                time -= Time.deltaTime;
                if (time < 0) time = 0;
            }
            render.transform.localPosition = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.CompareTag("Player")) playerOn = true;
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.collider.CompareTag("Player")) playerOn = false;
    }

    public void Respawn() {
        if (!disabled) return;
        time = 0;
        disabled = false;
        GetComponent<Collider>().enabled = true;
        render.SetActive(true);
    }
}
