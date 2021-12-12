using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeMixer : MonoBehaviour
{
    public AudioMixer mixer;

    private void Start()
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("vol", 0.3f)) * 20);
        gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("vol", 0.3f);
    }

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("vol", sliderValue);
    }
}
