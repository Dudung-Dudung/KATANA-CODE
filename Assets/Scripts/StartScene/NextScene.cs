using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    public string scneName = "TestMainScene";
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(scneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
