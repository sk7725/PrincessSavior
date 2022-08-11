using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VelocityHandle", menuName = "Sword/HandleVelocity", order = 148)]
public class VelocityHandle : Handle {
    public float duration = 2f;
    public Vector2 velocity;
    private uint throwid = 0;

    public override void OnThrow(Vector3 position, Vector3 throwVector) {
        base.OnThrow(position, throwVector);

        throwid++;
        GameControl.main.player.StartCoroutine(IVelocity());
    }

    private IEnumerator IVelocity() {
        uint cid = throwid;
        float time = 0;
        while (time < duration && cid == throwid && GameControl.main.player.sword.gameObject.activeInHierarchy) {
            yield return null;
            time += Time.deltaTime;
            GameControl.main.player.sword.rigid.AddForce(velocity, ForceMode.VelocityChange);
        }
    }

    public override void Unequip() {
        base.Unequip();
        throwid = 0;
    }
}
