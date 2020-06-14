using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMovement : MonoBehaviour
{

    [Range(0.1f, 15.0f)] public float speed;
    [Range(0.1f, 15.0f)] public float waitTime;
    private float mag;
    public Transform targetPoint;
    public Vector3 target;
    private Vector3 origin, dir;
    public bool hasPlayer = false, reverse = false;
    private bool gate = true;

    private void Start()
    {
        origin = transform.position;
        target = targetPoint.position;
        dir = (target - origin).normalized;
        mag = (target - origin).magnitude;
        
    }

    private void Update()
    {
        if (hasPlayer == false)
        {
           Move(MovePlatform());
        }
        else
        {
           
        }
       
    }

  
    public Vector3 MovePlatform()
    {
        if ((transform.position - origin).magnitude > mag && gate)
        {
            StartCoroutine(WaitThenSwitch());
        }
        return dir * speed;
    }

    public IEnumerator WaitThenSwitch()
    {
        gate = false;
        float s = speed;
        speed = 0f;
        yield return new WaitForSeconds(waitTime);
        speed = s;
        Switch();
        yield return new WaitForSeconds(0.1f);
        gate = true;
    }

    public void Switch()
    {
        Vector3 o = origin;
        origin = target;
        target = o;
        dir = (target - origin).normalized;
    }

    public void Move(Vector3 move)
    {
        transform.position += move * Time.deltaTime;
    }

}