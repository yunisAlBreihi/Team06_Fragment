using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOjnar : MonoBehaviour
{
    [SerializeField,Tooltip("time in seconds on which ojnar fades.")] float fadeTime = 2.0f;

    List<Color> originalColors = new List<Color>();
    Material[] myMaterials;

    public void StartFade()
    {
        myMaterials = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().materials;

        //foreach (var item in myMaterials)
        //{
        //    originalColors.Add(item.GetColor("_BaseColor"));
        //}
        StartCoroutine(Fade());
    }

    IEnumerator Fade() 
    {
        float fadeDelta = 0;
        List<Color> endColors = new List<Color>();

        //foreach (var item in myMaterials)
        //{
        //    endColors.Add(new Color(item.GetColor("_BaseColor").r, item.GetColor("_BaseColor").g, item.GetColor("_BaseColor").b, 0.0f));
        //}

        while (fadeDelta <=1.0f)
        {
            //for (int i = 0; i < myMaterials.Length; i++)
            //{
            //    myMaterials[i].SetColor("_BaseColor", Color.Lerp(originalColors[i], endColors[i], fadeDelta));
            //}
            fadeDelta += Time.deltaTime / fadeTime;

            yield return new WaitForSeconds(1.0f / 30.0f);
        }

        gameObject.SetActive(false);
    }
}