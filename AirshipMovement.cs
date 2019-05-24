using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipMovement : MonoBehaviour {

    public GameObject airshipModel;
    public Transform arrivalPoint;
    public GameObject currentDock;
    public GameObject previousDock;
    public GameObject[] docks;

    public bool docked = false;
    public bool elevated = false;
    [Tooltip("How long (in seconds) the airship waits when docked before setting off")]
    public float waitTime = 10f;
    [Tooltip("Speed at which the airship travels")]
    public float airshipSpeed = 20.0f;
    [Tooltip("How fast the airship move down to dock, and moves up before setting off")]
    public float airshipVerticalSpeed = 2.0f;
    [Tooltip("How high the airship has to be above the dock before setting off towards the next dock")]
    public float elevationHeight = 10.0f;
    [Tooltip("How high up the airship will be when it reaches the next dock")]
    public float aboveDockHeight = 40.0f;

    public float distanceX;
    private int dockCount;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        currentDock = docks[0].gameObject;
        previousDock = docks[docks.Length - 1].gameObject;
        airshipModel = transform.GetChild(0).gameObject;
    }
    void Start () {
        dockCount = 0;
	}

    void Update()
    {
        //get arrival marker through current dock
        arrivalPoint = currentDock.transform.GetChild(0);

        distanceX = Mathf.Abs(arrivalPoint.position.x - transform.position.x);
        //print(distanceX);
        var arrivalAngle = Quaternion.Euler(0, currentDock.transform.localEulerAngles.y, 0);

        //rotate the ship to be perpendicular to the dock
        if (distanceX <= 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, arrivalAngle, 0.5f * Time.deltaTime);
        }

        //lower the ship down to match dock height
        //ship has arrived
        if (transform.position.y >= arrivalPoint.position.y - 10.0f
             && distanceX < 0.01f)
        {

            transform.Translate(Vector3.down * (airshipVerticalSpeed * Time.deltaTime));

            if (transform.position.y <= arrivalPoint.position.y - 10.0f
                && distanceX < 0.01f)
            {
                docked = true;
                print("docked");
                Invoke("SwitchDock", waitTime);
            }
        }
        else if (distanceX >= 0.01f) //if moving towards a dock....
        {
            //elevate airship before departing
            if (transform.position.y <= previousDock.transform.GetChild(0).transform.position.y + elevationHeight
                && !elevated)
            {
                transform.Translate(Vector3.up * (airshipVerticalSpeed * Time.deltaTime));

                //check if airship is elevated enough
                if (transform.position.y >= previousDock.transform.GetChild(0).transform.position.y + elevationHeight)
                {
                    elevated = true;
                }
            }

            //if elevated enough, move towards dock
            if (elevated)
            {
                Vector3 aboveDockPos = new Vector3(arrivalPoint.position.x, arrivalPoint.position.y + aboveDockHeight, arrivalPoint.position.z);
                //move towards the arrival point, aligning the center of the object with the center of the arrival point
                transform.position = Vector3.SmoothDamp(transform.position, aboveDockPos, ref velocity, 1f, airshipSpeed);
               //transform.position = Vector3.MoveTowards(transform.position, arrivalPoint.position, airshipSpeed * Time.deltaTime);
            }

            if (!docked)
            {
                //rotate to always face towards the direction of the dock its going towards unless it is close enough to rotate perpendicularly
                var lookPos = arrivalPoint.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 0.5f);
            }

        }


        //loop between docks
        switch (dockCount)
        {
            default:
                currentDock = docks[dockCount].gameObject;
                break;
        }
        //set what a previous dock is
        switch (dockCount)
        {
            default:
                if (dockCount > 0)
                {
                    previousDock = docks[dockCount - 1].gameObject;
                } else if (dockCount == 0)
                {
                    previousDock = docks[docks.Length - 1].gameObject;
                }
                break;
        }
    }

    public void SwitchDock()
    {
        if (dockCount < docks.Length)
        {
            docked = false;
            elevated = false;
            print("current count: " + dockCount + " / " + docks.Length);
            dockCount++;
        }
        //reset dock count to 1 if it is ever greater than the array of docks for this airship
        if (dockCount >= docks.Length)
        {
            dockCount = 0;
        }
    }
    public void Accelerate()
    {

    }
}
