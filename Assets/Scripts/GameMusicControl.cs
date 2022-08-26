using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicControl : MonoBehaviour {
    public int current = 0;
    public GameMusic[] musicData;

    private bool transition = false;
    private int nextid;

    void Start() {
        AudioControl.main.music.clip = musicData[current].clip;
        transition = true;
        AudioControl.main.music.FadeIn(2f, () => {
            transition = false;
        }, this);
    }

    private void Update() {
        if (transition) return;

        float y = GameControl.main.player.transform.position.y;
        if (current < musicData.Length - 1 && y - 50f >= musicData[current + 1].minHeight) {
            Play(current + 1);
        }
        else if(current > 0 && y + 50f < musicData[current].minHeight) {
            Play(current - 1);
        }
    }

    private void Play(int next) {
        nextid = next;
        transition = true;

        AudioControl.main.music.FadeOut(2f, () => {
            AudioControl.main.music.clip = musicData[nextid].clip;
            current = nextid;
            AudioControl.main.music.FadeIn(3f, () => {
                transition = false;
            }, this);
        }, this);
    }

    [System.Serializable]
    public class GameMusic {
        public float minHeight;
        public AudioClip clip;
    }
}
