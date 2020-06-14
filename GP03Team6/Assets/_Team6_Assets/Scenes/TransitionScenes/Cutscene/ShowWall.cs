using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWall : MonoBehaviour
{

    [SerializeField]
    public GameObject wallToShow;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            wallToShow.SetActive(true);
        }
    }
}
