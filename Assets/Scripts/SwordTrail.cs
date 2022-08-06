using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrail : TrailFollower
{
    protected override bool UpdateTarget() {
        return GameControl.main.player.state == PlayerControl.State.thrown || GameControl.main.player.state == PlayerControl.State.pound;
    }
}
