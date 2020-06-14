using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SlopeSlideState : IMovement
{

    private float slideSpeed;
    PlayerMovement p;

    public SlopeSlideState(PlayerMovement player)
    {
        p = player;
    }

    public void Start()
    {
        slideSpeed = 0f;
    }

    public Vector3 UpdateState(Vector3 input, Vector3 vel)
    {
        //Become Grounded
        if (p.ReturnGrounded() == false)        
        {
            p.StartCoroutine(p.SwitchState(p.airState));
            return p.airState.UpdateState(input, vel);
        }
        //Become AirBorne
        if (p.ReturnGrounded() && p.GroundAngle() < p.cc.slopeLimit)
        {
            p.StartCoroutine(p.SwitchState(p.groundMoveState));
            return p.groundMoveState.UpdateState(input, vel);
        }

        return (SlopeMovement(input, vel));
    }

    private Vector3 GetSlopeDirection()
    {
        Vector3 v = p.groundHit.normal;
        Vector3 dir = Vector3.ProjectOnPlane(Vector3.down, v);
        return dir.normalized;
    }

    public Vector3 SlopeMovement(Vector3 vel, Vector3 input)
    {
        Vector3 move = vel;
        move.y = 0f;

        Vector3 targetVel = input * p.groundSettings.speed;

        move = Vector3.Lerp(move, targetVel, p.airSettings.relAccel * Time.deltaTime);              //Horizontal Acceleration
        move = Vector3.MoveTowards(move, targetVel, p.airSettings.absAccel * Time.deltaTime);       //Horizontal Acceleration

        Vector3 slopeDir = GetSlopeDirection();
        slideSpeed += (Time.deltaTime * -p.airSettings.gravity * 5f);
        move += (slideSpeed * slopeDir * Time.deltaTime);

        move = Vector3.ClampMagnitude(move, -p.airSettings.maxFallSpeed);

        return move;
    }

    public void ExitState()
    {

    }


}
