using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetMainScene", 0.2f);
        Debug.Log("점수 반영용 씬 이동");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainScene()
    {
        SceneManager.LoadScene("TestMainScene");
    }
}
