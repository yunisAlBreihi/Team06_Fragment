using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{

    public Light light;

    public bool active;

    public float counter;

    void Start()
    {
        if (light == null)
        {
            Destroy(this);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (active)
        {
            counter -= Time.deltaTime *3f;
            light.spotAngle = counter * 10f;
            if(counter < 0f)
            {
                DeActivate();
            }
        }
        



    }

    public void Activate ()
    {
        light.gameObject.SetActive(true);
        active = true;
        counter = 11f;
    }
    public void DeActivate()
    {
        light.gameObject.SetActive(false);
       
        active = false;
    }
}
