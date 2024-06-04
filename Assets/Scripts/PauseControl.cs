using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseControl : MonoBehaviour
{
    public bool IsPaused { set; get; } = false;
    public AudioSource audioSource;

    public Text CountDownDisplay;
    private float timer = 3.0f;



    public void ClickedPause()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            Paused();
        }
        else
        {
            CountDown();
        }
    }
    void Paused()
    {
        Time.timeScale = 0.0f;
        audioSource.Pause();
    }

    void UnPaused()
    {
        Time.timeScale = 1.0f;
        audioSource.UnPause();
    }

    public void CountDown()
    {
        StartCoroutine(CountDownToStart(timer));
    }

    IEnumerator CountDownToStart(float time)
    {
        float count = Time.realtimeSinceStartup + time;
        
        while (count > Time.realtimeSinceStartup)
        {
            CountDownDisplay.text = ((int)(count - Time.realtimeSinceStartup)).ToString();
            yield return null;
        }
        CountDownDisplay.text = "";
        UnPaused();
        yield return null;
    }
}
