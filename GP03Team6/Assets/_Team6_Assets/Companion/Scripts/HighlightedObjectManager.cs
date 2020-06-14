using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighableRenderer
{
    private Highlightable highlightable;
    public Highlightable Highlightable { get => highlightable; set => highlightable = value; }

    private MeshRenderer meshRenderer;
    public MeshRenderer MeshRenderer { get => meshRenderer; set => meshRenderer = value; }
}

[CreateAssetMenu(fileName = "HighlightedObjectManager",menuName = "ScriptableObjects/HighlightedObjectManager", order = 1)]
public class HighlightedObjectManager : ScriptableObject 
{
    private List<HighlighableRenderer> highlighableRenderers = new List<HighlighableRenderer>();
    public List<HighlighableRenderer> HighlighableRenderers => highlighableRenderers;

    public HighlighableRenderer GetClosestHighlightableToPoint(Camera cam, Vector3 targetPosition)
    {
        HighlighableRenderer closestObject = null;
        Vector3 objScreenPos = new Vector3();
        Vector3 closestObjScreenPos = new Vector3();

        foreach (HighlighableRenderer obj in highlighableRenderers)
        {
            objScreenPos = cam.WorldToScreenPoint(obj.Highlightable.transform.position);

            if (closestObject != null)
            {
                closestObjScreenPos = cam.WorldToScreenPoint(closestObject.Highlightable.transform.position);
            }

            if (closestObject == null || Vector3.Distance(objScreenPos, targetPosition) < Vector3.Distance(closestObjScreenPos, targetPosition))
            {
                closestObject = obj;
            }
        }
        return closestObject;
    }

    public List<HighlighableRenderer> GetAllOtherHighlightable(HighlighableRenderer highlightable) 
    {
        List<HighlighableRenderer> tempHighlightables = new List<HighlighableRenderer>();

        foreach (HighlighableRenderer highlight in highlighableRenderers)
        {
            tempHighlightables.Add(highlight);
        }

        if (tempHighlightables.Contains(highlightable))
        {
            tempHighlightables.Remove(highlightable);
            return tempHighlightables;
        }
        return tempHighlightables;
    }
}