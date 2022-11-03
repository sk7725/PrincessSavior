using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class TitleScroller : MonoBehaviour
{
    public RectTransform title, subtitle, bg;
    public Button forwardButton, backButton;

    float scrollSpeed = 8f;

    [System.NonSerialized] public bool scrolled = false;
    [Range(0, 1)] public float step = 0f;

    private void Start()
    {
        if (!Application.isPlaying) return;
        float h = title.rect.height;
        step = scrolled ? 1 : 0;

        title.SetTop(0);
        title.SetBottom(0);

        //update subtitle top&bottom
        subtitle.SetTop(h);
        subtitle.SetBottom(-h);

        //update bg
        bg.SetBottom(-h / 2);

        forwardButton.onClick.AddListener(ForwardClicked);
        backButton.onClick.AddListener(BackClicked);
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            step = Mathf.Lerp(step, scrolled ? 1 : 0, Time.deltaTime * scrollSpeed);
        }

        float h = title.rect.height;

        title.SetTop(-h * step);
        title.SetBottom(h * step);

        //update subtitle top&bottom
        subtitle.SetTop(h);
        subtitle.SetBottom(-h);

        //update bg
        bg.SetBottom(-h/2 * (1 - step));
    }

    private void ForwardClicked()
    {
        scrolled = true;
    }

    private void BackClicked()
    {
        scrolled = false;
    }
}
