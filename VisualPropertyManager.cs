using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPropertyManager : MonoBehaviour {

    [Tooltip("The density of normal fog. Refer to the fog density in Render Settings")]
    public float normalFogDensity;
    [Tooltip("The density of thick fog. Refer to the fog density in Render Settings.")]
    public float thickFogDensity;
    [Tooltip("The lower the slower, the higher the faster")]
    public float transitionSpeed;

    GameObject player;
    float distance;
    public float entranceDistance;

    bool inFoggyArea;

    private void Start()
    {
        player = GameObject.Find("Avatar");
        RenderSettings.fogDensity = normalFogDensity;
    }

    private void Update()
    {
        if (inFoggyArea)
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, thickFogDensity, Time.deltaTime * transitionSpeed);
        } else
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, normalFogDensity, Time.deltaTime * transitionSpeed);
        }

    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= entranceDistance)
        {
            inFoggyArea = true;
        }

        if(distance > entranceDistance)
        {
            inFoggyArea = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, entranceDistance);
    }
}
