using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxControl : MonoBehaviour {

    public GameObject player;

    #region footstepsSFX
    [FMODUnity.EventRef]
    public string footstepsRef;

    FMOD.Studio.EventInstance footstepsEV;

    #region Footsteps Values
    public float muteSteps;

    public float grassSteps;
    public float grassStepsSprint;

    public float hardSteps;
    public float hardStepsSprint;

    public float waterSteps;
    public float waterStepsSprint;
    #endregion

    [Space(10)]
    #endregion

    #region GliderSFX
    [FMODUnity.EventRef]
    public string gliderWindRef;
    [FMODUnity.EventRef]
    public string gliderOpenRef;
    [FMODUnity.EventRef]
    public string gliderCloseRef;
    [FMODUnity.EventRef]
    public string rockRingBoostRef;

    FMOD.Studio.EventInstance gliderWindEV;

    [Space(10)]
    #endregion

    #region InventorySFX
    [FMODUnity.EventRef]
    public string inventoryOpenRef;
    [FMODUnity.EventRef]
    public string inventoryCloseRef;
    #endregion

    #region ButtonSFX
    [FMODUnity.EventRef]
    public string buttonInRef;
    [FMODUnity.EventRef]
    public string buttonOutRef;

    [Space(10)]
    #endregion

    #region PickupsSFX
    [FMODUnity.EventRef]
    public string pickupPlantRef;
    [FMODUnity.EventRef]
    public string pickupGemRef;
    [FMODUnity.EventRef]
    public string pickupOreRef;
    #endregion

    #region WaterSFX
    [FMODUnity.EventRef]
    public string waterInSFXRef;
    [FMODUnity.EventRef]
    public string waterOutSFXRef;
    #endregion

    #region UISFX
    [FMODUnity.EventRef]
    public string MS_NotificationRef;
    [FMODUnity.EventRef]
    public string EG_NotificationRef;
    #endregion


    public string footstepsParameterName = "FOOTSTEPS MATERIAL";
    public string gliderWindParameterName = "SPEED GLIDER";

    public string currentMaterial;

    bool playerMoving;
    public bool inWater;

    private void Start()
    {
        footstepsEV = FMODUnity.RuntimeManager.CreateInstance(footstepsRef);
        footstepsEV.start();
        footstepsEV.setParameterValue(footstepsParameterName, muteSteps);

        gliderWindEV = FMODUnity.RuntimeManager.CreateInstance(gliderWindRef);
        gliderWindEV.start();
    }

    private void FixedUpdate()
    {
        gliderWindEV.setParameterValue(gliderWindParameterName, player.GetComponent<CharControl>().windSfxParameter);
        currentMaterial = player.GetComponent<CharControl>().currentWalkingMaterial;
    }

    private void OnDestroy()
    {
        gliderWindEV.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void Update()
    {
        #region Check Player Movement
        //detect if player is moving
        if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f
            || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
            && player.GetComponent<CharControl>().isGrounded
            && !player.GetComponent<CharControl>().isGliding)
        {
            playerMoving = true;
        }
        else
        {
            playerMoving = false;
        }
        #endregion

        if (playerMoving)
        {
            Footsteps();
        } else
        {
            //mute steps if player is not moving
            footstepsEV.setParameterValue(footstepsParameterName, muteSteps);
        }


        /*
        //when not gliding
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!player.GetComponent<CharControl>().isGrounded)
            {

                switch (player.GetComponent<CharControl>().isGliding)
                {
                    case true:
                        gliderWindEV.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                        FMODUnity.RuntimeManager.PlayOneShot(gliderCloseRef, transform.position);
                        print("noGlideSFX");
                        break;
                    case false:
                        FMODUnity.RuntimeManager.PlayOneShot(gliderOpenRef, transform.position);
                        print("glideSFX");
                        break;
                }
            }
        } */

    }

    public void Footsteps()
    {
        if (!inWater)
        {
            #region Grass
            if (currentMaterial == "Island")
            {
                footstepsEV.setParameterValue(footstepsParameterName, grassSteps);
                if (Input.GetButton("Sprint"))
                {
                    footstepsEV.setParameterValue(footstepsParameterName, grassStepsSprint);
                }
            }
            #endregion
            #region Hard Surface
            if (currentMaterial != "Island"
                && currentMaterial != "water")
            {
                footstepsEV.setParameterValue(footstepsParameterName, hardSteps);
                if (Input.GetButton("Sprint"))
                {
                    footstepsEV.setParameterValue(footstepsParameterName, hardStepsSprint);
                }
            }
            #endregion
        } else if (inWater)
        {
            #region Water
            footstepsEV.setParameterValue(footstepsParameterName, waterSteps);
            if (Input.GetButton("Sprint"))
            {
                footstepsEV.setParameterValue(footstepsParameterName, waterStepsSprint);
            }
            #endregion
        }


    }

    public void InventoryOpen()
    {
        FMODUnity.RuntimeManager.PlayOneShot(inventoryOpenRef);
    }

    public void InventoryClose()
    {
        FMODUnity.RuntimeManager.PlayOneShot(inventoryCloseRef);
    }

    public void ButtonIn()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonInRef);
    }

    public void ButtonOut()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonOutRef);
    }

    public void RockRingBoostSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(rockRingBoostRef);
    }

    public void PickUpPlantSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickupPlantRef);
    }

    public void PickUpGemSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickupGemRef);
    }

    public void PickUpOreSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickupOreRef);
    }

    public void WaterIn()
    {
        FMODUnity.RuntimeManager.PlayOneShot(waterInSFXRef);
    }

    public void WaterOut()
    {
        FMODUnity.RuntimeManager.PlayOneShot(waterOutSFXRef);
    }

    public void MilestoneNotifySFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(MS_NotificationRef);
    }

    public void EasterEggNotifySFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(EG_NotificationRef);
    }

    public void GliderOpen()
    {
        FMODUnity.RuntimeManager.PlayOneShot(gliderOpenRef);
    }
    
    public void GliderClose()
    {
        FMODUnity.RuntimeManager.PlayOneShot(gliderCloseRef);
    }
}
