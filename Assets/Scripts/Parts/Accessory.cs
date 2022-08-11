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

    public override void Equip() {
        //todo set material & mesh of both sword and heldsword's SwflordModel class
        foreach (SwordModel model in GameControl.main.player.swordModels) {
            model.accessory.filter.mesh = mesh;
            model.accessory.renderer.sharedMaterials = materials;
        }
    }

    public override void Unequip() {

    }
}
