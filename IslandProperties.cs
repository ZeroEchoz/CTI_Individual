using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandProperties : MonoBehaviour {

    public GameObject IslandNameImage;
    public GameObject player;

    public float distanceToIsland;
    [Tooltip("The distance symbolised by the blue sphere of how close the player needs to be, to shows the island name")]
    public float entranceDistance;

    public void FixedUpdate()
    {
        distanceToIsland = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToIsland <= entranceDistance)
        {
            IslandNameImage.GetComponent<Animator>().SetBool("ShowName", true);
        }

        if (distanceToIsland > entranceDistance)
        {
            IslandNameImage.GetComponent<Animator>().SetBool("ShowName", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, entranceDistance);
    }

}
