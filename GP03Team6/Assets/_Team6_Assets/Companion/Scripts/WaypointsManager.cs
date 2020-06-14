using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    private static WaypointsManager waypointsManager;
    public WaypointsManager WaypointManager => waypointsManager;

    private List<Waypoint> waypointList = new List<Waypoint>();
    public List<Waypoint> WaypointLists => waypointList;

    private void Start()
    {
        waypointsManager = this;
    }

    public Waypoint GetClosestWaypoint()
    {
        Waypoint closestWaypoint = null;
        float closestDistanceToTarget = Mathf.Infinity;
        
        foreach (Waypoint item in waypointList)
        {
            if (item.GetDistanceToOjnar() < closestDistanceToTarget)
            {
                closestDistanceToTarget = item.GetDistanceToOjnar(); 
                closestWaypoint = item;
            }
        }

        if (closestWaypoint != null)
        {
            return closestWaypoint;
        }

        return null;
    }
}
