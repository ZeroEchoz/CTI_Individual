using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour {

    [HideInInspector]
    public bool inWater;

    public PostProcessProfile normalProfile;
    public PostProcessProfile waterProfile;

    public PostProcessVolume PP_Volume;

    private void Start()
    {
        PP_Volume.profile = normalProfile;
        inWater = false;
    }

    public void Update()
    {
        if (inWater)
        {
            WaterCam();
        } else if (!inWater)
        {
            NormalCam();
        }
    }

    public void WaterCam()
    {
        PP_Volume.profile = waterProfile;
    }

    public void NormalCam()
    {
        PP_Volume.profile = normalProfile;
    }
}
