using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressF_Respects : MonoBehaviour {

    public GameObject notification;
    public int CP_Award = 10;
    
    public BoxCollider[] pressF_Triggers;
    MilestoneRankManager milestoneRankManagerScript;
    SfxControl sfxScript;

    Text CPText;

    public void Start()
    {
        milestoneRankManagerScript = GameObject.Find("Avatar").GetComponent<MilestoneRankManager>();
        sfxScript = GameObject.Find("SFXControl").GetComponent<SfxControl>();
        CPText = notification.transform.Find("CP").transform.Find("CPTxt").GetComponent<Text>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Avatar")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                notification.SetActive(true);
                milestoneRankManagerScript.TotalCP += CP_Award;
                CPText.text = "+" + "<b>" + CP_Award + "</b>" + "CP";
                sfxScript.EasterEggNotifySFX();
                for (int i = 0; i < pressF_Triggers.Length; i++)
                {
                    pressF_Triggers[i].enabled = false;
                }
            }
        }
    }
}
