using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInteractible : InteractibleObject
{
    [Header("The object that should rotate")]
    public GameObject obj;
    [Header("Where the player should hold on to")]
    public Transform handle;

    public override void InteractedWith(GameObject other)
    {
        PlayerMovement p = player.GetComponent<PlayerMovement>();
        p.beamMoveState.handle = handle;
        p.beamMoveState.obj = obj;
       StartCoroutine(p.SwitchState(p.beamMoveState));
    }


}
