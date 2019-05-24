using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRingRotation : MonoBehaviour {

    public float rotationSpeed = 1.0f;

	void Update () {
        this.transform.Rotate(0, 0, rotationSpeed);
	}
}
