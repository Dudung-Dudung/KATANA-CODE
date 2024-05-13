using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeStart : MonoBehaviour
{
    private Fade fade;
    void Awake()
    {
        fade = this.GetComponent<Fade>();
    }
    // Start is called before the first frame update
    void Start()
    {
        fade.StartFade();
    }
}
