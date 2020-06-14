using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class TentacleMovement : MonoBehaviour
{
    [SerializeField, Tooltip("The speed in which the tentacles move")] private float moveSpeed = 0.1f;
    [SerializeField,Tooltip("How far away the tentacles can move from its start position." +
        " this is for the base and for each spline point it is multiplied by an increment")]private float moveRadius = 1.0f;

    SplineComputer splineComputer;
    SplinePoint[] splinePoints;
    List<Vector3> startSplinePositions = new List<Vector3>();
    List<Vector3> lastPointPositions = new List<Vector3>();
    private void Awake()
    {
        splineComputer = GetComponent<SplineComputer>();
        splinePoints = splineComputer.GetPoints();

        for (int i = 0; i < splinePoints.Length; i++)
        {
            startSplinePositions.Add(splinePoints[i].position);
            lastPointPositions.Add(splinePoints[i].position);
        }
        StartCoroutine(MovePoints());

        //for (int i = 0; i < splinePoints.Length; i++)
        //{
        //    splinePoints[i].SetPosition
        //}
    }

    IEnumerator MovePoints()
    {
        float delta = 0;
        List<Vector3> EndPositions = new List<Vector3>();
        for (int i = 0; i < splinePoints.Length; i++)
        {
            EndPositions.Add(startSplinePositions[i] + Random.insideUnitSphere * (moveRadius * i));
        }

        while (delta <= 1.0f)
        {
            for (int i = 1; i < splinePoints.Length; i++)
            {
                splinePoints[i].SetPosition(Vector3.Lerp(lastPointPositions[i], EndPositions[i], delta));
            }
            delta += moveSpeed;
            splineComputer.SetPoints(splinePoints);

            yield return new WaitForSeconds(1.0f/15.0f);
        }

        SetLastPointPositions();
        StartCoroutine(MovePoints());
    }

    void SetLastPointPositions()
    {
        for (int i = 0; i < splinePoints.Length; i++)
        {
            lastPointPositions[i] = splinePoints[i].position;
        }
    }
}
