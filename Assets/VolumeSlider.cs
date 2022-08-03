using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum SliderType
    {
        Music,
        SFX
    };
    [SerializeField] private SliderType sliderType;

    [SerializeField] private Slider slider;
    [SerializeField] private float defaultValue = 0.5f;
    public void OnEnable()
    {
        if(sliderType == SliderType.Music)
            slider.value = PlayerPrefs.HasKey("MusicVolume")? PlayerPrefs.GetFloat("MusicVolume") : defaultValue;
        else
            slider.value = PlayerPrefs.HasKey("SFXVolume")? PlayerPrefs.GetFloat("SFXVolume") : defaultValue;
    }
    public void SetVolume(/*string busPath, */float value)
    {
        if (sliderType == SliderType.Music)
            PlayerPrefs.SetFloat("MusicVolume", value);
        else
            PlayerPrefs.SetFloat("SFXVolume", value);

        UIManager.SetBusVolume(sliderType, value);
    }
}
