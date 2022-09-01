using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkButton : MonoBehaviour {
    [SerializeField] private string link = "";

    private void Start() {
        if (link == "") {
            Button b = GetComponent<Button>();
            b.transition = Selectable.Transition.None;
            return;
        }
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked() {
        Application.OpenURL(link);
    }
}
