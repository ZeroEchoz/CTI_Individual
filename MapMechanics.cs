using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMechanics : MonoBehaviour {

    float cameraZoom;
    public Camera mapCam;
    public float defaultZoom = 590f;
    [Range(5, 20)]
    public float zoomMultiplier = 10;
    public float minZoom = 10f;
    public float maxZoom = 1500f;

    void Start () {
        cameraZoom = defaultZoom;
	}

	void Update () {
        MapZoom();
	}

    private void LateUpdate()
    {
        MapMove();
    }

    public void MapZoom()
    {
        // float zoomVal = 1;
        //Debug.Log(zoomVal += Input.mouseScrollDelta.y);
        mapCam.orthographicSize = cameraZoom;
        cameraZoom = Mathf.Clamp(cameraZoom, minZoom, maxZoom);
        cameraZoom += Input.mouseScrollDelta.y * zoomMultiplier;
        print(cameraZoom);
    }

    public void MapMove()
    {

    }

}

