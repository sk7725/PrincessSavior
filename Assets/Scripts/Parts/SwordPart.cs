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

    public abstract void OnEquip();

    public abstract void OnUnequip();

    public abstract void EquipPlayer();

    public abstract Color GetColor();

    public virtual string Description() {
        return description;
    }
}
