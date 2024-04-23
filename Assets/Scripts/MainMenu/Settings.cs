using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    private bool isFullScreen;
    [SerializeField] private AudioMixer am;
    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
    public void AudioVolume(float sliderValue)
    {
        am.SetFloat("masterVolume", sliderValue);
    }

    public void ChangeSens(float sliderValue)
    {
        Debug.Log("меняем чувствительность");
        PlayerPrefs.SetFloat("sens", sliderValue);

    }
}
