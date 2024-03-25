using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTIme : MonoBehaviour
{
    public AudioSource audioSource;
    private float startTime;
    private float nowTime;
    private int count = 0;
    private bool IsStart = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying && !IsStart)
        {
            startTime = Time.realtimeSinceStartup;
            IsStart = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        nowTime = Time.realtimeSinceStartup - startTime; 
        if(other.CompareTag("Stick_Red") || other.CompareTag("Stick_Blue"))
        {
            if (IsStart)
            {
                Debug.Log(count + " " + nowTime);
                count++;
            }
            
        }
    }
}
