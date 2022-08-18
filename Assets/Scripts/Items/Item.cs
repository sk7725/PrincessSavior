using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Preset Fields")]
    [SerializeField] private MagnetRange magnetRange;

    [Header("Item")]
    [SerializeField] private int coins = 0;
    [SerializeField] private float healAmount = 0f;
    [SerializeField] private float magnetStrength = 20f;
    [SerializeField] private float rotateSpeed = 150f;
    [SerializeField] private GameObject hitFx = null;

    private bool magnetized;
    private float magSpeed;

    private void Awake() {
        magnetized = false;
        magSpeed = 0;
        if (magnetRange != null) magnetRange.item = this;
    }

    protected virtual void Start()
    {
        magnetized = false;
        transform.rotation = Quaternion.Euler(0, rotateSpeed * (transform.position.x + transform.position.y), 0);
    }

    protected virtual void LateUpdate()
    {
        if (rotateSpeed != 0f) {
            Vector3 rot = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(rot.x, rot.y + rotateSpeed * Time.deltaTime, rot.z);
        }

        if (magnetized && DoMagnet()) {
            if (GameControl.main.player.dead) {
                //stop magging
                magSpeed = 0;
                magnetized = false;
                magnetRange.Unmagnetize();
            }
            magSpeed += Time.deltaTime * 8f;
            if (magSpeed > magnetStrength) magSpeed = magnetStrength;
            transform.position = Vector3.MoveTowards(transform.position, GameControl.main.player.PlayerPos(), Time.deltaTime * magSpeed);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!GameControl.main.player.dead && CanCollect() && (other.CompareTag("Player") || other.CompareTag("Sword"))) {
            OnCollect();
            Destroy(gameObject);
        }
    }

    public virtual void OnCollect() {
        GameControl.main.player.Fx(hitFx, transform.position, Quaternion.identity);
        if (healAmount != 0f) {
            GameControl.main.player.Damage(-healAmount);
        }
        GameControl.main.player.coins += coins;
    }

    public virtual bool CanCollect() {
        return true;
    }

    public virtual bool DoMagnet() {
        return healAmount <= 0.1f || GameControl.main.player.health < GameControl.main.player.maxHealth;
    }

    public virtual void EnableMagnet() {
        magnetized = true;
    }
}
