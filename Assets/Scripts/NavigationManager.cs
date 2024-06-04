using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MusicManager;

public class NavigationManager : MonoBehaviour
{
    public RectTransform currentView;
    public AudioSource songAudio;

    // Start is called before the first frame update

    public void NavTabBarClick(RectTransform newview)
    {
        if(currentView != null)
        {
            currentView.gameObject.SetActive(false);
        }
        currentView = newview;
        currentView.gameObject.SetActive(true);
        if(songAudio.clip.name != "titanium-AlisiaBeats") {
            AudioClip musicClip = Resources.Load<AudioClip>("Audio/titanium-AlisiaBeats");
            songAudio.clip = musicClip;
            songAudio.Play();
        }
    }
}
