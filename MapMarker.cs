using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarker : MonoBehaviour {

    [Tooltip("What the marker is following")]
    public Transform target;
    public Vector2 positionOffset;
    [Tooltip("The camera that's displaying the full map")]
    public Camera mapCam;
    [Tooltip("The Canvas that this UI is inside of")]
    public Canvas canvas;

    RectTransform thisRect;
    RectTransform canvasRect;

	void Start () {
        thisRect = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
	}
	
	void Update () {
        Vector2 viewportPoint = mapCam.WorldToViewportPoint(target.position);

        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((viewportPoint.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((viewportPoint.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        thisRect.anchoredPosition = WorldObject_ScreenPosition + positionOffset;
    }
}
