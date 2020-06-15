using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDoor : MonoBehaviour
{
    [SerializeField] private LazerPuzzle l;
    private Collider col;
    private float f = -5.5f;
    public float openSpeed = 10f;

    [SerializeField] private Material mat;

   [SerializeField] private bool open = false;

    [FMODUnity.EventRef]
    [SerializeField] public string openSound;
    [FMODUnity.EventRef]
    [SerializeField] public string closeSound;

    private PlayerMovement p;

    private MeshRenderer m;

    private void Start()
    {
        m = GetComponent<MeshRenderer>();
        mat = GetComponent<MeshRenderer>().material;
        col = GetComponent<Collider>();
        p = PlayerMovement.MyPlayer;
    }

    void Update()
    {
        if(l.ReachedGoal() && !open)
        {
            Open();
            open = true;
        }
        if(!l.ReachedGoal() && open)
        {
            Close();
            open = false;
        }

        f = Mathf.MoveTowards(f, (open?5.5f:-5.5f), Time.deltaTime * openSpeed);

        mat.SetFloat("_DissolveRate", f);
        if(f > 0f)
        {
            col.enabled = false;
            m.enabled = false;
        }
        else
        {
            col.enabled = true;
            m.enabled = true;
        }
    }

    private void Open()
    {
        Invoke("OpenSound", 0f);
    }
    private void Close()
    {
        Invoke("CloseSound", 0f);
    }
    private void OpenSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(openSound, gameObject);
    }
    private void CloseSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(closeSound, gameObject);
    }
}
