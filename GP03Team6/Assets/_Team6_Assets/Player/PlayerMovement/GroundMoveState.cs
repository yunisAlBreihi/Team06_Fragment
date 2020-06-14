using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMoveState : IMovement
{
    PlayerMovement p;

    public PingPongMovement pl;

    public bool speedOverride;
    public float speedOverrideTarget;

    public GroundMoveState(PlayerMovement player)
    {
        p = player;
    }
    public void Start()
    {
        p.abilities.jumpCounter = 0f;
        p.abilities.canJump = false;
       p.cc.stepOffset = 0.5f;
    }

    public Vector3 UpdateState(Vector3 input, Vector3 vel)
    {
        if(p.ReturnGrounded() == false)
        {
            p.StartCoroutine (p.SwitchState(p.airState));
            return p.airState.UpdateState(input, vel);
        }
        if (p.ReturnGrounded() && p.GroundAngle() > p.cc.slopeLimit)
        {
            p.StartCoroutine(p.SwitchState(p.slopeSlideState));
            return p.slopeSlideState.UpdateState(input, vel);
        }

        if (p.input.Buttons() == "Jump")
        {
            p.abilities.tryJump = true;
        }

        if (p.ReturnGroundHit().collider.gameObject.GetComponent<PingPongMovement>() != null)
        {
            pl = p.ReturnGroundHit().collider.gameObject.GetComponent<PingPongMovement>();
            Vector3 platformMove = PlatformMovement();

            if (input.magnitude != 0f)
            {
                UpdateRotation(vel - platformMove + input);
            }
            pl.Move(platformMove);
            return CalcMovement(input, (vel - platformMove)) + platformMove;
        }
        else if (pl != null)
        {

            pl.hasPlayer = false;
            pl = null;
        }
       
        UpdateRotation((vel.magnitude < 3f ? Vector3.zero : vel) + input);
        return CalcMovement(input,vel);
    }
    public Vector3 CalcMovement(Vector3 input, Vector3 vel)
    {
        Vector3 move = vel;
        move.y = 0f;

        Vector3 targetVel = input * p.groundSettings.speed;

        move = Vector3.Lerp(move, targetVel, p.groundSettings.relAccel * Time.deltaTime);
        move = Vector3.MoveTowards(move, targetVel, p.groundSettings.absAccel * Time.deltaTime);
        move.y = GravityAccel();

        return move;
    }
    public float GravityAccel()                        //Vertical Acceleration
    {
        float ySpeed = p.cc.velocity.y + (Time.deltaTime * p.airSettings.gravity);
        return Mathf.Clamp(ySpeed, -5f, 5f);
    }

    public void UpdateRotation(Vector3 targetForward)
    {
        Vector3 fRot = p.gameObject.transform.forward;
        fRot = Vector3.RotateTowards(fRot, targetForward, p.groundSettings.rotationLerpSpeed * Time.deltaTime, 0f);
        fRot.y = 0f;
        p.gameObject.transform.rotation = Quaternion.LookRotation(fRot, Vector3.up);
    }

    private Vector3 PlatformMovement ()
    {
        pl.hasPlayer = true;

        return pl.MovePlatform();
    }

    public void ExitState()
    {
    }
}
