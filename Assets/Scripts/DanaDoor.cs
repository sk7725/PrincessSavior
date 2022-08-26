using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DanaDoor : MonoBehaviour {
    public string endScene = "EndScene";

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Sword")) {
            Rigidbody sword = GameControl.main.player.sword.rigid;
            Vector3 v = sword.velocity;
            v.x *= Mathf.Pow(0.92f, Time.deltaTime * 60);
            sword.velocity = v;
        }
        else if (other.CompareTag("Player") && GameControl.main.player.state == PlayerControl.State.idle && GameControl.main.player.landed) {
            GameControl.Pause();
            AudioControl.main.music.FadeOut(1f, this);
            Settings.TimeRecord = (float)NumberFrame.time;
            if(Settings.TimeRecord > 15f && (Settings.BestTimeRecord <= 15f || Settings.TimeRecord < Settings.BestTimeRecord)) Settings.BestTimeRecord = Settings.TimeRecord;

            UI.CircleFade(false, 2f, () => {
                Time.timeScale = 1f;
                SceneManager.LoadSceneAsync(endScene);
            });
        }
    }
}
