using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwordPartItem : Item {
    [Header("Sword Part")]
    [SerializeField] private SwordModel model;
    [SerializeField] private Blade blade;
    [SerializeField] private Handle handle;
    [SerializeField] private Accessory accessory;

    [SerializeField] private ParticleSystem particle;
    [SerializeField] private TextMeshPro text;

    protected override void Start() {
        base.Start();
        if (blade != null) {
            model.blade.filter.sharedMesh = blade.mesh;
            model.blade.renderer.sharedMaterials = blade.materials;
            text.text = blade.localized;
        }
        else if (handle != null) {
            model.handle.filter.sharedMesh = handle.mesh;
            model.handle.renderer.sharedMaterials = handle.materials;
            text.text = handle.localized;
        }
        else if (accessory != null) {
            model.accessory.filter.sharedMesh = accessory.mesh;
            model.accessory.renderer.sharedMaterials = accessory.materials;
            text.text = accessory.localized;
        }
        text.color = GetColor();
        var pm = particle.main;
        pm.startColor = GetColor();
    }

    protected override void Respawn() {
        base.Respawn();
        particle.Play();
    }

    public override bool CanCollect() {
        return base.CanCollect() && !GameControl.main.player.swordPopupActive && CompareParts();
    }

    public override bool DoMagnet() {
        return base.DoMagnet() && CompareParts();
    }

    private bool CompareParts() {
        PlayerControl player = GameControl.main.player;
        if (blade != null) return blade != player.blade;
        if (handle != null) return handle != player.handle;
        return accessory != player.accessory;
    }

    public override void OnCollect() {
        base.OnCollect();
        UI.PartPopup(blade != null ? blade : (handle != null ? handle : accessory));
        GameControl.main.player.swordPopupActive = true;
        particle.Stop();
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
