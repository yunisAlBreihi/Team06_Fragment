using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.1f;

    private float angle = 0.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed, Space.World);
    }
}
