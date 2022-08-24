using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour {
    public AudioMixer mixer;

    private void Start() {

    }

    private void Update() {

    }

    private void SetSound(float volume) {
        mixer.SetFloat("vSound", ToDecibel(volume));
    }

    private void SetMusic(float volume) {
        mixer.SetFloat("vMusic", ToDecibel(volume));
    }

    public static float ToDecibel(float volume) {
        return Mathf.Log10(volume) * 20;
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Setup Audio Source Mixers", false)]
    private static void SetupSound() {
        AudioMixer mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/AudioMixer.mixer");
        AudioMixerGroup soundg = mixer.FindMatchingGroups("Master/Sound")[0];

        //SEARCH PREFABS
        int n = 0;
        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (var guid in guids) {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (go.GetComponentInChildren<AudioSource>() != null) {
                //this boi has an audiosource
                using (var editingScope = new PrefabUtility.EditPrefabContentsScope(path)) {
                    GameObject prefab = editingScope.prefabContentsRoot;
                    AudioSource[] audios = prefab.GetComponentsInChildren<AudioSource>();
                    foreach (AudioSource audio in audios) {
                        if (audio.outputAudioMixerGroup == null) {
                            audio.outputAudioMixerGroup = soundg;
                            n++;
                            Debug.Log(string.Format("Set Audio Source of prefab {0}", go.name), go);
                        }
                    }
                }
            }
        }
        Debug.Log(string.Format("Set {0} prefab Audio Sources to use the sound mixer group.", n));

        //SEARCH HIERARCHY
        n = 0;
        AudioSource[] audioi = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioi) {
            if (audio.outputAudioMixerGroup == null) {
                audio.outputAudioMixerGroup = soundg;
                n++;
                Debug.Log(string.Format("Set Audio Source of object {0}", audio.gameObject.name), audio.gameObject);
            }
        }
        Debug.Log(string.Format("Set {0} hierarchy Audio Sources to use the sound mixer group.", n));
    }
#endif
}
