using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReciever : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private Transform doorTarget;
    [SerializeField, Tooltip("False for dissolve and True for Open")] bool DissolveOrOpen;
    public Vector3 doorOrigin;
    public List<GameObject> connectedBeams;
    private bool open;

    private Material doorMaterial;
    private float dissolveRate;
    private float maxDissolveRate;

    private void Awake()
    {
        doorMaterial = door.GetComponent<Renderer>().material;
        maxDissolveRate = dissolveRate = -5.5f;
    }

    private void Start()
    {
        connectedBeams = new List<GameObject>();
        doorOrigin = door.position;
    }
    private void Update()
    {
        if (connectedBeams.Count == 0 && open)
        {
            if (DissolveOrOpen == true)
            {
                StartCoroutine(MoveDoor(doorOrigin));
            }
            else
            {
                StartCoroutine(FadeInDoor());
            }
        }
        else if (connectedBeams.Count > 0 && !open)
        {
            if (DissolveOrOpen == true)
            {
                StartCoroutine(MoveDoor(doorTarget.position));
            }
            else
            {
                StartCoroutine(FadeOutDoor());
            }
        }
    }

    public void OpenDoor()
    {
        if (DissolveOrOpen == true)
        {
            StartCoroutine(MoveDoor(doorTarget.position));
        }
        else
        {
            StartCoroutine(FadeInDoor());
        }
        open = true;
    }

    public void CloseDoor()
    {
        if (connectedBeams.Count < 1)
        {
            if (DissolveOrOpen == true)
            {
                StartCoroutine(MoveDoor(doorOrigin));
            }
            else
            {
                StartCoroutine(FadeOutDoor());
            }
            open = false;
        }
    }

    private IEnumerator MoveDoor(Vector3 target)
    {
        while (door.position != target)
        {
            door.position = Vector3.MoveTowards(door.position, target, Time.deltaTime * 55f);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeInDoor()
    {
        while (dissolveRate <= -maxDissolveRate)
        {
            doorMaterial.SetFloat("_DissolveRate", dissolveRate);
            dissolveRate += doorMaterial.GetFloat("_DissolveSpeed");
            yield return doorMaterial.GetFloat("_DissolveSpeed");
        }
        door.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutDoor()
    {
        door.gameObject.SetActive(true);
        while (dissolveRate >= maxDissolveRate)
        {
            doorMaterial.SetFloat("_DissolveRate", dissolveRate);
            dissolveRate -= doorMaterial.GetFloat("_DissolveSpeed");
            yield return doorMaterial.GetFloat("_DissolveSpeed");
        }
    }
}
