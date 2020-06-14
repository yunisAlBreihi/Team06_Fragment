using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AirState : IMovement
{
    public float airTime;

    public bool grav = false;

    public ClimbCheck climbCheck;

    PlayerMovement p;
    private float climbingCD = 0.2f; //This is to avoid lots of 
    public AirState(PlayerMovement player)
    {
        p = player;
    }
    public void Start()
    {
        p.StartCoroutine(DisableJump());
        if(p.groundMoveState.pl != null)
        {
            p.groundMoveState.pl.hasPlayer = false;
            p.groundMoveState.pl = null;
        }
       climbingCD = 0.2f;
       // p.cc.stepOffset = 0f;
        airTime = 0f;
    }
    public Vector3 UpdateState(Vector3 input, Vector3 vel)
    {
        airTime += Time.deltaTime;
        p.anim.SetFloat("airTime", airTime);

        //climbingCD -= Time.deltaTime;
        if (p.ReturnGrounded() && p.GroundAngle()<p.cc.slopeLimit)     { p.StartCoroutine(p.SwitchState(p.groundMoveState));}

        //if (p.climbChecker != null)
        //{
        //    if (p.climbChecker.ClimbCheck() && !p.abilities.canJump && climbingCD < 0)     //Not sure if that needs to be there? Probably enough that the player is in the air to grab a ledge?
        //    {
        //        p.climbChecker.targetObject = p.climbChecker.groundHit.transform.gameObject;

        //        p.StartCoroutine(p.SwitchState(p.climbState));
        //    }
        //}

        if (vel.y < 0f)
        {
            climbCheck.TryClimb();
        }

        if (p.input.Buttons() == "Jump")     { p.abilities.tryJump = true;}

        if (airTime < 0.25f)                 { p.groundMoveState.UpdateRotation(vel);}

        return AirMovement(input, vel);
    }

    private IEnumerator DisableJump()
    {
        yield return new WaitForSeconds(0.1f);
        p.abilities.canJump = false;
    }
    public Vector3 AirMovement(Vector3 input, Vector3 vel)
    {
        Vector3 move = vel;
        move.y = 0f;

        Vector3 targetVel = input * p.groundSettings.speed;

        move = Vector3.Lerp(move, targetVel, p.airSettings.relAccel * Time.deltaTime);              //Horizontal Acceleration
        move = Vector3.MoveTowards(move, targetVel, p.airSettings.absAccel * Time.deltaTime);       //Horizontal Acceleration

        if (airTime > 0.1f || grav) { move.y = GravityAccel(); }
        else { move.y = 0f;}
        return move;
    }

    private float GravityAccel()                        //Vertical Acceleration
    {
        float ySpeed = p.cc.velocity.y + (Time.deltaTime * p.airSettings.gravity);
        return Mathf.Clamp(ySpeed, p.airSettings.maxFallSpeed, 12f);
    }

    public void ExitState()
    {
        p.anim.SetFloat("airTime", 0f);
        airTime = 0f;
    }
}
