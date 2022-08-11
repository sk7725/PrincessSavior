using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private const float MOVE_THRESH2 = 0.01f;

    [Header("Settings")]
    public float throwDamage = 8f, strikeDamage = 15f, poundDamage = 25f;
    public float playerOffset;

    [HideInInspector] public bool landed;

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

    //called by enemy (via triggerenter)
    public void Hit(Enemy enemy) {
        //1. deal damage to the enemy (from sword stats)
        enemy.Damage(throwDamage * GameControl.main.player.blade.damageMultiplier, 3f, gameObject);
        //2. take knockback by normal (if sword is not immune to knockback)
        if (!GameControl.main.player.accessory.penetrate) {
            time = 0f;
            rigid.AddExplosionForce(enemy.knockback, enemy.transform.position, 10f, 0.2f, ForceMode.VelocityChange);
        }
        GameControl.main.player.blade.OnThrownHit(transform.position, transform.rotation);
    }

    public float GetPoundDamage() {
        return poundDamage * (Physics.gravity.y / GameControl.main.player.defaultGravity);
    }
}
