using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField, Tooltip("Transform used for the end position the platform will Move to. specify any object for this")] private Transform endTransform;
    [SerializeField, Range(0.1f, 15.0f), Tooltip("How fast the platform moves")] private float movingSpeed = 1.0f;
    [SerializeField, Range(0.05f, 10.0f), Tooltip("The duration in which the platform waits before moving again")] private float pauseDuration = 1.0f;

    private Vector3 startPosition;
    private float moveDelta;

    public LayerMask layers;
    public bool b = false;

    private Collider col;
    private float traceHeight = 0.2f;



    private void Awake()
    {
        col = GetComponent<Collider>();
        startPosition = transform.position;
        //if (b)
        //{
        //    StartCoroutine(MoveToPosition(endTransform.position));
        //}
        //else
        //{
        //    StartCoroutine(MoveToPosition(startPosition));
        //}

    }

    public void Update()
    {
        Vector3 target = b ? endTransform.position : startPosition;

        moveDelta = Time.deltaTime * movingSpeed;
        Vector3 delta = Vector3.MoveTowards(transform.position, target, moveDelta) - transform.position;

        Debug.Log(delta);

        //MovePlayer(delta);
        transform.position += delta;

        if (transform.position == target)
        {
            StartCoroutine(MovementPause());
        }
    }


    IEnumerator MoveToPosition(Vector3 target)
    {
        while ((transform.position - target).magnitude > 0.1f)
        {
            moveDelta = Time.deltaTime * movingSpeed;
            Vector3 delta = Vector3.MoveTowards(transform.position, target, moveDelta) - transform.position;

            //MovePlayer(delta);
            transform.position += delta;
            yield return new WaitForEndOfFrame();
        }
       
        StartCoroutine(MovementPause());
    }

    //private void MovePlayer(Vector3 delta)
    //{
    //    PlayerMovement p = FindPlayer();
    //    if (p != null)
    //    {
    //        Debug.Log("PLAYER FOUND");
    //        //p.gameObject.SetActive(false);
    //        //p.gameObject.transform.position += delta;
    //        //p.gameObject.SetActive(true);
    //        p.gameObject.transform.position += delta;
    //    }
    //}

    //private PlayerMovement FindPlayer()
    //{
    //    Vector3 v = transform.position;
    //    v.y += traceHeight;
    //    Vector3 v2 = col.bounds.extents;
    //    v2.y += traceHeight;
    //    Collider[] colliders = Physics.OverlapBox(v, v2, transform.rotation);
    //    foreach (Collider item in colliders)
    //    {
    //        if (item.gameObject.GetComponent<PlayerMovement>() != null)
    //        {
    //            return item.gameObject.GetComponent<PlayerMovement>();
    //        }
    //    }
    //    return null;
    //}

    IEnumerator MovementPause()
    {
        b = !b;
        yield return new WaitForSeconds(pauseDuration);
      
        //if (b)
        //{
        //    StartCoroutine(MoveToPosition(endTransform.position));
        //}
        //else
        //{
        //    StartCoroutine(MoveToPosition(startPosition));
        //}
    }
}
