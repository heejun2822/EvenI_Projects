using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] bool isDustOnWalk = true;
    [SerializeField] ParticleSystem dustParticleSystem;

    public void CreateDustParticle()
    {
        if (isDustOnWalk)
        {
            dustParticleSystem.Stop();
            dustParticleSystem.Play();
        }
    }

}
