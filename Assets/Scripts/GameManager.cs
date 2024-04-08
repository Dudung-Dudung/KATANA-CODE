using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int songTitle;
    public static GameManager instance;

    //public ParticleSystem effectPrefab;

   // public AudioClip effectAudio;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

   
    public void GameQuit()
    {
        Application.Quit();
    }

}
