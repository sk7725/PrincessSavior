using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shootFx;

    [SerializeField] private int shots;
    [SerializeField] private float spread = 15f;

    void Start() {
        int n = shots;
        for (int i = 0; i < n; i++) {
            float angle = - (n - 1) / 2f * spread + i * spread;
            Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(angle, 0, 0));
        }
        GameControl.main.player.Fx(shootFx, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
