using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static PlayerControl;

public class ArrowUpdater : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Gradient color;
    [SerializeField] private float widthMultiplier = 0.2f;
    [SerializeField] private float playerOffset = 1f;

    void Start()
    {
        sprite.enabled = false;
    }

    void Update()
    {
        PlayerControl player = GameControl.main.player;
        bool enabled = player.state == State.idle && player.dragged;
        if (enabled) {
            Vector2 v = player.ThrowVector();
            float len = v.magnitude;
            if(len * len <= MIN_DRAG_LEN2 * player.throwStr * player.throwStr) {
                enabled = false;
            }
            else {
                transform.position = player.transform.position + (Vector3)v * widthMultiplier / 4f + (Vector3)v.normalized * playerOffset;
                transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, v));
                sprite.size = new Vector2(len * widthMultiplier, sprite.size.y);
                sprite.color = color.Evaluate(len / (player.maxDragLength * player.throwStr));
            }
        }
        sprite.enabled = enabled;
    }
}
