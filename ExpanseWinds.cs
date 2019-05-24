using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpanseWinds : MonoBehaviour {

    public GameObject player;
    public float pushForce;

    private void Start()
    {
        player = GameObject.Find("Avatar");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Avatar")
        {
            other.GetComponent<Rigidbody>().AddForce(player.transform.Find("MainGal").transform.forward * -1 * (pushForce));
            print("Hit");
        }
    }
}
