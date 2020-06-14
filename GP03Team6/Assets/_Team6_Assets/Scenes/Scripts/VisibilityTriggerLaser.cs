using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTriggerLaser : VisibilityTriggerBase
{
    private LazerPuzzle lazerPuzzle;

    private void Start()
    {
        lazerPuzzle = GetComponent<LazerPuzzle>();
    }

    private void Update()
    {
        if (lazerPuzzle.ReachedGoal() && hasTriggered == false)
        {
            hasTriggered = true;
            SetVisibility(visibility);
        }
        //else
        //{
        //    SetVisibility(!visibility);
        //}
    }
}
