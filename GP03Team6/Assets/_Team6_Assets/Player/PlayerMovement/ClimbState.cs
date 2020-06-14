using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ClimbState// : IMovement
{
    //PlayerMovement p;
    //Transform climbHere;
    //private RaycastHit groundHit;
    //private RaycastHit groundHit2;
    //private float climbCD = 0.2f;
    //public bool isClimbing;

    //public ClimbState(PlayerMovement player)
    //{
    //    p = player;
    //}
    //public void Start()
    //{

    //    climbHere = p.transform.Find("climbTemp");
    //    isClimbing = true;
    //    RaycastHit raycastHit;
    //    if (Physics.SphereCast(p.transform.position, 0.5f, p.transform.forward, out raycastHit, Mathf.Infinity, p.climbChecker.WallClimbMask))
    //    {
    //        Vector3 tempVec = new Vector3(-raycastHit.normal.x, 0, -raycastHit.normal.z);
    //        p.transform.rotation = Quaternion.LookRotation(tempVec, p.transform.up);
    //    }

    //}
    //private Vector3 ClimbPosition()
    //{
    //    RaycastHit raycastHit;
    //    float offset = 0.5f;
    //    Vector3 climbHereRaypoint = new Vector3(climbHere.position.x, climbHere.position.y + offset, climbHere.position.z);
    //    while (!Physics.SphereCast(climbHereRaypoint, p.GetComponent<CapsuleCollider>().radius, Vector3.down, out raycastHit, 2, p.climbChecker.WallClimbMask))
    //    {
    //        climbHereRaypoint.y -= 0.1f;
    //        if (Vector3.Distance(p.transform.position, climbHereRaypoint) > 4 || Physics.SphereCast(climbHereRaypoint, p.GetComponent<CapsuleCollider>().radius, Vector3.down, out raycastHit, p.GetComponent<CapsuleCollider>().bounds.extents.y * 2, p.climbChecker.WallClimbMask))
    //        {
    //            break;
    //        }
    //    }

    //    return climbHereRaypoint;
    //}
    //public Vector3 UpdateState(Vector3 input, Vector3 vel)
    //{
    //    Debug.DrawLine(p.transform.position, climbHere.position + (climbHere.position - p.transform.position).normalized, Color.red);
    //    if (Input.GetButtonDown("Jump") && p.abilities.canJump)
    //    {
    //        if (climbHere)
    //        {
    //            if (Physics.Raycast(
    //                 p.transform.position,
    //                (climbHere.position - p.transform.position).normalized, out groundHit2,              
    //                Vector3.Distance(p.transform.position, climbHere.position + (climbHere.position - p.transform.position).normalized), 
    //                p.climbChecker.WallClimbMask))
    //            {
                   
    //                p.abilities.tryJump = true;
    //                p.StartCoroutine(p.SwitchState(p.airState));
    //            }
    //            else if (Physics.Raycast(climbHere.position, Vector3.down, out groundHit, 2.0f, p.climbChecker.WallClimbMask))
    //            {
    //                p.gameObject.SetActive(false);
    //                p.transform.position = ClimbPosition();
    //                p.gameObject.SetActive(true);
    //                p.StartCoroutine(p.SwitchState(p.airState));
    //            }
    //            else
    //            {
    //                p.abilities.tryJump = true;
    //                p.StartCoroutine(p.SwitchState(p.airState));


    //            }
    //        }

    //    }
    //    return Climbing(input, vel);
    //}
    //public Vector3 Climbing(Vector3 input, Vector3 vel)
    //{
    //    Vector3 move = vel;
    //    move.y = 0f;

    //    if (p.climbChecker.ClimbCheckPrediction(input) == 1)
    //    {
    //        Vector3 targetVel = -p.transform.right * p.groundSettings.speed;

    //        move = Vector3.Lerp(move, targetVel, p.airSettings.relAccel * Time.deltaTime);              //Horizontal Acceleration
    //        move = Vector3.MoveTowards(move, targetVel, p.airSettings.absAccel * Time.deltaTime);       //Horizontal Acceleration
    //    }
    //    else if (p.climbChecker.ClimbCheckPrediction(input) == 2)
    //    {
    //        Vector3 targetVel = p.transform.right * p.groundSettings.speed;

    //        move = Vector3.Lerp(move, targetVel, p.airSettings.relAccel * Time.deltaTime);              //Horizontal Acceleration
    //        move = Vector3.MoveTowards(move, targetVel, p.airSettings.absAccel * Time.deltaTime);       //Horizontal Acceleration
    //    }
    //    else
    //    {
    //        move = Vector3.zero;
    //    }
    //    // move.y = GravityAccel();

    //    return move;
    //}
    //public void ExitState()
    //{

    //    isClimbing = false;
    //}
}
