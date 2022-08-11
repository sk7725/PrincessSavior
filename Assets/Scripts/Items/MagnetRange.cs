using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetRange : MonoBehaviour
{
    [System.NonSerialized] public Item item;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Sword")) {
            if(item.DoMagnet()) item.EnableMagnet();
        }
    }
}
