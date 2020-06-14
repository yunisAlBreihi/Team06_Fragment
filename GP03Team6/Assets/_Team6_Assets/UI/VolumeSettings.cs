using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Text VolumeText;
    [SerializeField] Image FillPic;
    public float GetCurrentVolume()
    {
        return VolumeSlider.value;
    }
    private void Update()
    {
        VolumeText.text = VolumeSlider.value.ToString() + "%";
       // FillPic.fillAmount = VolumeSlider.value / 100;
        //if (FillPic.fillAmount > 0.66f)
        //    FillPic.color = Color.green;
        //else if (FillPic.fillAmount > 0.33f)
        //    FillPic.color = Color.yellow;
        //else
        //    FillPic.color = Color.red;
    }
}
