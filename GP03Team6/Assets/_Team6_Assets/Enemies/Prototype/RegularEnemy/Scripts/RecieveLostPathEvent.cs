using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveLostPathEvent : MonoBehaviour
{
    private void OnEnable()
    {
        Enemy.OnLostPath += MakeSound;
    }

    private void OnDisable()
    {
        Enemy.OnLostPath -= MakeSound;
    }

    private void MakeSound() 
    {
        Debug.Log("Loud Yell!");
    }
}
