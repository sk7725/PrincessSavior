using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBroadcaster : MonoBehaviour {
    private void Awake() {
        tag = "AudioBroadcaster";
        DontDestroyOnLoad(gameObject);
    }
}
