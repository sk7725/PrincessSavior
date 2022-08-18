using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public enum Team {
        none,
        player,
        enemy
    }

    [Header("Movement")]
    public Team team = Team.player;
    public float speed = 5f;
    public float drag = 1f;
    public float lifetime = 5f;

    public bool penetrate = false, hitTerrain = true;

    [Header("Damage")]
    public float damage = 10f;
    public float knockback = 2f;

    [Header("Fx")]
    public GameObject hitFx;
    public GameObject despawnFx;

    protected float time, cspeed;
    protected bool despawned;

    protected virtual void Start() {
        time = 0f;
        cspeed = speed;
        despawned = false;
    }

    protected virtual void Update() {
        if (despawned) return;
        transform.position += transform.forward * Time.deltaTime * cspeed;
        time += Time.deltaTime;
        if(time > lifetime) {
            despawned = true;
            Despawn();
        }
    }

    private void FixedUpdate() {
        cspeed *= drag;
    }

    private void OnTriggerEnter(Collider other) {
        if (team != Team.enemy && other.CompareTag("Enemy")) {
            Enemy e;
            if (other.TryGetComponent(out e)) {
                Hit();
                HitEnemy(e);
                if (!penetrate) Destroy(gameObject);
            }
        }
        else if(team != Team.player && other.CompareTag("Player")) {
            PlayerControl p;
            if (other.TryGetComponent(out p)) {
                Hit();
                HitPlayer(p);
                if (!penetrate) Destroy(gameObject);
            }
        }
        else if (hitTerrain && other.CompareTag("Ground")) {
            despawned = true;
            Despawn();
        }
    }

    protected virtual void Hit() {
        GameControl.main.player.Fx(hitFx, transform.position, transform.rotation);
    }

    protected virtual void HitPlayer(PlayerControl player) {
        player.Damage(damage);
    }

    protected virtual void HitEnemy(Enemy enemy) {
        enemy.Damage(damage, knockback, gameObject);
    }

    protected virtual void Despawn() {
        GameControl.main.player.Fx(despawnFx, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void SetSpeed(float speed) {
        cspeed = speed;
    }
}
