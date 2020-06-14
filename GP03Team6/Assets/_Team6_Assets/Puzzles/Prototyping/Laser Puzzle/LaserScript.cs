using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    private LineRenderer laser;
    [SerializeField]private float laserReach=500f;
    public LaserReciever reciever;
    public LayerMask layers;

    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positions = new Vector3[4];
        Vector3[] dir = new Vector3[3];

        RaycastHit hit;

        int posCount = 0;
        positions[0] = transform.position;
        dir[0] = transform.forward;

        bool b = true;

        if (Physics.Raycast(positions[0], dir[0], out hit, laserReach, layers))
        {
            
            posCount++;
            positions[posCount] = hit.point;
            LaserReciever l = hit.collider.gameObject.GetComponent<LaserReciever>();
            if (l != null)
            {
                HitReciever(l);
                b = false;
            }
            else if(hit.collider.gameObject.CompareTag("Reflective") == false)
            {
               
                if (reciever != null)
                {
                    Disconnect();
                }
                b = false;
            }
        }
        else 
        {
            if (reciever != null)
            {
                Disconnect();
            }
            posCount++;
            positions[posCount] = transform.position + transform.forward * laserReach;
            b = false;
        }
        
        while(b && posCount < 3)
        {
            dir[posCount] = Quaternion.AngleAxis(180f, hit.normal) * (dir[posCount - 1] * -1f);


            if (Physics.Raycast(positions[posCount], dir[posCount], out hit, laserReach, layers))
            {
                posCount++;
                positions[posCount] = hit.point;

                LaserReciever l = hit.collider.gameObject.GetComponent<LaserReciever>();
                if (l != null)           //Checking if we hit reciever
                {
                    HitReciever(l);
                    b = false;
                }
                else if (hit.collider.gameObject.CompareTag("Reflective") == false)
                {
                    if (reciever != null)
                    {
                        Disconnect();
                    }
                    b = false;
                }
            }
            else
            {
                if (reciever != null)
                {
                    Disconnect();
                }
                posCount++;
                positions[posCount] = positions[posCount-1] + dir[posCount-1] * laserReach;
                b = false;
            }

        }
        Vector3[] pos = new Vector3[posCount+1];
        
        for(int i = 0; i < posCount +1;  i ++)
        {
            pos[i] = positions[i];
        }
        laser.positionCount = posCount +1;
        laser.SetPositions(pos);

    }

    public void HitReciever(LaserReciever l)
    {
        if (!l.connectedBeams.Contains(this.gameObject))
        {
            l.connectedBeams.Add(this.gameObject);
            reciever = l;
            l.OpenDoor();
        }
    }
   
    public void Disconnect()
    {
        if (reciever.connectedBeams.Contains(this.gameObject))
        {
            reciever.connectedBeams.Remove(this.gameObject);

            reciever.CloseDoor();
        }
    }
}
