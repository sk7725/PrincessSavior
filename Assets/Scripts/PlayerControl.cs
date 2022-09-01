using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using static PlayerRenderer;

public class PlayerControl : MonoBehaviour
{
    public const float MIN_DRAG_LEN2 = 0.2f, HEIGHT = 2f;
    private const float INVIN_TIME = 0.5f, DEATH_BARRIER = -40f;

    [Header("Settings")]
    public float poundStr = 5f;
    public float throwStr = 5f, bounceStr = 15f;
    public float maxDragLength = 3f;
    [SerializeField] private Vector3 throwTorque;
    [SerializeField] private Vector2 throwOffset;
    [SerializeField] private Quaternion throwRotation = Quaternion.identity;
    public float maxHealth = 100f;
    [HideInInspector] public int maxGems = 0; //set by gem items
    public float defaultGravity = -18f;

    [Header("Sword Parts")]
    public Blade blade;
    public Handle handle;
    public Accessory accessory;

    //preset fields
    [Header("Preset Fields")]
    public Sword sword;
    public GameObject heldSword;
    public AnimationTime animTime;
    public PlayerRenderer animator;
    [SerializeField] private SwordTrail swordTrail;
    [SerializeField] private FlashProjector flash;
    public SwordModel[] swordModels;

    [Header("Fx")]
    public GameObject catchFx;
    public GameObject catchPrevFx, swordUpgradeFx;

    [Header("Sfx")]
    public AudioClip landSound;
    public AudioClip throwSound, catchSound, swordUpgradeSound, damageSound;

    [Header("Debugging")]
    public State state;
    public State nextState;
    public float stateTime = 0f; //time after a state change

    public bool landed, pounding, landBounce;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] private Collider col;
    [HideInInspector] public AudioSource audios;
    private Collider[] colresult = new Collider[10];
    private float swordOffset, landBounceTimer;
    private Quaternion? targetRotation;

    public float health;
    public int coins = 0;
    public int gems = 0;
    [System.NonSerialized] public bool swordPopupActive = false, dead = false;
    private float invincibility;

    private Collider[] groundCol = new Collider[1];
    private MeshEffectGroup.MeshEffect lastMeshEffect;

    //inputs
    public bool dragged;
    private Vector2 dragStartPos, dragCurrentPos;

    public enum State {
        none,
        idle,
        preThrow,
        thrown,
        pound,
        pick
    }

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        audios = GetComponent<AudioSource>();
        health = maxHealth;
        Physics.gravity = Vector2.up * defaultGravity;
        maxGems = 0;
    }

    void Start()
    {
        state = State.none;
        nextState = State.idle;
        swordOffset = sword.playerOffset;

        //equip parts
        blade.OnEquip();
        handle.OnEquip();
        accessory.OnEquip();

        sword.gameObject.SetActive(false);
        landed = pounding = false;
        swordPopupActive = false;
        coins = 0;
        gems = 0;

        animator.Trig(Trigger.upgrade);
    }

    void Update()
    {
        stateTime += Time.deltaTime;
        CheckLanded();
        if (dead) return;
        CheckDeath();
        UpdateInput();
        bool updateTransform = true; //update animator transform
        if(invincibility > 0) invincibility -= Time.deltaTime;

        //AUTO STATE CHANGE
        if (nextState == State.none) {
            switch (state) {
                case State.preThrow:
                    if (stateTime >= animTime.throwing) {
                        nextState = State.thrown;
                    }
                    break;
                case State.thrown:
                    if (landBounce) {
                        landBounceTimer += Time.deltaTime;
                        if(landBounceTimer >= animTime.landBounce) {
                            nextState = State.pound;
                        }
                    }
                    else {
                        //check if sword is airborne
                        //if not, yeet sword up and then switch to pick
                        if (sword.landed) {
                            sword.rigid.velocity = Vector3.up * bounceStr + (dragStartPos.x > dragCurrentPos.x ? Vector3.right : Vector3.left) * bounceStr * 0.02f;
                            landBounce = true;
                            landBounceTimer = 0;
                            blade.OnBounce(sword.transform.position);
                        }
                    }
                    break;
                case State.pound:
                    if (landed) {
                        nextState = State.pick;
                    }
                    break;
                case State.pick:
                    //if animation finishes,go back to idle
                    if(stateTime >= animTime.pick) {
                        nextState = State.idle;
                    }
                    break;
            }
        }

        //STATE INIT (per state)
        if (nextState != State.none) {
            state = nextState;
            nextState = State.none;
            switch (state) {
                case State.preThrow:
                    animator.Trig(Trigger.thrown);
                    audios.PlayOneShot(throwSound, 0.4f);
                    break;
                case State.thrown:
                    AreaDamage(heldSword.transform.position, sword.strikeDamage, 1f);
                    ThrowSword();
                    break;
                case State.pound:
                    pounding = false;
                    targetRotation = sword.rigid.velocity.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
                    CatchSword();
                    break;
                case State.pick:
                    if (pounding) {
                        Vector3 hs = heldSword.transform.position;
                        hs.y = transform.position.y - HEIGHT / 2f;
                        blade.OnPound(hs);
                        AreaDamage(hs, sword.GetPoundDamage(), 1.5f, 8f);
                        if (lastMeshEffect != null) {
                            Fx(lastMeshEffect.hitFx, hs, Quaternion.identity);
                            if (lastMeshEffect.hitSound != null) {
                                audios.PlayOneShot(lastMeshEffect.hitSound);
                            }
                        }
                    }
                    break;
            }
            stateTime = 0f;
        }

        //UPDATE (per state)
        switch (state) {
            case State.pound:
                if(stateTime > animTime.pound) {
                    if (!pounding) {
                        rigid.velocity = Vector3.zero;
                        pounding = true;
                    }
                }
                else {
                    //lerp animator z
                    updateTransform = false;
                }
                break;
        }

        UpdateAnimator(updateTransform);
    }

    private void FixedUpdate() {
        if(!dead && state == State.pound && pounding) rigid.AddForce(Vector3.down * poundStr * (Physics.gravity.y / defaultGravity) * 60, ForceMode.Acceleration);
    }

    private void UpdateInput() {
        bool pressed = Input.GetMouseButton(0) && !GameControl.main.DialogOpen() && !EventSystem.current.IsPointerOverGameObject(Input.touchCount > 0 ? Input.GetTouch(0).fingerId : -1) && !ZoomButton.pressed;

        switch (state) {
            case State.idle:
                if (dragged) {
                    if (pressed) {
                        //update drag arrow graphics
                        dragCurrentPos = FingerPos();
                    }
                    else {
                        //end drag
                        dragged = false;
                        //check if drag vector is long enoough
                        Vector2 dragv = dragCurrentPos - dragStartPos;
                        if (dragv.sqrMagnitude > MIN_DRAG_LEN2) {
                            nextState = State.preThrow;
                        }
                    }
                }
                else {
                    if (pressed) {
                        //start drag
                        dragged = true;
                        dragStartPos = dragCurrentPos = FingerPos();
                    }
                }
                break;
            case State.thrown:
                if (pressed && !accessory.disablePound) {
                    //ground pound!
                    nextState = State.pound;
                }
                break;
        }
    }

    private void UpdateAnimator(bool updateTransform) {
        if(updateTransform) animator.transform.position = transform.position;

        //UPDATE (per state)
        switch (state) {
            case State.idle:
                if (dragged && Mathf.Abs(dragStartPos.x - dragCurrentPos.x) > 0.1f) {
                    targetRotation = (dragCurrentPos.x < dragStartPos.x) ? Quaternion.identity : Quaternion.Euler(0, 179.9f, 0);
                }
                if(targetRotation != null) {
                    animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, targetRotation.Value, 8f * Time.deltaTime);
                    if(animator.transform.rotation.Similar(targetRotation.Value, 0.001f)) {
                        animator.transform.rotation = targetRotation.Value;
                        targetRotation = null;
                    }
                }
                break;
            case State.pound:
                if (targetRotation != null) {
                    animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, targetRotation.Value, 5f * Time.deltaTime);
                    if (animator.transform.rotation.Similar(targetRotation.Value, 0.001f)) {
                        animator.transform.rotation = targetRotation.Value;
                        targetRotation = null;
                    }
                }

                if (!updateTransform) {
                    float z = Mathf.Lerp(animator.transform.position.z, 0, 5f * Time.deltaTime);
                    animator.transform.position = new Vector3(transform.position.x, transform.position.y, z);
                }
                break;
            default: 
                if (targetRotation != null) {
                    animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, targetRotation.Value, 8f * Time.deltaTime);
                    if (animator.transform.rotation.Similar(targetRotation.Value, 0.001f)) {
                        animator.transform.rotation = targetRotation.Value;
                        targetRotation = null;
                    }
                }
                break;
        }
    }

    private void CheckLanded() {
        bool prevl = landed;
        landed = Physics.OverlapSphereNonAlloc(new Vector3(col.bounds.center.x, col.bounds.center.y - ((HEIGHT - 1f) / 2 + 0.15f), col.bounds.center.z), 0.45f, groundCol, 1 << 6, QueryTriggerInteraction.Ignore) > 0;
        if (!prevl && landed && groundCol[0] is MeshCollider m) lastMeshEffect = m.MeshEffect();
        if(!prevl && landed) {
            audios.PlayOneShot(landSound, Mathf.Clamp01(-rigid.velocity.y / 10f));
        }
    }

    private void CheckDeath() {
        Vector3 pos = PlayerPos();
        if (pos.y < DEATH_BARRIER) Kill();
    }

    /// <returns>Player or the sword's position, depending on the current player state.</returns>
    public Vector3 PlayerPos() {
        return sword.gameObject.activeInHierarchy ? sword.transform.position : transform.position;
    }

    private void ThrowSword() {
        landBounce = false;
        sword.gameObject.SetActive(true);
        //sword.transform.position = transform.position + (Vector3)throwOffset;
        sword.transform.position = (Vector2)heldSword.transform.position;
        sword.rigid.MoveRotation(heldSword.transform.rotation);
        Vector3 v = ThrowVector() * handle.throwMultiplier * Mathf.Sqrt(Physics.gravity.y / defaultGravity);
        sword.rigid.velocity = v;
        sword.rigid.angularVelocity = throwTorque;
        sword.Init();
        blade.OnThrow(transform.position + (Vector3)throwOffset, v);
        handle.OnThrow(transform.position + (Vector3)throwOffset, v);

        heldSword.SetActive(false);
        GameControl.main.camc.UpdateTarget();
    }

    private void CatchSword() {
        //set player pos to sword pos + sword offset
        Fx(catchPrevFx);
        transform.position = sword.GetPlayerPos();
        rigid.MovePosition(sword.GetPlayerPos());
        //set playerrenderer pos to true sword pos
        animator.transform.position = sword.GetPlayerPos(true);
        animator.transform.rotation = sword.transform.rotation;
        rigid.velocity = (Vector2)sword.rigid.velocity;
        sword.gameObject.SetActive(false);
        Fx(catchFx);

        heldSword.SetActive(true);
        Flash(0.3f);
        GameControl.main.camc.UpdateTarget();
        audios.PlayOneShot(catchSound, 0.35f);
    }

    public void AreaDamage(Vector3 position, float damage, float radius, float knockback = 5f) {
        int n = Physics.OverlapSphereNonAlloc(position, radius, colresult, 1);
        for (int i = 0; i < n; i++) {
            if (colresult[i].gameObject.CompareTag("Enemy")) {
                Enemy e;
                if (colresult[i].gameObject.TryGetComponent(out e)) {
                    e.Damage(damage, knockback, gameObject);
                }
            }
        }
    }

    public void Damage(float damage, Enemy enemy = null) {
        if (dead) return;
        if(damage < 0f) {
            health = Mathf.Min(health - damage, maxHealth);
        }
        else {
            if (invincibility > 0) return;
            if(enemy != null && pounding && state == State.pound) {
                //actually, the enemy should be dying
                enemy.Damage(sword.GetPoundDamage() * blade.damageMultiplier, 8f, gameObject);
            }
            else {
                health = Mathf.Min(health - damage * accessory.takeDamageMultiplier, maxHealth);
                invincibility = INVIN_TIME;
                if (health <= 0f) Kill();
                else if (enemy != null){
                    //process knockback
                    bool right = transform.position.x > enemy.transform.position.x;
                    rigid.velocity = (Vector3.up + Vector3.right * (right ? 1f : -1f)) * enemy.knockback;
                }
                animator.Trig(Trigger.hurt);
                audios.PlayOneShot(damageSound);
            }
        }
    }

    public void Kill() {
        if(dead) return;
        dead = true;
        coins -= 10;
        if (coins < 0) coins = 0;

        //todo death effect
        animator.Trig(Trigger.dies);
        Invoke("ReturnCheckpoint", 3f);
    }

    public void ReturnCheckpoint() {
        transform.position = GameControl.main.playerSpawn.position;
        rigid.velocity = Vector3.zero;
        invincibility = INVIN_TIME;

        state = State.none;
        nextState = State.idle;
        heldSword.SetActive(true);
        sword.gameObject.SetActive(false);
        landed = pounding = false;
        health = maxHealth;
        dead = false;
    }

    public void OnPartUpdate() {
        //todo fx
        animator.Trig(Trigger.upgrade);
        StartCoroutine(IPartUpdateFx());
    }

    private IEnumerator IPartUpdateFx() {
        yield return new WaitForSeconds(animTime.swordUp);
        Fx(swordUpgradeFx, heldSword.transform.position, heldSword.transform.rotation);
        audios.PlayOneShot(swordUpgradeSound);
    }

    private Vector2 FingerPos() {
        return Input.mousePosition * Settings.FlingSensitivity / 10f;
    }

    public Vector2 ThrowVector() {
        float cstr = (dragCurrentPos - dragStartPos).magnitude;
        if (cstr == 0) return Vector2.zero;
        return -(dragCurrentPos - dragStartPos) / cstr * (cstr > maxDragLength ? maxDragLength : cstr) * throwStr;
    }

    //todo pool fx
    public void Fx(GameObject fx) {
        Fx(fx, transform.position, transform.rotation);
    }

    public void Fx(GameObject fx, Vector3 position, Quaternion rotation) {
        if (fx == null) return;
        Instantiate(fx, position, rotation);
    }

    public void SetSwordColor(Gradient color) {
        swordTrail.trail.colorGradient = color;
    }

    public Gradient GetSwordColor() {
        return swordTrail.trail.colorGradient;
    }

    public void Flash(Gradient color, float duration) {
        flash.Set(color, duration);
    }

    public void Flash(float duration) {
        Flash(GetSwordColor(), duration);
    }

    [System.Serializable]
    public class AnimationTime {
        public float throwing, pound, pick, landBounce, swordUp;
    }
}
