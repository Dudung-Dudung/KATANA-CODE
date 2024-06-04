using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartSceneManager : MonoBehaviour
{
    public float startDelay;
    public Transform Player;
    public static StartSceneManager instance;

    GameObject Warning;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Warning = GameObject.Find("Warning");
    }

    private void Start()
    {
        Invoke("StartGame", startDelay);
    }

    private void StartGame()
    {
        Warning.GetComponent<Fade>().StartFade();
    }

    //�� �ε��
    public string nextScene;
    public void SetMainScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void SetMainScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }
}
