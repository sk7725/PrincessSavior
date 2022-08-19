using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPartItem : Item
{
    [Header("Sword Part")]
    [SerializeField] private SwordModel model;
    [SerializeField] private Blade blade;
    [SerializeField] private Handle handle;
    [SerializeField] private Accessory accessory;

    [SerializeField] private ParticleSystem particle;

    protected override void Start() {
        base.Start();
        if (blade != null) {
            model.blade.filter.sharedMesh = blade.mesh;
            model.blade.renderer.sharedMaterials = blade.materials;
        }
        else if (handle != null) {
            model.handle.filter.sharedMesh = handle.mesh;
            model.handle.renderer.sharedMaterials = handle.materials;
        }
        else if (accessory != null) {
            model.accessory.filter.sharedMesh = accessory.mesh;
            model.accessory.renderer.sharedMaterials = accessory.materials;
        }
        var pm = particle.main;
        pm.startColor = GetColor();
    }

    public override bool CanCollect() {
        return base.CanCollect() && !GameControl.main.player.swordPopupActive;
    }

    public override void OnCollect() {
        base.OnCollect();
        UI.PartPopup(blade != null ? blade : (handle != null ? handle : accessory));
        GameControl.main.player.swordPopupActive = true;
    }

    private Color GetColor() {
        if (blade != null) {
            return blade.GetColor();
        }
        else if (handle != null) {
            return handle.GetColor();
        }
        else {
            return accessory.GetColor();
        }
    }
}
