using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseDialog : Dialog {
    [SerializeField] private Button bgListener, closeButton;

    private void Awake() {
        bgListener.onClick.AddListener(Close);
        closeButton.onClick.AddListener(Close);
    }
}
