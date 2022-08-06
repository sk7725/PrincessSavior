using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private const float MOVE_THRESH2 = 0.05f;

    [Header("Settings")]
    public float playerOffset;

    public bool landed;

    [HideInInspector] public Rigidbody rigid;
    private float time = 0f;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update() {
        time += Time.deltaTime;
        CheckLanded();
    }

    public void Init() {
        time = 0f;
        landed = false;
    }

    public Vector3 GetPlayerPos(bool preserveZ = false) {
        if (preserveZ) {
            return transform.position + transform.right * playerOffset;
        }
        Vector2 v = transform.right;
        v.Normalize();
        return transform.position + transform.right * playerOffset;
    }

    public void CheckLanded() {
        //this is quite different from the susal landed checker, just check if it is moving
        landed = rigid.velocity.sqrMagnitude < MOVE_THRESH2 && time > 0.5f;
    }
}
