using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour {

    public ParticleSystem[] windTunnels;
    public ParticleSystem[] fireflies;
    public GameObject[] streetLights;

    public Color windTunnelNightColor;
    public Color windTunnelDayColor;

    Color windParticleColor;

    public bool nightTime;

	void Start () {
        foreach (ParticleSystem firefly in fireflies)
        {
            firefly.Stop();
        }
	}
	
	void LateUpdate () {

        AdjustWindTunnels();

        if (nightTime)
        {
            EnableFireflies();
            EnableStreetLights();
        } else
        {
            DisableFireflies();
            DisableStreetLights();
        }
	}

    public void EnableFireflies()
    {
        for (int i = 0; i < fireflies.Length; i++)
        {
            if (!fireflies[i].isPlaying)
            {
                fireflies[i].Play();
            }
        }
    }

    public void DisableFireflies()
    {
        for (int i = 0; i < fireflies.Length; i++)
        {
            if (fireflies[i].isPlaying)
            {
                fireflies[i].Stop();
            }
        }
    }

    public void EnableStreetLights()
    {
        for (int i = 0; i < streetLights.Length; i++)
        {
            if (!streetLights[i].activeSelf)
            {
                streetLights[i].SetActive(true);
            }
        }
    }

    public void DisableStreetLights()
    {
        for (int i = 0; i < streetLights.Length; i++)
        {
            if (streetLights[i].activeSelf)
            {
                streetLights[i].SetActive(false);
            }
        }
    }

    public void AdjustWindTunnels()
    {
        //adjust to night time
        if (nightTime)
        {
            for (int i = 0; i < windTunnels.Length; i++)
            {
                ParticleSystem.MainModule settings = windTunnels[i].main;
                settings.startColor = windTunnelNightColor;
            }
        } else
        {
            for (int i = 0; i < windTunnels.Length; i++)
            {
                ParticleSystem.MainModule settings = windTunnels[i].main;
                settings.startColor = windTunnelDayColor;
            }
        }
    }
}
