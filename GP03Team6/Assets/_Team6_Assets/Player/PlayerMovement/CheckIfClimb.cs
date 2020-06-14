using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfClimb : MonoBehaviour
{
    private Vector3 groundCastStart;
    private float groundCastRadius;
    private CapsuleCollider pushBox;
    public RaycastHit groundHit;
    private RaycastHit groundHitLeft;
    private RaycastHit groundHitRight;
    private float groundCastDistance = 1.0f;
    public LayerMask WallClimbMask;
    public GameObject rightChecker;
    public GameObject leftChecker;
    public GameObject targetObject;
    private CapsuleCollider rightCollider;
    private CapsuleCollider leftCollider;
    private Vector3 rightCast;
    private float rightRadius;
    private Vector3 leftCast;
    private float leftRadius;

    void Start()
    {
        rightCollider = rightChecker.GetComponent<CapsuleCollider>();
        leftCollider = leftChecker.GetComponent<CapsuleCollider>();
        pushBox = GetComponent<CapsuleCollider>();
        groundCastStart = (new Vector3(0f, pushBox.radius - pushBox.bounds.extents.y, 0f) + pushBox.center);
        groundCastRadius = pushBox.radius;
        rightCast = (new Vector3(0f, rightCollider.radius - rightCollider.bounds.extents.y, 0f) + rightCollider.center);
        rightRadius = rightCollider.radius;
        leftCast = (new Vector3(0f, leftCollider.radius - leftCollider.bounds.extents.y, 0f) + leftCollider.center); ;
        leftRadius = leftCollider.radius;
    }
    public bool ClimbCheck()
    {
       // Debug.DrawLine(transform.position - groundCastStart, transform.position + Vector3.down * groundCastDistance);
       bool pet = Physics.Raycast(transform.position - groundCastStart,  Vector3.down, out groundHit, groundCastDistance, WallClimbMask);   //Add a layerMask to this at some point.+
      // bool pot = Physics.Raycast(transform.position - groundCastStart,  Vector3.up, out groundHit, groundCastDistance, WallClimbMask);   //Add a layerMask to this at some point.+
        return pet;
    }
    public int ClimbCheckPrediction(Vector3 directionToClimb) // -1 Left, +1 Right
    {
        if (Input.GetAxis("KBHorizontal") < 0) //Left checker
        {
            Debug.DrawLine(leftChecker.transform.position - leftCast, leftChecker.transform.position + Vector3.down * 2f, Color.yellow);
            bool left = Physics.SphereCast(leftChecker.transform.position - leftCast, leftRadius, Vector3.down, out groundHitLeft, groundCastDistance, WallClimbMask);
            Debug.Log(left);
            if(left)
            return 1;   //Add a layerMask to this at some point.
            else
                return 0;
        }
        else if ( Input.GetAxis("KBHorizontal") > 0) //Right checker
        {
            Debug.DrawLine(rightChecker.transform.position - rightCast, rightChecker.transform.position + Vector3.down * 2f, Color.red);
            bool right = Physics.SphereCast(rightChecker.transform.position - rightCast, rightRadius, Vector3.down, out groundHitRight, groundCastDistance, WallClimbMask);
            if (right)
                return 2;
            else
                return 0;
        }
        else
            return 0;

    }
}
