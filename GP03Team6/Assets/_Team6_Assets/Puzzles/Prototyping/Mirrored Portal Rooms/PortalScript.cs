using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{

    public Transform otherPortal;
    public bool b = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && b)
        {
            Vector3 offset = other.gameObject.transform.position - transform.position;
            otherPortal.gameObject.GetComponent<PortalScript>().b = false;
            StartCoroutine(Wait());
            other.gameObject.transform.position = otherPortal.transform.position + offset;
        }

    }

    private IEnumerator Wait ()
    {

        yield return new WaitForSeconds(0.2f);
        otherPortal.gameObject.GetComponent<PortalScript>().b = true;
    }

}
