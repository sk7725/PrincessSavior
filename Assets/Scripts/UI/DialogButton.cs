using UnityEngine;
using UnityEngine.UI;

public class DialogButton : MonoBehaviour {
    [SerializeField] private Dialog dialog;

    void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
    }


    private void Clicked() {
        dialog.gameObject.SetActive(true);
        dialog.Build();
    }
}
