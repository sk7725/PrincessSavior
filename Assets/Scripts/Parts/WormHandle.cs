using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WormHandle", menuName = "Sword/HandleWorm", order = 147)]
public class WormHandle : Handle
{
    public float distance = 3f;

    public GameObject poofFx, poofOutFx;

    public override void OnThrow(Vector3 position, Vector3 throwVector) {
        base.OnThrow(position, throwVector);

        GameControl.main.player.Fx(poofFx, position, Quaternion.identity);
        GameControl.main.player.sword.transform.position += throwVector.normalized * distance;
        GameControl.main.player.Fx(poofOutFx, GameControl.main.player.sword.transform.position, Quaternion.identity);
    }
}
