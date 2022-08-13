using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
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
}
