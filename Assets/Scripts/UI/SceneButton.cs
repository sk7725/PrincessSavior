using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour {
    [SerializeField] private string scene;

    void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked() {
        SceneManager.LoadScene(scene);
    }
}
