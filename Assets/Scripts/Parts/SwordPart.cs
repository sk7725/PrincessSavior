using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwordPart : ScriptableObject
{
    [Header("General")]
    public string localized;
    public int cost = 10;

    public Mesh mesh;
    public Material[] materials;

    public abstract void Equip();

    public abstract void Unequip();
}
