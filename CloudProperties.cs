using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudProperties : MonoBehaviour {

    public GameObject player;
    public Material normalCloud;
    public Material rainCloud;

    CloudMechanics cloudMechanicsScript;
    CharControl charControlScript;

    #region positionPoints
    [Tooltip("The central point of each island")]
    public GameObject[] islandCentralPoints;
    public Vector3 offset = new Vector3(0, 150, 0);
    [Tooltip("Speed of the cloud as it travels from island to island")]
    public float travelSpeed;

    public float distanceToTarget;
    private Vector3 currentTargetPos;
    private Vector3 nextTargetLocation;
    private int targetCount;
    #endregion

    private void Start()
    {
        player = GameObject.Find("Avatar");
        cloudMechanicsScript = player.GetComponent<CloudMechanics>();
        charControlScript = player.GetComponent<CharControl>();
    }

    private void FixedUpdate()
    {
        if (this.tag == "rainCloud")
        {
            RainCloudMechanics();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Avatar")
        {
            if (this.tag == "rainCloud")
            {
                cloudMechanicsScript.cloudParticles.GetComponent<ParticleSystemRenderer>().material = rainCloud;
                cloudMechanicsScript.fixedCloudParticles.GetComponent<ParticleSystemRenderer>().material = rainCloud;
            }
            else
            {
                cloudMechanicsScript.cloudParticles.GetComponent<ParticleSystemRenderer>().material = normalCloud;
                cloudMechanicsScript.fixedCloudParticles.GetComponent<ParticleSystemRenderer>().material = normalCloud;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Avatar")
        {
            cloudMechanicsScript.inCloud = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Avatar")
        {
            cloudMechanicsScript.inCloud = false;
        }
    }

    void RainCloudMechanics()
    {
        currentTargetPos = islandCentralPoints[targetCount].transform.position + offset;

        distanceToTarget = Vector3.Distance(transform.position, currentTargetPos);

        print(targetCount + "/" + islandCentralPoints.Length);

        //if the cloud is not on the island
        if (distanceToTarget > 1.0f)
        {
            //move it forward towards there
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPos, Time.deltaTime * travelSpeed);
            print("moving towards");
            
        } else if (distanceToTarget < 1.0f) //if it has reached the island
        {
            print("reached");
            //and if it hasn't reached the end of the island list
            if (targetCount < islandCentralPoints.Length - 1)
            {
                //continue on to the next island on the list
                targetCount++;
                print("next one");
            } else if (targetCount >= islandCentralPoints.Length - 1)
            {
                //otherwise, reset the count to start from the beginning
                targetCount = 0;
                print("Reset");
            }
        }
    }

}
