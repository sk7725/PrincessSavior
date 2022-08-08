using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;

    [Header("Configuration")]
    public float maxHealth = 20f;
    public float contactDamage = 10f;
    public float knockback = 20f;

    [Header("Fx")]
    public GameObject deathFx;

    [System.NonSerialized] public float health;
    private bool dead;
    private float deathTimer;

    protected virtual void Start() {
        dead = false;
        health = maxHealth;
    }

    protected void Update() {
        if (dead) {
            deathTimer -= Time.deltaTime;
            if(deathTimer <= 0) {
                Fx(deathFx);
                Destroy(gameObject);
            }
        }
        else {
            UpdateAI();
        }
    }

    protected virtual void UpdateAI() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Sword")) {
            GameControl.main.player.sword.Hit(this);
            OnHit();
        }
        else if (!dead && other.CompareTag("Player")) {
            OnContact(GameControl.main.player);
        }
    }

    /// <summary>
    /// Called when hit by a sword.
    /// </summary>
    /// <remarks>
    /// Damage has already been processed beforehand. Is not called when the enemy is killed.
    /// </remarks>
    public virtual void OnHit() {
        //todo blink enemy
    }

    /// <summary>
    /// Called when the enemy comes into contact with the player.
    /// </summary>
    /// <remarks>
    /// Contact damage should be processed here.
    /// </remarks>
    public virtual void OnContact(PlayerControl player) {
        player.Damage(contactDamage, this);
    }

    public virtual void Damage(float damage, float knockback = 0f, GameObject threat = null) {
        health = Mathf.Min(health - damage, maxHealth);
        if (health <= 0f) Kill();
        if (threat != null) {
            //rigid.AddExplosionForce(knockback, threat.transform.position - Vector3.up, 20f, 0.4f, ForceMode.VelocityChange);
            bool right = transform.position.x > threat.transform.position.x;
            rigid.velocity = (Vector3.up + Vector3.right * (right ? 1f : -1f)) * knockback;
        }
    }

    public virtual void Kill() {
        if (dead) return;
        dead = true;
        deathTimer = 1f;
    }

    //todo pool fx
    public void Fx(GameObject fx) {
        Fx(fx, transform.position, transform.rotation);
    }

    public void Fx(GameObject fx, Vector3 position, Quaternion rotation) {
        if (fx == null) return;
        Instantiate(fx, position, rotation);
    }
}
