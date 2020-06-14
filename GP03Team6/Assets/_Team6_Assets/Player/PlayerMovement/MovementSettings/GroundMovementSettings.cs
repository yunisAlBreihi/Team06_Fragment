using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class GroundMovementSettings 
{
    [Header ("Speed Settings")]
    public float speed = 7f;
    public float relAccel = 6f;
    public float absAccel = 4.5f;

    [Header ("Jumping from Ground")]
    public float jumpCooldown = 0.4f;
    public float jumpForce = 8.5f;
    public float jumpTargetSpeed = 10f;
    public float jumpDelay = 0.05f;
    [Tooltip ("For how long do we buffer the jump input? How early can the player press the button?")]
    public float bufferTime = 0.18f;

    [Header("Other")]
    public float rotationLerpSpeed = 5f;
    public float pushSpeed = 5f;
    public float pushAccel = 2f;
    public float boxTurnSpeed = 1.7f;
}
