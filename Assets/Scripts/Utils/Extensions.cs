using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
    public static bool Similar(this Quaternion quatA, Quaternion value, float acceptableRange) {
        return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
    }

    public static Color Hue(this Color color, float amount) {
        float s, v;
        Color.RGBToHSV(color, out _, out s, out v);
        color = Color.HSVToRGB(amount, s, v);
        return color;
    }

    public static Color Saturation(this Color color, float amount) {
        float h, v;
        Color.RGBToHSV(color, out h, out _, out v);
        color = Color.HSVToRGB(h, amount, v);
        return color;
    }

    public static Color Value(this Color color, float amount) {
        float h, s;
        Color.RGBToHSV(color, out h, out s, out _);
        color = Color.HSVToRGB(h, s, amount);
        return color;
    }

    public static MeshEffectGroup.MeshEffect MeshEffect(this Collision collision) {
        MeshEffectGroup effectGroup;
        if ((collision.collider is MeshCollider mc) && collision.gameObject.TryGetComponent(out effectGroup)) {
            foreach(MeshEffectGroup.MeshEffect effect in effectGroup.meshes) {
                if(effect.collider == mc) {
                    return effect;
                }
            }
        }
        return null;
    }

    public static MeshEffectGroup.MeshEffect MeshEffect(this MeshCollider mc) {
        MeshEffectGroup effectGroup;
        if (mc.gameObject.TryGetComponent(out effectGroup)) {
            foreach (MeshEffectGroup.MeshEffect effect in effectGroup.meshes) {
                if (effect.collider == mc) {
                    return effect;
                }
            }
        }
        return null;
    }

    public static string ToTimeString(this double time) {
        if (time < 0) return "--:--.---";
        int hours = (int)(time / 60 / 60);
        return hours > 0 ? string.Format("{0}:{1,2:D2}:{2,2:D2}.{3,3:D3}", hours, (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000))
            : string.Format("{0}:{1,2:D2}.{2,3:D3}", (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000));
    }

    public static string ToTimeString(this float time) {
        if (time < 0) return "--:--.---";
        int hours = (int)(time / 60 / 60);
        return hours > 0 ? string.Format("{0}:{1,2:D2}:{2,2:D2}.{3,3:D3}", hours, (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000))
            : string.Format("{0}:{1,2:D2}.{2,3:D3}", (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000));
    }

    public static void FadeOut(this AudioSource source, float duration, MonoBehaviour caller) {
        caller.StartCoroutine(IFadeOut(source, duration, null));
    }

    public static void FadeOut(this AudioSource source, float duration, Action endAction, MonoBehaviour caller) {
        caller.StartCoroutine(IFadeOut(source, duration, endAction));
    }

    public static void FadeIn(this AudioSource source, float duration, MonoBehaviour caller) {
        caller.StartCoroutine(IFadeIn(source, duration, null));
    }

    public static void FadeIn(this AudioSource source, float duration, Action endAction, MonoBehaviour caller) {
        caller.StartCoroutine(IFadeIn(source, duration, endAction));
    }

    static IEnumerator IFadeOut(AudioSource source, float duration, Action endAction) {
        float time = 0f;
        while (time < duration) {
            time += Time.unscaledDeltaTime;
            source.volume = 1 - time / duration;
            yield return null;
        }
        source.volume = 0f;
        source.Stop();
        if(endAction != null) endAction();
    }

    static IEnumerator IFadeIn(AudioSource source, float duration, Action endAction) {
        source.Play();
        float time = 0f;
        while (time < duration) {
            time += Time.unscaledDeltaTime;
            source.volume = time / duration;
            yield return null;
        }
        source.volume = 1f;
        if (endAction != null) endAction();
    }

    public static void SetLeft(this RectTransform rt, float left) {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right) {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top) {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom) {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
