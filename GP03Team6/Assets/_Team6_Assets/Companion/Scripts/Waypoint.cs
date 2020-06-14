using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private WaypointsManager waypointsManager;
    
    [SerializeField]
    private Animator ojnar;

    private void Awake()
    {
        waypointsManager.WaypointLists.Add(this);
    }

    public float GetDistanceToOjnar()
    {
        float distance = Vector3.Distance(transform.position, ojnar.transform.position);
        return distance;
    }

    public void DisableWayPoint()
    {
        waypointsManager.WaypointLists.Remove(this);
    }
}