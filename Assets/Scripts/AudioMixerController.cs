using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer masterAudioMixer;
    [SerializeField] private Slider masterAudioSlider;
    [SerializeField] private AudioSource masterAudioSource;
    public TextMeshProUGUI VolumeValue;

  
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
