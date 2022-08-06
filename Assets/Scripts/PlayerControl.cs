using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public const float MIN_DRAG_LEN2 = 0.15f, HEIGHT = 2f;

    [Header("Settings")]
    public float poundStr = 5f;
    public float throwStr = 5f, bounceStr = 15f;
    public float maxDragLength = 3f;
    [SerializeField] private Vector3 throwTorque;
    [SerializeField] private Vector2 throwOffset;
    [SerializeField] private Quaternion throwRotation = Quaternion.identity;

    //preset fields
    [Header("Preset Fields")]
    public Sword sword;
    public GameObject heldSword;
    public AnimationTime animTime;
    public PlayerRenderer animator;

    [Header("Fx")]
    public GameObject poundFx;
    public GameObject throwFx, catchFx, swordBounceFx, catchPrevFx;

    public State state, nextState;
    public float stateTime = 0f; //time after a state change

    public bool landed, pounding, landBounce;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] private Collider col;
    private float swordOffset, landBounceTimer;
    private Quaternion? targetRotation;

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
    }

    void Start()
    {
        state = State.none;
        nextState = State.idle;
        swordOffset = sword.playerOffset;
        sword.gameObject.SetActive(false);
        landed = pounding = false;
    }

    void Update()
    {
        stateTime += Time.deltaTime;
        CheckLanded();
        UpdateInput();
        bool updateTransform = true; //update animator transform

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
                            sword.rigid.velocity = Vector3.up * bounceStr;
                            landBounce = true;
                            landBounceTimer = 0;
                            Fx(swordBounceFx, sword.transform.position, Quaternion.identity);
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
                case State.thrown:
                    ThrowSword();
                    break;
                case State.pound:
                    pounding = false;
                    targetRotation = sword.rigid.velocity.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
                    CatchSword();
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
                    rigid.AddForce(Vector3.down * poundStr, ForceMode.VelocityChange);
                }
                else {
                    //lerp animator z
                    updateTransform = false;
                }
                break;
        }

        UpdateAnimator(updateTransform);
    }

    private void UpdateInput() {
        bool pressed = Input.GetMouseButton(0);

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
                        Debug.Log(dragCurrentPos);
                    }
                }
                else {
                    if (pressed) {
                        //start drag
                        dragged = true;
                        dragStartPos = dragCurrentPos = FingerPos();
                        Debug.Log(dragStartPos);
                    }
                }
                break;
            case State.thrown:
                if (pressed) {
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
        }
    }

    private void CheckLanded() {
        landed = Physics.CheckSphere(new Vector3(col.bounds.center.x, col.bounds.center.y - ((HEIGHT - 1f) / 2 + 0.15f), col.bounds.center.z), 0.45f, 1 << 6, QueryTriggerInteraction.Ignore);
    }

    private void ThrowSword() {
        landBounce = false;
        sword.gameObject.SetActive(true);
        sword.transform.position = transform.position + (Vector3)throwOffset;
        sword.rigid.MoveRotation(throwRotation);
        sword.rigid.velocity = ThrowVector();
        sword.rigid.angularVelocity = throwTorque;
        sword.Init();
        Fx(throwFx, transform.position + (Vector3)throwOffset, throwRotation);

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
        GameControl.main.camc.UpdateTarget();
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

    [System.Serializable]
    public class AnimationTime {
        public float throwing, pound, pick, landBounce;
    }
}
