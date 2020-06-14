using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDoorDouble : MonoBehaviour
{
    [SerializeField] private LazerPuzzle[] l;
    private Collider col;
    private float f = -5.5f;
    public float openSpeed = 10f;

    [SerializeField] private Material mat;

   [SerializeField] private bool open = false;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        col = GetComponent<Collider>();
    }

    void Update()
    {
        bool b = true;
        foreach (LazerPuzzle item in l)
        {
            if(!item.ReachedGoal())
            {
                b = false;
            }
        }

        if (b && !open)
        {
            Open();
            open = true;
        }
        if (!b && open)
        {
            Close();
            open = false;
        }

        f = Mathf.MoveTowards(f, (open?5.5f:-5.5f), Time.deltaTime * openSpeed);

        mat.SetFloat("_DissolveRate", f);
        if (f > 0f)
        {
            col.enabled = false;
            gameObject.SetActive(false);
        }
        else
        {
            col.enabled = true;
            gameObject.SetActive(true);
        }
    }

    private void Open()
    {
       
    }
    private void Close()
    {
        col.enabled = true;
        
    }
}
