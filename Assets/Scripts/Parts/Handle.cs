using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHandle", menuName = "Sword/Handle", order = 145)]
public class Handle : SwordPart {
    [Header("Handle")]
    public float throwMultiplier = 1f;

    public virtual void OnThrow(Vector3 position, Vector3 throwVector) {
        
    }

    public override void OnEquip() {
        //todo set material & mesh of both sword and heldsword's SwflordModel class
        foreach (SwordModel model in GameControl.main.player.swordModels) {
            model.handle.filter.sharedMesh = mesh;
            model.handle.renderer.sharedMaterials = materials;
        }
    }

    public override void OnUnequip() {

    }

    public override void EquipPlayer() {
        GameControl.main.player.handle.OnUnequip();
        GameControl.main.player.handle = this;
        GameControl.main.player.handle.OnEquip();
    }

    public override string Description() {
        return string.Format("{0}\n<color=blue>[´øÁö´Â Èû {1}%]</color>", base.Description(), Mathf.RoundToInt(throwMultiplier * 100));
    }

    public override Color GetColor() {
        return materials[0].color.Value(1);
    }
}
