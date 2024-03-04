using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGagueManager : MonoBehaviour
{
    public GameObject[] CircleGagues;

    public float delayTime;

    public GameObject MainGague;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnGague()
    {
        foreach(GameObject CIrcleGague in CircleGagues)
        {
            CIrcleGague.SetActive(true);
            yield return new WaitForSeconds(Random.Range(1.0f, delayTime));
        }
        yield return null;
    }    
}
