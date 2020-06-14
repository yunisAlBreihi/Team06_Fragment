using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private List<RectTransform> myButtons = new List<RectTransform>();
    [SerializeField] private RectTransform pointer;
    private Vector3 newPosY = Vector3.zero;
    
    public void SetPointerPosition(RectTransform button)
    {
        newPosY = pointer.position;
        newPosY = new Vector3(newPosY.x,button.position.y,newPosY.z);
        pointer.position = newPosY;
        
    }
    public void SetPointerActive()
    {
       // if (pointer.gameObject.activeSelf)
       // {
       //     pointer.gameObject.SetActive(false);
       // }
       // else
       // {
       //     pointer.gameObject.SetActive(true);
       // }
    }

}
