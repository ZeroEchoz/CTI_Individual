using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    [FMODUnity.EventRef]
    public string music;

    FMOD.Studio.EventInstance BGM_EV;


    public DayNightCycle dayNightScript;

	void Start () {
        BGM_EV = FMODUnity.RuntimeManager.CreateInstance(music);
        BGM_EV.start();
    }
	
	void LateUpdate () {
        ChangeBGM();
	}

    void ChangeBGM()
    {
        //change to morning theme + morning ambience
        //change to afternoon theme
        //change to evening theme
        //change to night theme

        switch (dayNightScript.currentTime)
        {
            case DayNightCycle.TimePeriod.Morning:
                BGM_EV.setParameterValue("time sections", 0);
                break;

            case DayNightCycle.TimePeriod.Afternoon:
                BGM_EV.setParameterValue("time sections", 1.1f);
                break;

            case DayNightCycle.TimePeriod.Evening:
                BGM_EV.setParameterValue("time sections", 2.1f);
                break;

            case DayNightCycle.TimePeriod.Night:
                //BGM.setParameterValue("time sections", 3.1f);
                break;
        }
    }

}
