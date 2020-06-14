using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{

    [SerializeField]
    public GameObject blackScreen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BlackScreenTrigger")
        {
            this.gameObject.SetActive(true);
            blackScreen.gameObject.SetActive(true);
        }
    }
}
