using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogueClock : MonoBehaviour {

    [Tooltip("Total time in seconds for an entire day. Refer to 5th case in 'DayNightCycle' script")]
    public float totalTime;

    float rotationSpeed;
    public float totalTimePassed;

    public DayNightCycle dayNightScript;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!dayNightScript.isTalking)
        {
            rotationSpeed = 360 / totalTime * Time.deltaTime;

            totalTimePassed += Time.deltaTime;
            if (totalTimePassed >= totalTime)
            {
                totalTimePassed = 0;
            }


            transform.Rotate(0, 0, -rotationSpeed, Space.Self);

            if (transform.rotation.z >= 360.01f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

	}
}
