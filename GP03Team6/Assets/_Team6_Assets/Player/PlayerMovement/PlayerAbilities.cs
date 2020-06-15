using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerMovement p;
    private Vector3 jumpForce;

    public bool tryJump = false; //if true; we will try to jump for about 0.1 seconds, then stop trying to jump. This stores the players input if they press button too early.
    public float tryJumpCounter = 0f;

    public bool canJump = false;
    public bool jumpDisabled = false;
    public float jumpCD;
    public float jumpCounter;


    [FMODUnity.EventRef]
    [SerializeField] public string jump;

    private void Start()
    {
        p = GetComponent<PlayerMovement>();
        jumpForce = new Vector3(0f, p.groundSettings.jumpForce , 0f);
        jumpCD = p.groundSettings.jumpCooldown;
    }

    private void Update()
    {
        float t = Time.deltaTime;
        if(canJump == false && p.ReturnGrounded())  // This was in the if check :    && p.IsClimbing()
        {
            jumpCounter += t;
            if(jumpCounter > jumpCD)
            {
                canJump = true;
                jumpCounter = 0f;
            }
        }

        if(tryJump)
        {
            StartCoroutine(Jump());

            tryJumpCounter += t;
            if (tryJumpCounter > 0.2f)      
            {
                tryJump= false;
                tryJumpCounter = 0f;
            }
        }
    }

    public IEnumerator Jump ()
    {
        if (canJump && jumpDisabled == false && (p.GroundAngle() < p.cc.slopeLimit||p.airState.airTime < 0.1f))
        {
            tryJump = false;
            jumpCounter = 0f;
            canJump = false;
            float initSpeed = p.groundSettings.speed;
            p.groundSettings.speed = p.groundSettings.jumpTargetSpeed;
            p.anim.SetTrigger("jump");
            FMODUnity.RuntimeManager.PlayOneShotAttached(jump, p.gameObject);
            p.airState.grav = true;
            p.addMoveVector = jumpForce - new Vector3(0f,p.cc.velocity.y,0f);
            p.addToMove = true;
            yield return new WaitForSeconds(0.2f);
            p.groundSettings.speed = initSpeed;
            p.airState.grav = false;
        }
        else yield return null;
    }

    public IEnumerator Stagger(float speed, float duration)
    {
        Vector3 speedRemove = p.cc.velocity * -0.5f;
        p.addMoveVector = speedRemove;
        p.addToMove = true;

        float initSpeed = p.groundSettings.speed;
        p.groundSettings.speed = speed;
        p.groundMoveState.speedOverride = true;

        yield return new WaitForSeconds(duration);
        p.groundSettings.speed = initSpeed;
        Debug.Log("Stagger Complete");
    }




}
