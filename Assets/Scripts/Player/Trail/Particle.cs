using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (Player.Info.isDashing == true)
        {
            particle.Play();
        }
        else
        {
            particle.Stop();
        }
        transform.localScale = new Vector3((int)Player.Info.currentDirection, 1, transform.localScale.z);
    }
}
