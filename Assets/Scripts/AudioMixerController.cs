using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField] private Slider masterAudioSlider;
    [SerializeField] private AudioSource masterAudioSource;
    public TextMeshProUGUI VolumeValue;


    private void Start()
    {
        Debug.Log(masterAudioSlider.value);
        masterAudioMixer.GetFloat("Master", out float volume);
        Debug.Log(" volume" + volume);
        masterAudioSlider.value = volume;
        SetMasterVolume();
    }
    public void SetMasterVolume()
    {
        float volume = masterAudioSlider.value;
        if (volume <= -15) volume = -80;
        masterAudioMixer.SetFloat("Master", volume);

        if (volume <= -80) volume = (int)0;
        else volume = (int)((volume + 15) * 4);


        VolumeValue.text = volume.ToString() + "%";

    }
}
