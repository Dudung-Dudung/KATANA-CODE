using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeStart : MonoBehaviour
{
    public GameObject[] Fades;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCutScene());
    }

    IEnumerator StartCutScene()
    {
        foreach (var f in Fades)
        {
            f.SetActive(true);
            f.GetComponent<Fade>().StartFade();
            yield return null;

            while (f.activeSelf)
            {
                yield return null;
            }
        }
    }

}
