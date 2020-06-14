using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ClimbCheck : MonoBehaviour
{
    [SerializeField] public ClimbSettings settings;

    private PlayerMovement p;

    private CapsuleCollider col;
    private GameObject currentClimbBox;
    public Vector3 rHand, lHand;

    public Quaternion targetRot;
    private Vector3 flatNormal;
    private Vector3 capsEndPos;
    public Vector3 targetCenter;
    public Vector3 climbUpTarget;

    private float surfaceAngle;
    private float pointsOffset;
    private float radius;

    private bool onCooldown = false;

    private RaycastHit wallhit, topHit;

    private void Start()
    {
        p = gameObject.GetComponent<PlayerMovement>();
        col = GetComponent<CapsuleCollider>();
        radius = col.radius;
        pointsOffset = (col.height * 0.5f) - radius;
    }


    private Vector3 CapsulePoint(float offset, Vector3 center)
    {
        return (transform.up * offset + col.center + center);
    }

    public void TryClimb()
    {
        if (onCooldown == false)
        {
            if (TraceForward())
            {
                surfaceAngle = CheckForSurface();

                if (surfaceAngle < settings.maxSurfaceAngle && LookForCorner())
                {
                    if (CheckObstruction() == false)
                    {
                        targetRot = Quaternion.LookRotation(flatNormal * -1f, Vector3.up);
                        StartCoroutine(p.SwitchState(p.climbingState));
                    }

                }
            }
        }

    }

    //Caps tracing down from above to find the surface. Returns the angle of the surface.

    //Traces forward for climbable, then checks for obstructions.
    private bool TraceForward()
    {
        //Center of collider at end-point?
        if (Physics.CapsuleCast(CapsulePoint(pointsOffset, transform.position), CapsulePoint(-pointsOffset, transform.position), radius, transform.forward, out wallhit, settings.forwardReach, settings.climbable))
        {
            Vector3 playerPos = transform.position;
            capsEndPos = wallhit.point - transform.forward * radius * 1.1f;
            playerPos.y = 0f;
            capsEndPos.y = 0f;
            float traceDist = Vector3.Distance(playerPos, capsEndPos);
            if (Physics.CapsuleCast(CapsulePoint(pointsOffset, transform.position), CapsulePoint(-pointsOffset, transform.position), radius, transform.forward, traceDist, settings.obstructing)
                || Vector3.Angle(wallhit.normal, Vector3.ProjectOnPlane(wallhit.normal, Vector3.up)) > 7.87f)       //7.87 is arbitrary, no need to be scared of changing it lol
            {
                //Debug.Log("Traceforward: Wall obstructed");
                return false;
            }
            else
            {
                //Debug.Log("Traceforward: Wall Found");
                currentClimbBox = wallhit.transform.gameObject;
                return true;
            }
        }

        //Debug.Log("Traceforward: No Wall Found");
        return false;
    }

    private float CheckForSurface()
    {
        flatNormal = Vector3.ProjectOnPlane(wallhit.normal, Vector3.up).normalized;
        Vector3 capsCenter = wallhit.point - (flatNormal * (radius + 0.1f));        // 0.1f because we dont wanna be too close to the edge after climbing up.
        capsCenter.y = transform.position.y + settings.upwardReach + col.bounds.extents.y * 0.51f;

        float traceLength = capsCenter.y - transform.position.y;

        if (Physics.CapsuleCast(CapsulePoint(pointsOffset, capsCenter), CapsulePoint(-pointsOffset, capsCenter), radius, Vector3.down, out topHit, traceLength, settings.climbable))
        {
            float angle = Vector3.Angle(topHit.normal, Vector3.up);

            if (angle < settings.maxSurfaceAngle)
            {
                return angle;
            }
        }
        return settings.maxSurfaceAngle + 1f;
    }

    private bool LookForCorner()
    {
        float hitAngle = 99f;
        float yHeightOffset = 0f;
        float yHeight = wallhit.point.y + 0.5f;
        RaycastHit hit = new RaycastHit();

        Vector3 startPos = wallhit.point + flatNormal * 0.5f;

        Vector3 traceDir = Vector3.down - flatNormal;

        while (hitAngle > settings.maxSurfaceAngle && yHeightOffset < settings.upwardReach + 1f)
        {
            startPos.y = yHeight + yHeightOffset;

            if (Physics.Raycast(startPos, traceDir, out hit, 1.5f, settings.climbable))
            {
                hitAngle = Vector3.Angle(hit.normal, Vector3.up);
            }
            Debug.DrawLine(startPos, startPos + traceDir.normalized * 2f, Color.red);
            yHeightOffset += 0.05f;
        }

        if (hitAngle < settings.maxSurfaceAngle && SetTargetPos(hit.point))
        {
            lHand = hit.point + transform.right * settings.handOffset;
            rHand = hit.point - transform.right * settings.handOffset;

            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator ClimbCD(float time)
    {
        onCooldown = true;
        yield return new WaitForSeconds(time);
        onCooldown = false;
    }

    private bool SetTargetPos(Vector3 ledgePos)
    {
        targetCenter = ledgePos + flatNormal * radius * 1.01f + Vector3.down * (col.bounds.extents.y + 0.4f);


        if (Physics.OverlapCapsule(CapsulePoint(pointsOffset, targetCenter), CapsulePoint(-pointsOffset, targetCenter), radius, settings.both).Length > 0)
        {

            targetCenter.y += 0.4f;
            if (Physics.OverlapCapsule(CapsulePoint(pointsOffset, targetCenter), CapsulePoint(-pointsOffset, targetCenter), radius, settings.both).Length > 0)
            {

               for (int i = 0; i < Physics.OverlapCapsule(CapsulePoint(pointsOffset, targetCenter), CapsulePoint(-pointsOffset, targetCenter), radius, settings.both).Length; i++)
                {
                    if (Physics.OverlapCapsule(CapsulePoint(pointsOffset, targetCenter), CapsulePoint(-pointsOffset, targetCenter), radius, settings.both)[i].gameObject != currentClimbBox)
                    {                     
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private bool CheckObstruction()
    {
        Vector3 vec = wallhit.point;
        vec.y = transform.position.y;

        Vector3 target = vec + flatNormal * (radius * 0.5f + 0.001f);

        Vector3 p1 = CapsulePoint(pointsOffset, target);
        Vector3 p2 = CapsulePoint(-pointsOffset, target);
        Vector3 dir = Vector3.up;

        float dist = topHit.point.y + (col.height * 0.5f) + 0.2f - transform.position.y;


        if (Physics.CapsuleCast(p1, p2, radius, dir, dist, settings.obstructing) == false)
        {
            p1.y += dist;
            p2.y += dist;

            Vector3 hStart = p1;
            hStart.y = 0f;
            Vector3 hEnd = topHit.point;
            hEnd.y = 0f;

            float dist2 = Vector3.Distance(hEnd, hStart);

            if (Physics.CapsuleCast(p1, p2, radius, -flatNormal, dist2, settings.obstructing) == false)
            {
                climbUpTarget = topHit.point + new Vector3(0f, col.bounds.extents.y + 0.1f, 0f);

                return false;
            }
        }
        return true;
    }
}
