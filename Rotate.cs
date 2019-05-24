using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate {

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360f)
            angle += 360f;

        if (angle > 360f)
            angle -= 360f;

        return Mathf.Clamp(angle, min, max);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
