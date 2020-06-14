using UnityEngine;

public class Transitions
{
    public const string stateName = "State";
    public const string isFollowingPlayerName = "IsFollowingPlayer";
    public const string isGoingToWaypointName = "IsGoingToWaypoint";

    public enum Transition 
    {
        IDLE,         //0
        WANDER,       //1
        FOLLOWPLAYER, //2
        MOVE,         //3
        ATTACK,       //4
        INTERACT,     //5
        JUMP,         //6
        LAND,         //7
        FOLLOWWAPOINTS//8  
    }
}
