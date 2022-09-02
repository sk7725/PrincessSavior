using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    private const float RESPAWN_DIST2 = 100f;

    [Header("Preset Fields")]
    [SerializeField] private MagnetRange magnetRange;

    [Header("Item")]
    [SerializeField] private int coins = 0;
    [SerializeField] private float healAmount = 0f;
    [SerializeField] private float magnetStrength = 20f;
    [SerializeField] private float rotateSpeed = 150f;
    [SerializeField] private GameObject hitFx = null;

    [SerializeField] private bool respawns = false;
    [SerializeField] private bool autoRespawn = false;
    [SerializeField] private GameObject respawnFx = null;

    private bool magnetized;
    private float magSpeed;

    private bool collected = false;
    private Vector3 respawnPos;

    private void Awake() {
        magnetized = false;
        magSpeed = 0;
        if (magnetRange != null) magnetRange.item = this;
    }

    protected virtual void Respawn() {
        magnetized = false;
        magSpeed = 0;
        collected = false;
        transform.position = respawnPos;
        foreach (Transform c in transform) {
            c.gameObject.SetActive(true);
        }
        if (magnetRange != null) magnetRange.Unmagnetize();

        GameControl.main.player.Fx(respawnFx, transform.position, Quaternion.identity);
    }

    protected virtual void Start() {
        magnetized = false;
        collected = false;
        transform.rotation = Quaternion.Euler(0, rotateSpeed * (transform.position.x + transform.position.y), 0);
        respawnPos = transform.position;
    }

    protected virtual void LateUpdate() {
        if (collected) {
            if (autoRespawn && (GameControl.main.player.PlayerPos() - transform.position).sqrMagnitude > RESPAWN_DIST2 && (GameControl.main.player.transform.position - transform.position).sqrMagnitude > RESPAWN_DIST2 && !GameControl.main.player.swordPopupActive) {
                Respawn();
            }
            return;
        }

        if (rotateSpeed != 0f) {
            Vector3 rot = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rot.x, rot.y + rotateSpeed * Time.deltaTime, rot.z);
        }

        if (magnetized && DoMagnet()) {
            if (GameControl.main.player.dead) {
                //stop magging
                magSpeed = 0;
                magnetized = false;
                if (magnetRange != null) magnetRange.Unmagnetize();
            }
            //magSpeed += Time.deltaTime * 8f;
            //if (magSpeed > magnetStrength) magSpeed = magnetStrength;
            magSpeed = magnetStrength;
            transform.position = Vector3.MoveTowards(transform.position, GameControl.main.player.PlayerPos(), Time.deltaTime * magSpeed);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!collected && !GameControl.main.player.dead && CanCollect() && (other.CompareTag("Player") || other.CompareTag("Sword"))) {
            OnCollect();
            collected = true;
            if (respawns) {
                foreach (Transform c in transform) {
                    c.gameObject.SetActive(false);
                }
            }
            else Destroy(gameObject);
        }
    }

    public virtual void OnCollect() {
        GameControl.main.player.Fx(hitFx, transform.position, Quaternion.identity);
        if (healAmount != 0f) {
            GameControl.main.player.Damage(-healAmount);
        }
        GameControl.main.player.coins += coins * GameControl.main.player.accessory.scoreMultiplier;
    }

    public virtual bool CanCollect() {
        return true;
    }

    public virtual bool DoMagnet() {
        return !collected && (healAmount <= 0.1f || GameControl.main.player.health < GameControl.main.player.maxHealth);
    }

    public virtual void EnableMagnet() {
        magnetized = true;
    }
}
