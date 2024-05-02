using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Configuraciones : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("VolumenAudio", 1f);
        AudioListener.volume = sliderValue;
    }

    public void ChangeSlider(float value)
    {
        slider.value = value;
        PlayerPrefs.SetFloat("VolumenAudio", sliderValue);
        AudioListener.volume = slider.value;

    }
}
