using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBGMovement : MonoBehaviour
{
     Vector3 endPosition;

    // Start is called before the first frame update
    void Awake()
    {
        endPosition = transform.position + Vector3.right * Random.Range(-150.0f, 150.0f);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //IEnumerator Move() 
    //{
    //    yield return new 
    //}
}
