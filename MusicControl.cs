using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour {

    [FMODUnity.EventRef]
    public string musicRef;

    FMOD.Studio.EventInstance musicEV;

    [Tooltip("Change this value to change BGM.  1 = Afternoon ; 2 = Evening ; 3 = Nght ; 4 = Morning")]
    [Range(1, 4)]
    public int musicValue;
    public string parameterName = "M-A-E-N";

    public DayNightCycle dayNightScript;

    void Start()
    {
        musicEV = FMODUnity.RuntimeManager.CreateInstance(musicRef);
        musicEV.start();
    }

    private void LateUpdate()
    {
        musicEV.setParameterValue(parameterName, musicValue);

        switch (dayNightScript.currentTime)
        {
            case DayNightCycle.TimePeriod.Morning:
                musicValue = 4;
                break;

            case DayNightCycle.TimePeriod.Afternoon:
                musicValue = 1;
                break;

            case DayNightCycle.TimePeriod.Evening:
                musicValue = 2;
                break;

            case DayNightCycle.TimePeriod.Night:
                musicValue = 3;
                break;
        }
    }

    private void OnDestroy()
    {
        musicEV.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

}
