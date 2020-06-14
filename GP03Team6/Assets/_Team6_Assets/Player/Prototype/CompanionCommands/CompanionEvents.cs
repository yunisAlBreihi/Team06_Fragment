using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionEvents : MonoBehaviour
{
    [SerializeField] private HighlightedObjectManager highlightedObjectManager;
    [SerializeField] private float distance = 50f;

    //Events
    public delegate void MoveCompanion(GameObject gameObject);
    public static event MoveCompanion OnMove;

    private Camera camera;

    private List<HighlighableRenderer> notToHighlighList = new List<HighlighableRenderer>();
    private HighlighableRenderer CurrentObjectToHighlight;
    private Vector3 screenCenter;

    private void Awake()
    {
        camera = Camera.main;
        screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    private void Update()
    {
        CurrentObjectToHighlight = highlightedObjectManager.GetClosestHighlightableToPoint(camera, screenCenter);

     
        if (CurrentObjectToHighlight != null)
        {
	        if (Vector3.Distance(CurrentObjectToHighlight.Highlightable.gameObject.transform.position, transform.position) < distance)
	        {
	
	            if (CurrentObjectToHighlight != null)
	            {
	                CurrentObjectToHighlight.MeshRenderer.material.SetInt("_OutlineToggle", 1);
	            }
	
	            notToHighlighList = highlightedObjectManager.GetAllOtherHighlightable(CurrentObjectToHighlight);
	
	            foreach (HighlighableRenderer notToHighlight in notToHighlighList)
	            {
	                notToHighlight.MeshRenderer.material.SetInt("_OutlineToggle", 0);
	            }
	        }
	        else
	        {
	            CurrentObjectToHighlight.MeshRenderer.material.SetInt("_OutlineToggle", 0);
	            CurrentObjectToHighlight = null;
	        }
        }

        if (CurrentObjectToHighlight != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnMove(CurrentObjectToHighlight.Highlightable.gameObject);
            }
        }
    }
}

