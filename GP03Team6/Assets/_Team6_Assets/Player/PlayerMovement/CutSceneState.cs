using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneState : IMovement
{
    PlayerMovement p;
    private float initSens;

    public CutsceneState(PlayerMovement player)
    {
        p = player;

    }
    public void Start()
    {
        initSens = p.input.cameraSensitivity;
        p.input.cameraSensitivity = 0f;
    }
    public Vector3 UpdateState(Vector3 input, Vector3 vel)
    {
        Vector3 move = Vector3.zero;
        move.y = p.groundMoveState.GravityAccel();
        //Todo: Idle Anim & FX
        return move;
    }

    public void ExitState()
    {
        p.input.cameraSensitivity = initSens;
    }

}