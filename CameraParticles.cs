using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParticles : MonoBehaviour {

    public ParticleSystem windParticles;
    public GameObject player;

    CharControl playerScript;

    public void Start()
    {
        playerScript = player.GetComponent<CharControl>();
    }

    void Update () {

        //if gliding, and pressing the sprint button, activate the wind particles, otherwise don't
        if (playerScript.isGliding
            && Input.GetKey(playerScript.sprintKey)
            && playerScript.hasFuel)
        {
            if (windParticles.isStopped)
            {
                windParticles.Play();
            }
        } else
        {
            if (windParticles.isPlaying)
            {
                windParticles.Stop();
            }
        }
	}
}
