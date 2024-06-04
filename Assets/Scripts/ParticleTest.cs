using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    public ParticleSystem particle;
    
    void Start()
    {
        particle.gameObject.transform.position = this.gameObject.transform.position;
/*        particle.transform.localScale = transform.localScale;*/
        particle.Stop();
        particle.Play();
/*        ParticleSystem PS = particle.GetComponent<ParticleSystem>();
        PS.Stop(); PS.Play();
        if (!particle.isPlaying) particle.Play();
        if (particle.isPaused) particle.Play();*/
    }
}
