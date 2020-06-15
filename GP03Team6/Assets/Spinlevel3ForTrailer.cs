using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinlevel3ForTrailer : MonoBehaviour
{
    public bool b;

    public Transform t;



    private Quaternion q;


    private void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {if (other.gameObject.CompareTag("Player"))
        {
            b = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(b)

        {
            q = Quaternion.Euler(0f, 6f *Time.deltaTime, 0f);
            t.rotation *= q;
        }
    }
}
