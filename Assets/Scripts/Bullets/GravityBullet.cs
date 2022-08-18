using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBullet : Bullet
{
    [Header("Gravity Bullet")]
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private bool despawnOnStop = true;

    protected override void Start() {
        base.Start();
        rigid.velocity = transform.forward * speed;
    }

    protected override void Update() {
        if (despawned) return;
        time += Time.deltaTime;
        if(rigid.velocity.sqrMagnitude < 0.01f && despawnOnStop) {
            time += Time.deltaTime * 3f;
        }
        if (time > lifetime) {
            despawned = true;
            Despawn();
        }
    }

#if UNITY_EDITOR
    private void OnValidate() {
        rigid = GetComponent<Rigidbody>();
    }
#endif
}
