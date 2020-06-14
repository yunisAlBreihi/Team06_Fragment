using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    [SerializeField, Tooltip("The highlightable object manager ScriptableObject to use")]
    HighlightedObjectManager highlightedObjectManager;
    [SerializeField, Tooltip("Specify which renderer is used for the highlighting")]
    MeshRenderer rendererToHighlight;

    private HighlighableRenderer highlighableRenderer = new HighlighableRenderer();

    private void Awake()
    {
        highlighableRenderer.Highlightable = this;
        highlighableRenderer.MeshRenderer = rendererToHighlight;
    }

    private void OnBecameVisible()
    {
        enabled = true;
        highlightedObjectManager.HighlighableRenderers.Add(highlighableRenderer);
    }

    private void OnBecameInvisible()
    {
        highlightedObjectManager.HighlighableRenderers.Remove(highlighableRenderer);
        enabled = false;
    }
}
