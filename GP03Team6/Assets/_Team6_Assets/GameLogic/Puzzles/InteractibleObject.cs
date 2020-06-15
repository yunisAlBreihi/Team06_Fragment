using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class InteractibleObject : MonoBehaviour
{
    [Header("Overlap Sphere Settings")]
    [SerializeField] private Vector3 centerOffset;
    [SerializeField] private float radius;
    [SerializeField] private InteractPrompt interact;

    public bool canInteract = true;

    [Header("Custom Trigger Volume")]
    [Tooltip ("Use this to override the normal Sphere Collider for the interaction volume.")]
    [SerializeField] private Collider col;

    protected GameObject player;
    private PlayerMovement pInteract;
    private InputManager input;

    void Start()
    {
        
        if (col == null)
        {
            SphereCollider c = gameObject.AddComponent<SphereCollider>();
            c.radius = radius;
            c.center = centerOffset;
            col = c;
        }

        col.isTrigger = true;
        player = PlayerMovement.MyPlayer.gameObject;
        input = player.GetComponent<InputManager>();
        interact = player.GetComponent<PlayerMovement>().interactPrompt;
        if(interact != null)
        {
           
            Debug.Log("interactifds" +
                "afdsa" + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject == player)
        {
    
            interact.StartFadeIn();
            //Display Interact feedback (Show the player they can interact)
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player && input.Buttons() == "Interact" && canInteract && pInteract.ReturnGrounded())
        {
            InteractedWith(other.gameObject);
        }
    }
    

    public virtual void InteractedWith(GameObject other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            interact.StartFadeOut();
        }
    }

}
