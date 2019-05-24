using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    public Text cpTxt;
    public Text totalDayTxt;

    public GameObject CP_Backing;

    public manageTime manageTimeScript;

    public MilestoneRankManager milestoneRankManagerScript;
    public GameObject[] notificationBoxes;
    public Text tooltipText;
    public Image tooltipImage;
    public Vector3 tooltipOffset;
    Text notificationTxt;
    Text cpAwardedTxt;


    private int cpCurrent;
    private int totalDaysPassed;

    SfxControl sfxControlScript;
    [Space(15)]
    public GameObject page_inventory;
    public GameObject page_hearts;
    public GameObject page_map;
    public GameObject page_upgrade;
    public GameObject page_milestones;
    public GameObject page_notes;

    public GameObject[] tabs;


    private void Start()
    {
        StartCoroutine(CheckCP());
        sfxControlScript = GameObject.Find("SFXControl").GetComponent<SfxControl>();
        tooltipImage.gameObject.SetActive(false);

    }
    void Update () {
        cpCurrent = milestoneRankManagerScript.TotalCP;
        totalDaysPassed = manageTimeScript.Totaldaycount;

        cpTxt.text = cpCurrent + " CP";
        totalDayTxt.text = totalDaysPassed.ToString();

        #region Tooltips
        tooltipImage.rectTransform.position = Input.mousePosition + tooltipOffset;
        #endregion
    }

    public void Achievementdetermined(string milestonename, int obtainedCP, int completedRequirement)
    {
        switch (milestonename)
        {
            case "Gliding Time":
                Debug.Log("Gliding Called");
                NotifyCompletion(milestonename, obtainedCP, completedRequirement);
                break;
            case "Items Collected":
                Debug.Log("Item Collected Called");
                NotifyCompletion(milestonename, obtainedCP, completedRequirement);
                break;
            case "Time Played":
                Debug.Log("Time Played Called");
                NotifyCompletion(milestonename, obtainedCP, completedRequirement);
                break;
        }
    }

    public void NotifyCompletion(string milestonenameAssign, int obtainedCPAssign, int completedRequirementAssign)
    {
        for (int i = 0; i < notificationBoxes.Length; i++)
        {
            if (notificationBoxes[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && !notificationBoxes[i].GetComponent<Animator>().IsInTransition(0))
            {
                print("NOTIFICATION SKIP: " + i);
                continue;
            } else if (notificationBoxes[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !notificationBoxes[i].GetComponent<Animator>().IsInTransition(0))
            {
                notificationBoxes[i].GetComponent<Animator>().Play("NotificationAnim", -1, 0f);

                //assign text boxes and appropriate values
                notificationTxt = notificationBoxes[i].transform.Find("DescText").GetComponent<Text>();
                cpAwardedTxt = notificationBoxes[i].transform.Find("PlusCP").transform.Find("CPTXT").GetComponent<Text>();
                cpAwardedTxt.text = "+" + obtainedCPAssign + " CP";
                switch (milestonenameAssign)
                {
                    case "Gliding Time":
                        notificationTxt.text = "Glide for " + "<b>" + completedRequirementAssign + "</b>" + " seconds.";
                        break;
                    case "Items Collected":
                        notificationTxt.text = "Collect " + "<b>" + completedRequirementAssign + "</b>" + " items.";
                        break;
                    case "Time Played":
                        notificationTxt.text = "Play for " + "<b>" + completedRequirementAssign + "</b>" + " seconds.";
                        break;
                }
                print("NOTIFICATION RUNNING: " + i + "/" + notificationBoxes.Length);
                sfxControlScript.MilestoneNotifySFX();
                i = 0;

                print("NOTIFICATION WORKING: " + i);
                break;
            }
        }
    }

    public void ShowTooltipBundles()
    {
        if (Cursor.visible)
        {
            tooltipImage.gameObject.SetActive(true);
        }
    }
    public void HideTooltipBundles()
    {
        tooltipImage.gameObject.SetActive(false);
    }



    IEnumerator CheckCP()
    {
        while (Application.isPlaying)
        {
            var prevAmt = cpCurrent;
            yield return new WaitForEndOfFrame();
            var differenceValue = cpCurrent - prevAmt;
            if (differenceValue > 0)
            {
                CP_Backing.GetComponent<Animator>().Play("CPGain_Anim", -1, 0f);
            }
        }
    }

    public void TabManagement(Button tab)
    {
        print(tab.name);
        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i].name == tab.name)
            {
                tabs[i].GetComponent<Animator>().SetBool("Open", true);
            } else
            {
                tabs[i].GetComponent<Animator>().SetBool("Open", false);
            }
        }
    }

    public void Tab_Inventory()
    {
        sfxControlScript.InventoryOpen();

        page_inventory.SetActive(true);
        page_hearts.SetActive(false);
        page_map.SetActive(false);
        page_upgrade.SetActive(false);
        page_milestones.SetActive(false);
        page_notes.SetActive(false);
    }
    public void Tab_Hearts()
    {
        sfxControlScript.InventoryOpen();

        page_inventory.SetActive(false);
        page_hearts.SetActive(true);
        page_map.SetActive(false);
        page_upgrade.SetActive(false);
        page_milestones.SetActive(false);
        page_notes.SetActive(false);
    }
    public void Tab_Map()
    {
        sfxControlScript.InventoryOpen();

        page_inventory.SetActive(false);
        page_hearts.SetActive(false);
        page_map.SetActive(true);
        page_upgrade.SetActive(false);
        page_milestones.SetActive(false);
        page_notes.SetActive(false);
    }
    public void Tab_Upgrade()
    {
        sfxControlScript.InventoryOpen();

        page_inventory.SetActive(false);
        page_hearts.SetActive(false);
        page_map.SetActive(false);
        page_upgrade.SetActive(true);
        page_milestones.SetActive(false);
        page_notes.SetActive(false);
    }
    public void Tab_Milestones()
    {
        sfxControlScript.InventoryOpen();

        page_inventory.SetActive(false);
        page_hearts.SetActive(false);
        page_map.SetActive(false);
        page_upgrade.SetActive(false);
        page_milestones.SetActive(true);
        page_notes.SetActive(false);
    }
    public void Tab_Notes()
    {
        sfxControlScript.InventoryOpen();

        page_inventory.SetActive(false);
        page_hearts.SetActive(false);
        page_map.SetActive(false);
        page_upgrade.SetActive(false);
        page_milestones.SetActive(false);
        page_notes.SetActive(true);
    }
}
