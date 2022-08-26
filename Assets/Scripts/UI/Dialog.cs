using UnityEngine;

public abstract class Dialog : MonoBehaviour {
    public virtual void Build() {
        if(GameControl.main != null) GameControl.Pause();
    }

    public virtual void Close() {
        gameObject.SetActive(false);
        if (GameControl.main != null && !GameControl.main.DialogOpen()) GameControl.Unpause();
    }
}
