using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDialog : Dialog
{
    public enum Page {
        howto,
        settings,
        backtitle
    }

    [SerializeField] private PageObject[] pages;
    [SerializeField] private Button[] vetoButtons;
    [SerializeField] private Button bgListener, closeButton;
    [SerializeField] private Color disabledColor = Color.black;

    public Page page = Page.howto;
    public Page prevPage = Page.howto;

    private void Awake() {
        foreach (PageObject p in pages) {
            p.button.onClick.AddListener(() => {
                if (p.page == page) return;
                prevPage = page;
                page = p.page;
                Build();
            });
        }

        foreach (Button b in vetoButtons) {
            b.onClick.AddListener(() => {
                Page p = prevPage;
                prevPage = page;
                page = p;
                Build();
            });
        }
        bgListener.onClick.AddListener(Close);
        closeButton.onClick.AddListener(Close);
    }

    public override void Build() {
        base.Build();
        foreach (PageObject p in pages) {
            if (p.page == page) {
                p.pageObject.SetActive(true);
                p.buttonImage.color = Color.white;
                p.buttonRect.SetLeft(-20);
                p.buttonRect.SetRight(20);
            }
            else {
                p.pageObject.SetActive(false);
                p.buttonImage.color = disabledColor;
                p.buttonRect.SetLeft(0);
                p.buttonRect.SetRight(0);
            }
        }
    }

    [System.Serializable]
    public class PageObject {
        public Page page;
        public GameObject pageObject;
        public Button button;
        public Image buttonImage;
        public RectTransform buttonRect;
    }
}
