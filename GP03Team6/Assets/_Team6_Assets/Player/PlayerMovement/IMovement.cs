using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement // I = interface
{
    void Start();
    Vector3 UpdateState(Vector3 input, Vector3 vel);
    void ExitState();
}
