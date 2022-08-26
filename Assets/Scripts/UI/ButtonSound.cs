using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[AddComponentMenu("UI/Button Sound", 10)]
#endif
public class ButtonSound : MonoBehaviour {
    public AudioClip clip;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked() {
        AudioControl.Broadcast(clip);
    }

#if UNITY_EDITOR
    private void Reset() {
        if (clip != null) return;
        string[] cguids = AssetDatabase.FindAssets("click t:audioClip", new string[] { "Assets/Sounds" });
        if (cguids.Length <= 0) return;
        string path = AssetDatabase.GUIDToAssetPath(cguids[0]);
        clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }
#endif
}
