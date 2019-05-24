using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMapFollow : MonoBehaviour {
    public Transform target;
    public Camera mainCam;

    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 250, 0);

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
