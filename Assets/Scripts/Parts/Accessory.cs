using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGem", menuName = "Sword/Accessory", order = 155)]
public class Accessory : SwordPart {
    [Header("Accessory")]
    public Color color;

    public bool penetrate = false;
    public bool disablePound = false;
    public int scoreMultiplier = 1;
    public float takeDamageMultiplier = 1f;

    public override void OnEquip() {
        //todo set material & mesh of both sword and heldsword's SwflordModel class
        foreach (SwordModel model in GameControl.main.player.swordModels) {
            model.accessory.filter.sharedMesh = mesh;
            model.accessory.renderer.sharedMaterials = materials;
        }
    }

    public override void OnUnequip() {

    }

    public override void EquipPlayer() {
        GameControl.main.player.accessory.OnUnequip();
        GameControl.main.player.accessory = this;
        GameControl.main.player.accessory.OnEquip();
    }

    public override Color GetColor() {
        return color;
    }
}
