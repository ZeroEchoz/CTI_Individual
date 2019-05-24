using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOOD_EventEmitterController : MonoBehaviour {

    public FMODUnity.StudioEventEmitter mainEmitter;
    public string parameterName = "TIME OF DAY";

    public float emitterValue;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        mainEmitter.SetParameter(parameterName, emitterValue);
	}
}
