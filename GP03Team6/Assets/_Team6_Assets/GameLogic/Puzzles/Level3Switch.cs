using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Switch : InteractibleObject
{
    [SerializeField] private bool rotating = false;

    public LazerPuzzle l;

    [SerializeField] private Transform pivot, playerT;

    public float rotatePlayerThreshold;

    public GameObject[] objectsToUnparent;

    private Vector3 forward, target;

    private bool b = true;

    private Animator anim;

    private Quaternion rot;
    [SerializeField] private float rotSpeed = 10f;

    private void StartWithoutOverride()
    {
        anim = GetComponentInChildren<Animator>();
        playerT = PlayerMovement.MyPlayer.gameObject.transform;
        
        rot = Quaternion.Euler(0f, 90f, 0f);
    }

    public override void InteractedWith(GameObject other)
    {

        if (!rotating)
        {
            forward = pivot.forward;
            foreach (GameObject item in objectsToUnparent)
            {
                item.transform.parent = pivot.transform;
            }

            rotating = true;
            anim.SetBool("rotating", true);
            target = (rot * forward).normalized;
            target.y = forward.y = 0f;
            StartCoroutine(Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        Vector3 playerOffset = playerT.position - pivot.position;
        playerOffset.y = 0f;
        Vector3 playerTarget = rot * playerOffset;
     
        while (Vector3.Distance(forward, target) > 0.05f)
        {
            if (playerT.position.y > rotatePlayerThreshold)
            {
                playerOffset = Vector3.RotateTowards(playerOffset, playerTarget, Time.deltaTime * rotSpeed, 0f);
                playerT.position = new Vector3(pivot.position.x + playerOffset.x, playerT.position.y, pivot.position.z + playerOffset.z);
            }

            forward = Vector3.RotateTowards(forward, target, Time.deltaTime * rotSpeed, 0f);
            pivot.rotation = Quaternion.LookRotation(forward, Vector3.up);

            yield return new WaitForEndOfFrame();
        }

        pivot.rotation = Quaternion.LookRotation(target, Vector3.up);

        foreach (GameObject item in objectsToUnparent)
        {
            item.transform.parent = null;
        }
        rotating = false;
        anim.SetBool("rotating", false);
    }


    void Update()
    {
        if(b)
        {
            StartWithoutOverride();
            b = false;
        }

        canInteract = l.ReachedGoal();

    }
}
