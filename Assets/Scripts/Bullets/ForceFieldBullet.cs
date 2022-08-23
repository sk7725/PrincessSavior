using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldBullet : Bullet
{
    [Header("Force Field")]
    public MeshRenderer mrender;
    public float endScale = 5f;
    public float rotateSpeed = 180f;
    public Gradient color;

    Material mat;

    private void Awake() {
        mat = mrender.material;
        mat.color = color.Evaluate(0);
    }

    protected override void Update() {
        base.Update();
        if (despawned) return;
        mat.color = color.Evaluate(time / lifetime);
        transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.z, endScale, time / lifetime);

        transform.rotation *= Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0);
    }
}
