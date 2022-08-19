using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlade", menuName = "Sword/Blade", order = 140)]
public class Blade : SwordPart
{
    [Header("Blade")]
    public float damageMultiplier = 1f;
    public Gradient color;

    [Header("Fx/Bullets")]
    public GameObject strikeFx; //todo play strikefx in an arc or sth
    public GameObject throwFx;
    public GameObject poundFx;
    public GameObject thrownHitFx;
    public GameObject bounceFx;

    public virtual void OnThrow(Vector3 position, Vector3 throwVector) {
        if (throwFx != null) {
            GameControl.main.player.Fx(throwFx, position, Quaternion.LookRotation(throwVector, Vector3.up));
        }
    }

    public virtual void OnThrownHit(Vector3 position, Quaternion rotation) {
        GameControl.main.player.Fx(thrownHitFx, position, rotation);
    }

    public virtual void OnPound(Vector3 position) {
        GameControl.main.player.Fx(poundFx, position, Quaternion.identity);
    }

    public virtual void OnBounce(Vector3 position) {
        GameControl.main.player.Fx(bounceFx, position, Quaternion.identity);
    }

    public override void OnEquip() {
        //todo set material & mesh of both sword and heldsword's SwordModel class
        foreach(SwordModel model in GameControl.main.player.swordModels) {
            model.blade.filter.sharedMesh = mesh;
            model.blade.renderer.sharedMaterials = materials;
        }
        GameControl.main.player.SetSwordColor(color);
    }

    public override void OnUnequip() {

    }

    public override void EquipPlayer() {
        GameControl.main.player.blade.OnUnequip();
        GameControl.main.player.blade = this;
        GameControl.main.player.blade.OnEquip();
    }

    public override string Description() {
        return string.Format("{0}\n<color=blue>[°ø°Ý·Â {1}%]</color>", base.Description(), Mathf.RoundToInt(damageMultiplier * 100));
    }

    public override Color GetColor() {
        return color.Evaluate(0);
    }
}
