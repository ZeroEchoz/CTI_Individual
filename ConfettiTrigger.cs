using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiTrigger : MonoBehaviour {

    public Transform confettiSpawnLocation;
    public GameObject confetti;
    GameObject particleClone;

    public void Start()
    {
        //spawn confetti at location
        Vector3 spawnLoc = new Vector3(confettiSpawnLocation.position.x, confettiSpawnLocation.position.y, confettiSpawnLocation.position.z);
        particleClone = Instantiate(confetti, spawnLoc, Quaternion.identity);
        particleClone.GetComponent<ParticleSystem>().Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //play confetti once when triggered
            particleClone.GetComponent<ParticleSystem>().Play();
            print("SHOOT");
        }

    }
}
