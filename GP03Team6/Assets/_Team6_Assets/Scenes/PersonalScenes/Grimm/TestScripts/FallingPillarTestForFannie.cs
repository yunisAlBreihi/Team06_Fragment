using System.Collections;
using UnityEngine;

public class FallingPillarTestForFannie : MonoBehaviour
{
    [Range(0.75f, 0)]
    public float fallRange = 0.7f;
        public float acceleration = 1f;
    private Transform pillar;
    private bool isTriggerd = false;
    public float rotation = 0f, delay = 0.5f;
    private Quaternion startRot, targetRot;
    [Header ("How many degrees to rotate by :)")]
    [SerializeField] private Vector3 rotationEuler;

    public GameObject sound, vfx;

    void Start()
    {
        vfx.SetActive(false);
        sound.SetActive(false);
        targetRot = transform.rotation * Quaternion.Euler(rotationEuler.x, rotationEuler.y, rotationEuler.z);
        pillar = gameObject.transform.parent;
        
    }

    private void Update()
    {
        if (isTriggerd)
        {

            RotateTrunk();
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Delay());

        }
    }

    private IEnumerator Delay()
    {
        sound.SetActive(true);
        yield return new WaitForSeconds(delay);
        isTriggerd = true;
    }

    void RotateTrunk()
    {
        acceleration  = Mathf.Lerp(acceleration, 1f, Time.deltaTime * 0.15f);

        rotation += Time.deltaTime * acceleration;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotation);

        if(transform.rotation == targetRot)
        {
            vfx.SetActive(true);
            Destroy(this);
        }

        //rotation += Time.deltaTime * acceleration;
        //pillar.transform.rotation *= Quaternion.AngleAxis(-rotation * Time.deltaTime, Vector3.left);

        //if (pillar.transform.rotation.x > fallRange)
        //{
        //    isTriggerd = false;
        //}
    }
}
