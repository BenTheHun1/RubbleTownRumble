using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensitivity : MonoBehaviour
{
    private CameraController cm;

    // Start is called before the first frame update
    void Start()
    {
        cm = GameObject.Find("Main Camera").GetComponent<CameraController>();
        cm.mouseSensitivity = PlayerPrefs.GetFloat("sens", 400f);
        gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sens", 400f);
    }

    public void SetSens (float sliderVal)
    {
        cm.mouseSensitivity = sliderVal;
        PlayerPrefs.SetFloat("sens", cm.mouseSensitivity);
    }
}
