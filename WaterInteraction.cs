using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteraction : MonoBehaviour {

    SfxControl sfxControlScript;

    private void Awake()
    {
        sfxControlScript = GameObject.Find("SFXControl").GetComponent<SfxControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Avatar")
        {
            sfxControlScript.WaterIn();
            sfxControlScript.inWater = true;
        }

        if (other.name == "Main Camera")
        {
            other.gameObject.GetComponent<CameraEffects>().inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Avatar")
        {
            sfxControlScript.WaterOut();
            sfxControlScript.inWater = false;
        }

        if (other.name == "Main Camera")
        {
            other.gameObject.GetComponent<CameraEffects>().inWater = false;
        }
    }
}
