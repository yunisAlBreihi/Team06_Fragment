using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private enum MusicToPlay
    {
        LVL1,
        LVL2,
        LVL3,
        NOTHING,
    }

    public List<string> NextToLoad = new List<string>();
    public List<string> PreviousToUnload = new List<string>();
    [SerializeField] private bool isFader = false;



    //#if (UNITY_STANDALONE && !UNITY_EDITOR)

    //[FMODUnity.EventRef]
    //[SerializeField] public string music;

    private void OnEnable()
    {
        if (isFader)
        {
            FadeVolume.OnHasFaded += OnUnloadScene;
        }
    }

    private void OnDisable()
    {
        if (isFader)
        {
            FadeVolume.OnHasFaded -= OnUnloadScene;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFader == false)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnUnloadScene();
            }
        }
    }

    void OnUnloadScene()
    {
        if (NextToLoad[0] != "")
        {
            for (int i = 0; i < NextToLoad.Count; i++)
            {
                ManagerForScenes.Instance.Load(NextToLoad[i]);
            }
        }

        if (PreviousToUnload[0] != "")
        {
            StartCoroutine(UnloadScene());
        }
    }

    private IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < PreviousToUnload.Count; i++)
        {
            ManagerForScenes.Instance.Unload(PreviousToUnload[i]);
        }

    }
    //#endif
}
