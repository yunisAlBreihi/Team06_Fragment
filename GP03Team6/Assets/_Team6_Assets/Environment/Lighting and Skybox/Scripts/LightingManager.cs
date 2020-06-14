using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LightingManager : MonoBehaviour
{
    [SerializeField,Tooltip("Link to the scenes directional light here")]Light directionalLight;
    [SerializeField, Tooltip("Link to the scenes fog and sky volume here")] Volume volume;
    [SerializeField, Tooltip("Which characters position that determines when the light fades")] GameObject character;
    [SerializeField, Tooltip("gradient of light colors to fade between")] Gradient fogGradient;
    [SerializeField, Tooltip("gradient of light colors to fade between")] Gradient lightGradient;
    [SerializeField, Tooltip("Which positions to use for transition points")] Transform[] transitionPosition;

    Fog fog;

    float distanceBetweenFirstToSecondPoint;
    float distanceBetweenSecondToThirdPoint;
    float distanceBetweenPlayerToTransitionPoint;
    float distancePercent;

    float cappedDistancePercent;

    int currentTransitionPointIndex = 0;

    Vector3 characterDirectionToSecondPoint;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet<Fog>(out fog);
        distanceBetweenFirstToSecondPoint = Vector3.Distance(transitionPosition[0].position,transitionPosition[1].position);
        distanceBetweenSecondToThirdPoint = Vector3.Distance(transitionPosition[1].position, transitionPosition[2].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTransitionPointIndex == 0)
        {
            distanceBetweenPlayerToTransitionPoint = Vector3.Distance(character.transform.position, transitionPosition[1].position);

            distancePercent = (1 - (distanceBetweenPlayerToTransitionPoint / distanceBetweenFirstToSecondPoint)) / 2;

            if (distanceBetweenPlayerToTransitionPoint < 10.0f)
            {
                currentTransitionPointIndex = 1;
            }
        }
        else if (currentTransitionPointIndex == 1)
        {
            distanceBetweenPlayerToTransitionPoint = Vector3.Distance(character.transform.position, transitionPosition[2].position);

            distancePercent = ((1 - (distanceBetweenPlayerToTransitionPoint / distanceBetweenSecondToThirdPoint)) / 2) + 0.5f;

            if (distanceBetweenPlayerToTransitionPoint < 10.0f)
            {
                currentTransitionPointIndex = 2;
            }
        }

        if (currentTransitionPointIndex < 2)
        {
            if (distancePercent < 0.0f)
            {
                directionalLight.color = lightGradient.Evaluate(0);
                fog.tint.value = fogGradient.Evaluate(0);
            }
            else
            {
                directionalLight.color = lightGradient.Evaluate(distancePercent);
                fog.tint.value = fogGradient.Evaluate(distancePercent);
            }
        }
        else
        {
            directionalLight.color = lightGradient.Evaluate(1);
            fog.tint.value = fogGradient.Evaluate(1);
        }
    }
}
