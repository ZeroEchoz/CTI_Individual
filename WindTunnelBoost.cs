using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTunnelBoost : MonoBehaviour {

    public GameObject player;
    public ParticleSystem boostBurstParticles;
    public float boostForce;

    CharControl playerScript;
    Rigidbody playerRb;


	void Start () {
        player = GameObject.Find("Avatar");
        playerScript = player.GetComponent<CharControl>();
        playerRb = player.GetComponent<Rigidbody>();
	}
	
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Avatar")
        {
            WindBoost();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!boostBurstParticles.isPlaying)
        {
            boostBurstParticles.Play();
            player.GetComponent<CharControl>().sfxControlScript.RockRingBoostSFX();
        }
    }

    public void WindBoost()
    {
        playerRb.AddForce(player.transform.Find("MainGal").transform.forward * boostForce);
        print("WINDBOOST");
    }
}
