using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowScript : MonoBehaviour {

    public Transform target;
    [Tooltip("The camera that's displaying the full map")]
    public Camera mapCam;
    [Tooltip("The main camera")]
    public Camera mainCam;
    [Tooltip("The Canvas that this UI is inside of")]
    public Canvas canvas;

    RectTransform thisRect;
    RectTransform canvasRect;

    void Start () {
        thisRect = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }
	
	void FixedUpdate () {
        Vector2 viewportPoint = mapCam.WorldToViewportPoint(target.position);

        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((viewportPoint.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((viewportPoint.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        thisRect.anchoredPosition = WorldObject_ScreenPosition;

        transform.eulerAngles = new Vector3(0, 0, -mainCam.transform.eulerAngles.y - 150);
    }
}
