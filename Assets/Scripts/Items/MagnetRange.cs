using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetRange : MonoBehaviour
{
    [SerializeField] private Collider col;
    [System.NonSerialized] public Item item;

    private void OnTriggerEnter(Collider other) {
        if (!GameControl.main.player.dead && (other.CompareTag("Player") || other.CompareTag("Sword"))) {
            if (item.DoMagnet()) {
                item.EnableMagnet();
                col.enabled = false;
            }
        }
    }

    public void Unmagnetize() {
        col.enabled = true;
    }
}
