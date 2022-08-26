using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {
    void Start() {
#if UNITY_IOS
        gameObject.SetActive(false);
#endif
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked() {
        Application.Quit();
    }
}
