using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZeroGHandle", menuName = "Sword/HandleZeroG", order = 146)]
public class ZeroGravityHandle : Handle
{
    public float duration = 2f;
    private uint throwid = 0;

    public override void OnThrow(Vector3 position, Vector3 throwVector) {
        base.OnThrow(position, throwVector);

        throwid++;
        GameControl.main.player.StartCoroutine(IZeroG());
    }

    private IEnumerator IZeroG() {
        uint cid = throwid;
        GameControl.main.player.sword.rigid.useGravity = false;
        yield return new WaitForSeconds(duration);
        if(cid == throwid) GameControl.main.player.sword.rigid.useGravity = true;
    }

    public override void OnUnequip() {
        base.OnUnequip();
        throwid = 0;
        GameControl.main.player.sword.rigid.useGravity = true;
    }
}
