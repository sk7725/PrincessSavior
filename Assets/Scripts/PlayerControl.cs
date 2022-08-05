using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private const float MIN_DRAG_LEN2 = 0.15f, HEIGHT = 2f;

    [Header("Settings")]
    public float poundStr = 5f;
    public float throwStr = 5f, bounceStr = 15f;
    [SerializeField] private Vector3 throwTorque;
    [SerializeField] private Vector2 throwOffset;
    [SerializeField] private Quaternion throwRotation = Quaternion.identity;

    //preset fields
    [Header("Preset Fields")]
    public Sword sword;
    public AnimationTime animTime;

    [Header("Fx")]
    public GameObject poundFx;
    public GameObject throwFx, catchFx;

    public State state, nextState;
    public float stateTime = 0f; //time after a state change

    public bool landed;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] private Collider col;
    private float swordOffset;

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
    }

    void Update()
    {
        stateTime += Time.deltaTime;
        CheckLanded();
        UpdateInput();

        //AUTO STATE CHANGE
        if (nextState == State.none) {
            switch (state) {
                case State.preThrow:
                    if (stateTime >= animTime.throwing) {
                        nextState = State.thrown;
                    }
                    break;
                case State.thrown:
                    //todo check if sword is airborne
                    //if not, yeet sword up and then switch to pick
                    if (sword.landed) {
                        sword.rigid.velocity = Vector3.up * bounceStr;
                        nextState = State.pound;
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
                    CatchSword();
                    break;
            }
            stateTime = 0f;
        }

        //UPDATE (per state)
        switch (state) {
            case State.pound:
                if(stateTime > animTime.pound) {
                    rigid.AddForce(Vector3.down * poundStr, ForceMode.VelocityChange);
                }
                else {
                    //todo fuck
                    //first, lerp animator position
                    //  1. store current "sword" pos
                    //  2. unoffset -> rotate -> reoffset
                    //then, set player pos to animator pos
                }
                break;
        }
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

    private void CheckLanded() {
        landed = Physics.CheckSphere(new Vector3(col.bounds.center.x, col.bounds.center.y - ((HEIGHT - 1f) / 2 + 0.15f), col.bounds.center.z), 0.45f, 1 << 6, QueryTriggerInteraction.Ignore);
    }

    private void ThrowSword() {
        sword.gameObject.SetActive(true);
        sword.transform.position = transform.position + (Vector3)throwOffset;
        //sword.rigid.MovePosition(transform.position + throwOffset);
        sword.rigid.MoveRotation(throwRotation);
        sword.rigid.velocity = ThrowVector();
        sword.rigid.angularVelocity = throwTorque;
        sword.Init();
        Fx(throwFx, transform.position + (Vector3)throwOffset, throwRotation);
    }

    private void CatchSword() {
        //set player pos to sword pos + sword offset
        rigid.MovePosition(sword.GetPlayerPos());
        //todo set playerrenderer pos to true sword pos
        //renderer.transform.position = sword.GetPosition(true);
        //renderer.transform.rotation = sword.transform.rotation;
        rigid.velocity = (Vector2)sword.rigid.velocity;
        sword.gameObject.SetActive(false);
        Fx(catchFx);
    }

    private Vector2 FingerPos() {
        return GameControl.main.cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 ThrowVector() {
        return -(dragCurrentPos - dragStartPos) * throwStr;
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
        public float throwing, pound, pick;
    }
}
