using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//does something special when everything is collected
public class GemItem : Item {
    protected override void Start() {
        base.Start();
        GameControl.main.player.maxGems++;
    }

    public override void OnCollect() {
        base.OnCollect();
        GameControl.main.player.gems++;
    }
}
