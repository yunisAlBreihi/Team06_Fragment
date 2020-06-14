using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitatingObject : MonoBehaviour
{
    Vector3 offset, start, target;

   [SerializeField] private float speed, rotationSpeed = 1f;
    [SerializeField] private float targetRange;


    private Quaternion rot;
    [SerializeField] private bool rotate = false;
    public float xRot, zRot, yRot;
   

    void Start()
    {
        rot = Random.rotation;
        start = gameObject.transform.position;
        offset = new Vector3 (0f, Random.Range(-targetRange, targetRange), 0f);
        xRot = Random.Range(xRot, -xRot);
        zRot = Random.Range(zRot, -zRot);
        yRot = Random.Range(yRot, -yRot);


        target = start + offset;
        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        float targetSpeed = (0.1f + Vector3.Distance(transform.position, target));

       

        speed = Mathf.Lerp(speed, targetSpeed, Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, target, speed*Time.deltaTime);

        if(Vector3.Distance(transform.position, target )< 0.1f)
        {
            offset *= -1f;
            target = start + offset;
        }

        if(rotate)
        {
            transform.rotation *= Quaternion.Euler(Time.deltaTime * rotationSpeed * xRot, Time.deltaTime * rotationSpeed * yRot, Time.deltaTime * rotationSpeed * zRot);
        }

    }
}
