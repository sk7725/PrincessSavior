using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    public Animator animator;
    private PlayerControl player;

    public enum Trigger {
        thrown,
        upgrade,
        hurt,
        dies
    }

    private void Start()
    {
        player = GameControl.main.player;
    }

    private void Update()
    {
        animator.SetBool("landed", player.landed);
        animator.SetBool("prethrow", player.dragged && player.state == PlayerControl.State.idle);
        animator.SetBool("dead", player.dead);
        animator.SetBool("pounding", player.state == PlayerControl.State.pound);
    }

    public void Trig(Trigger trigger) {
        animator.SetTrigger(trigger.ToString());
    }
}
