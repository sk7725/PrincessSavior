using UnityEngine;

public abstract class Dialog : MonoBehaviour {
    public virtual void Build() {
        GameControl.Pause();
    }

    public virtual void Close() {
        gameObject.SetActive(false);
        if (!GameControl.main.DialogOpen()) GameControl.Unpause();
    }
}
