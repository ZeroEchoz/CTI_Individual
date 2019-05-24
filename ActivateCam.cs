using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCam : MonoBehaviour {

    public GameObject cineCamObj;

    public void ActivateFreelookCam()
    {
        cineCamObj.SetActive(true);
    }
}
