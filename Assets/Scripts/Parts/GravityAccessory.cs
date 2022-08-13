using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GravityAccessory", menuName = "Sword/AccessoryGravity", order = 156)]
public class GravityAccessory : Accessory
{
    public float gravity = -18f;

    public override void OnEquip() {
        base.OnEquip();
        Physics.gravity = Vector3.up * gravity;
    }

    public override void OnUnequip() {
        base.OnUnequip();
        Physics.gravity = Vector3.up * GameControl.main.player.defaultGravity;
    }
}
