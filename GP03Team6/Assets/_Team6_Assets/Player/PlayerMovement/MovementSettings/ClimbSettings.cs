using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ClimbSettings
{

    public float forwardReach = 0.5f;
    public float upwardReach = 2f;
    [Header ("Only Climbing surfaces")]
    public LayerMask climbable;
    [Header("All blockers, except climbing surfaces")]
    public LayerMask obstructing;
    [Header("All blockers, including Climbing surfaces")]
    public LayerMask both;

    public float maxSurfaceAngle = 25f;
    public float handOffset = 0.2f;

    public float entrySpeed = 3f;
    public float climbSpeed = 2f;


}
