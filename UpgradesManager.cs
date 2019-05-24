using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour {

    [Tooltip("Choose what you want this gameobject to upgrade")]
    public PlayerParameters upgradableParameters;
    [Tooltip("How much you want the parameter upgraded by. **PLEASE REFER TO THE AVATAR'S DEFAULT PARAMETER VALUES BEFORE CHANGING THIS**")]
    [HideInInspector]
    public float upgradeValue;
    [HideInInspector]
    [Tooltip("The object that will be activated")]
    public GameObject objectToActivate;
    [Tooltip("Describe what levelling this up will activate/spawn")]
    [HideInInspector]
    public string objectActivateDescription;

    [Tooltip("The highest level possible for this upgrade.")]
    public int maxLevel;

    [Tooltip("Total CP Needed to reach the next level")]
    public int requiredCP;

    //private variables
    private GameObject player;
    private int totalInvestedCP;
    private float totalUpgradeValue;

    private int currentCP;
    private Text upgradeName_Txt;
    private Text CP_CostText;
    private Text currentCP_Txt;
    private Text currentLevel_Txt;
    private Text description_Txt;
    private Text max_Txt;

    private Image progressionFill;
    private Button resetButton;

    //how much CP has been put into this
    private int investedCP;
    //how much CP is left before upgrade is successful
    private int currentNeededCP;
    //current level of upgrade
    int currentLevel = 0;

    //when resetting turn this on to avoid accidental over-spending of CP
    bool resetting = false;

    CharControl charControlScript;

    public enum PlayerParameters
    {
        WalkSpeed, SprintSpeed, JumpVelocity, GliderSpeed, GliderUpForce, GliderBankSpeed, FuelCapacity, BoostForce, FuelRefilling, FuelUsage, ObjectActivation
    }

	void Awake () {
        //object setup
        player = GameObject.Find("Avatar");
        progressionFill = transform.Find("ProgressBar").transform.Find("ProgressBarBacking").transform.Find("ProgressFill").GetComponent<Image>();
        upgradeName_Txt = transform.Find("UpgradeName").GetComponent<Text>();
        CP_CostText = transform.Find("AddLevel").transform.Find("CP Cost").transform.Find("CP Number").GetComponent<Text>();
        currentLevel_Txt = transform.Find("CurrentLevelBG").transform.Find("CurrentLevel").GetComponent<Text>();
        description_Txt = transform.Find("Description").transform.Find("DescriptionText").GetComponent<Text>();
        currentCP_Txt = GameObject.Find("PersonalStats").transform.Find("CurrentCP").GetComponent<Text>();
        resetButton = transform.Find("ResetButton").GetComponent<Button>();
        max_Txt = transform.Find("ProgressBar").transform.Find("MAX").GetComponent<Text>();

        charControlScript = player.GetComponent<CharControl>();
        upgradeName_Txt.text = gameObject.name;
        currentNeededCP = requiredCP;
    }

    private void Start()
    {
        #region Upgrade Switches
        switch (upgradableParameters)
        {
            case PlayerParameters.WalkSpeed:
                description_Txt.text = "Starting: " + charControlScript.defaultSpeed;
                break;
            case PlayerParameters.SprintSpeed:
                description_Txt.text = "Starting: " + charControlScript.sprintSpeed;
                break;
            case PlayerParameters.JumpVelocity:
                description_Txt.text = "Starting: " + charControlScript.jumpVelocity;
                break;
            case PlayerParameters.GliderSpeed:
                description_Txt.text = "Starting: " + charControlScript.defaultGliderSpeed;
                break;
            case PlayerParameters.GliderUpForce:
                description_Txt.text = "Starting: " + charControlScript.defaultUpwardsForce;
                break;
            case PlayerParameters.GliderBankSpeed:
                description_Txt.text = "Starting: " + charControlScript.defaultBankSpeed;
                break;
            case PlayerParameters.FuelCapacity:
                description_Txt.text = "Starting: " + charControlScript.fuelCapacity;
                break;
            case PlayerParameters.BoostForce:
                description_Txt.text = "Starting: " + charControlScript.boostForce;
                break;
            case PlayerParameters.FuelRefilling:
                description_Txt.text = "Starting: " + charControlScript.fuelRefill;
                break;
            case PlayerParameters.FuelUsage:
                description_Txt.text = "Starting: " + charControlScript.fuelUsage;
                break;
            case PlayerParameters.ObjectActivation:
                objectToActivate.SetActive(false);
                description_Txt.text = objectActivateDescription;
                break;

        }
        #endregion
    }

    void Update () {
        #region Always Running
        //METHODS
        ProgressionMechanic();
        //OTHER
        currentCP = player.GetComponent<MilestoneRankManager>().TotalCP;
        currentCP_Txt.text = currentCP + " CP";
        currentLevel_Txt.text = currentLevel + "/" + maxLevel;
        if (currentLevel == maxLevel)
        {
            max_Txt.text = "MAX LEVEL";
            currentNeededCP = 0;
            if (upgradableParameters == PlayerParameters.ObjectActivation)
            {
                //Hide button
                resetButton.enabled = false;
                resetButton.gameObject.SetActive(false);
            }
        } else
        {
            max_Txt.text = "";
        }
        #endregion
    }

    void ProgressionMechanic()
    {
        //SETUP
        float progressionPercentage = (float)investedCP / (float)requiredCP;
        //print(progressionPercentage);
        if (currentLevel < maxLevel)
        {
            progressionFill.fillAmount = Mathf.Lerp(progressionFill.fillAmount, progressionPercentage, 0.2f);
            //PROGRESS TO NEXT RANK
            if (progressionPercentage == 1
                && !resetting)
            {
                resetting = true;
                Invoke("ResetProgression", 0.5f);
            }
        } else if (currentLevel == maxLevel)
        {
            progressionFill.fillAmount = 1f;
        }
        CP_CostText.text = currentNeededCP + " CP";

    }

    public void AddLevel()
    {
        if (currentLevel < maxLevel)
        {
            if (currentCP > 0
                && requiredCP > 0
                && !resetting)
            {
                charControlScript.GetComponent<MilestoneRankManager>().TotalCP--;
                currentNeededCP--;
                investedCP++;
                totalInvestedCP++;
            }
        }
        else
        {
            currentNeededCP = 0;
        }


    }

    public void ResetProgression()
    {
        currentLevel++;
        #region Upgrading Switches
        switch (upgradableParameters)
        {
            case PlayerParameters.WalkSpeed:
                UpgradeWalkSpeed();
                break;
            case PlayerParameters.SprintSpeed:
                UpgradeSprintSpeed();
                break;
            case PlayerParameters.JumpVelocity:
                UpgradeJumpVelocity();
                break;
            case PlayerParameters.GliderSpeed:
                UpgradeGliderSpeed();
                break;
            case PlayerParameters.GliderUpForce:
                UpgradeGliderUpForce();
                break;
            case PlayerParameters.GliderBankSpeed:
                UpgradeGliderBankSpeed();
                break;
            case PlayerParameters.FuelCapacity:
                UpgradeFuelCapacity();
                break;
            case PlayerParameters.BoostForce:
                UpgradeBoostForce();
                break;
            case PlayerParameters.FuelRefilling:
                UpgradeFuelRefill();
                break;
            case PlayerParameters.FuelUsage:
                UpgradeFuelUsage();
                break;
            case PlayerParameters.ObjectActivation:
                ActivateObject();
                break;

        }
        #endregion
        if (currentLevel != maxLevel)
        {
            currentNeededCP = requiredCP;
        }
        investedCP = 0;
        totalUpgradeValue += upgradeValue;
        resetting = false;
    }

    #region All Upgrade Functions
    public void UpgradeWalkSpeed()
    {
        charControlScript.defaultSpeed += upgradeValue;
        description_Txt.text = (charControlScript.defaultSpeed - upgradeValue) + " Increased To: " + charControlScript.defaultSpeed;
    }
    public void UpgradeSprintSpeed()
    {
        charControlScript.sprintSpeed += upgradeValue;
        description_Txt.text = (charControlScript.sprintSpeed - upgradeValue) + " Increased To: " + charControlScript.sprintSpeed;
    }
    public void UpgradeJumpVelocity()
    {
        charControlScript.jumpVelocity += upgradeValue;
        description_Txt.text = (charControlScript.jumpVelocity - upgradeValue) + " Increased To: " + charControlScript.jumpVelocity;
    }
    public void UpgradeGliderSpeed()
    {
        charControlScript.defaultGliderSpeed += upgradeValue;
        description_Txt.text = (charControlScript.defaultGliderSpeed - upgradeValue) + " Increased To: " + charControlScript.defaultGliderSpeed;
    }
    public void UpgradeGliderUpForce()
    {
        charControlScript.defaultUpwardsForce += upgradeValue;
        description_Txt.text = (charControlScript.defaultUpwardsForce - upgradeValue) + " Increased To: " + charControlScript.defaultUpwardsForce;
    }
    public void UpgradeGliderBankSpeed()
    {
        charControlScript.defaultBankSpeed += upgradeValue;
        description_Txt.text = (charControlScript.defaultBankSpeed - upgradeValue) + " Increased To: " + charControlScript.defaultBankSpeed;
    }
    public void UpgradeFuelCapacity()
    {
        charControlScript.fuelCapacity += upgradeValue;
        description_Txt.text = (charControlScript.fuelCapacity - upgradeValue) + " Increased To: " + charControlScript.fuelCapacity;
    }
    public void UpgradeBoostForce()
    {
        charControlScript.boostForce += upgradeValue;
        description_Txt.text = (charControlScript.boostForce - upgradeValue) + " Increased To: " + charControlScript.boostForce;
    }
    public void UpgradeFuelRefill ()
    {
        charControlScript.fuelRefill += upgradeValue;
        description_Txt.text = (charControlScript.fuelRefill - upgradeValue) + " Increased To: " + charControlScript.fuelRefill;
    }
    public void UpgradeFuelUsage()
    {
        charControlScript.defaultSpeed -= upgradeValue;
        description_Txt.text = (charControlScript.fuelUsage - upgradeValue) + " Increased To: " + charControlScript.fuelUsage;
    }
    public void ActivateObject()
    {
        if (currentLevel == maxLevel)
        {
            objectToActivate.SetActive(true);
        }
    }
    #endregion

    public void ResetUpgrades()
    {
        if (progressionFill.fillAmount > 0)
        {
            //returns all invested CP
            charControlScript.GetComponent<MilestoneRankManager>().TotalCP += totalInvestedCP;
            //reset total invested CP
            totalInvestedCP = 0;
            //reset's the upgrade level to 0
            currentLevel = 0;
            //reset fill bar to 0 / reset invested CP
            investedCP = 0;
            //reset current needed CP
            currentNeededCP = requiredCP;
            #region Upgrade Switches
            switch (upgradableParameters)
            {
                case PlayerParameters.WalkSpeed:
                    charControlScript.defaultSpeed -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.defaultSpeed;
                    break;
                case PlayerParameters.SprintSpeed:
                    charControlScript.sprintSpeed -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.sprintSpeed;
                    break;
                case PlayerParameters.JumpVelocity:
                    charControlScript.jumpVelocity -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.jumpVelocity;
                    break;
                case PlayerParameters.GliderSpeed:
                    charControlScript.defaultGliderSpeed -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.defaultGliderSpeed;
                    break;
                case PlayerParameters.GliderUpForce:
                    charControlScript.defaultUpwardsForce -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.defaultUpwardsForce;
                    break;
                case PlayerParameters.GliderBankSpeed:
                    charControlScript.defaultBankSpeed -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.defaultBankSpeed;
                    break;
                case PlayerParameters.FuelCapacity:
                    charControlScript.fuelCapacity -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.fuelCapacity;
                    break;
                case PlayerParameters.BoostForce:
                    charControlScript.boostForce -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.boostForce;
                    break;
                case PlayerParameters.FuelRefilling:
                    charControlScript.fuelRefill -= totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.fuelRefill;
                    break;
                case PlayerParameters.FuelUsage:
                    charControlScript.fuelUsage += totalUpgradeValue;
                    description_Txt.text = "Starting: " + charControlScript.fuelUsage;
                    break;
                case PlayerParameters.ObjectActivation:
                    
                    break;

            }
            #endregion

            //reset total upgrade value
            totalUpgradeValue = 0;
        }

    }

}

