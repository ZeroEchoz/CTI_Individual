using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAmbience : MonoBehaviour {

    public AnalogueClock clockScript;
    public FMOOD_EventEmitterController emitterController;

    public float currentTimeOfDay;

	// Update is called once per frame
	void Update () {
        currentTimeOfDay = (clockScript.totalTimePassed / clockScript.totalTime) * 4;
        emitterController.emitterValue = currentTimeOfDay;

    }
}
