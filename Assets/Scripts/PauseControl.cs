using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public bool IsPaused { set; get; } = false;
    public AudioClip[] Clips;
    public void ClickedPause()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            Paused();
        }
        else
        {
            UnPaused();
        }
    }
    void Paused()
    {
        Time.timeScale = 0.0f;

    }

    void UnPaused()
    {
        Time.timeScale = 1.0f;
    }
}
