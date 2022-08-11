using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwordPart : ScriptableObject
{
    [Header("General")]
    public string localized;
    public string description;
    public int cost = 10;

    public Mesh mesh;
    public Material[] materials;

    public abstract void Equip();

    public abstract void Unequip();

    public virtual string Description() {
        return description;
    }
}
