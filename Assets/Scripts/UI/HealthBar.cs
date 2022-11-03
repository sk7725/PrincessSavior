using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private GameObject visualFrame;
    [SerializeField] private Image[] hearts;
    [SerializeField] private float showTime = 4f; //Iiiiiiit's showtime!

    private float time = 0;
    private float deltaHp, zOffset;

    void Start() {
        visualFrame.SetActive(false);
        Set(1);
        time = 0f;
        deltaHp = GameControl.main.player.health;
    }

    void Update() {
        transform.rotation = Quaternion.identity;

        if(Mathf.Abs(GameControl.main.player.health - deltaHp) > 0.1f) {
            deltaHp = GameControl.main.player.health;
            time = showTime;
        }

        if (time > 0) {
            time -= Time.deltaTime;
            Set(GameControl.main.player.health / GameControl.main.player.maxHealth);
            visualFrame.SetActive(true);
            if (time <= 0) {
                visualFrame.SetActive(false);
            }
        }
    }

    private void Set(float f) {
        float unit = 1f / hearts.Length;

        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].fillAmount = Mathf.Clamp01(f / unit);
            f -= unit;
        }
    }
}
