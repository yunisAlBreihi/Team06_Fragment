using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCrystalBop : MonoBehaviour
{
    [SerializeField, Tooltip("The speed of the crystals vertical bop")] float bopSpeed = 3.0f;
    [SerializeField, Tooltip("How fast the crystal rotates")] float rotationSpeed = 1.0f;
    [SerializeField, Tooltip("The distance upwards from the start position")] float bopDistance = 1.0f;

    Vector3 startPosition;
    Vector3 endPosition;

    float delta = 0.0f;

    Vector3 direction;

    // Start is called before the first frame update
    void Awake()
    {

        startPosition = transform.position;
        endPosition = startPosition + Vector3.up* bopDistance;
        delta = Random.Range(0.0f, 1.0f);

        direction = Vector3.up * Random.Range(-1, 2);
        while (direction == Vector3.zero)
        {
            direction = Vector3.up * Random.Range(-1, 2);
        }
        transform.position = Vector3.Lerp(startPosition, endPosition, delta);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up,1.0f);
        transform.position = Vector3.Lerp(startPosition, endPosition, delta);
        if (direction == Vector3.up)
        {
            delta += Time.deltaTime * bopSpeed;
            if (delta >= 1)
            {
                direction = Vector3.down;
            }
        }
        else
        {
            delta -= Time.deltaTime * bopSpeed;
            if (delta <= 0)
            {
                direction = Vector3.up;
            }
        }
    }
}
