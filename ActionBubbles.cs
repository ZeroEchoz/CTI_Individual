using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class ActionBubbles : MonoBehaviour {

    [HideInInspector]
    public TestBubble currentBubble;

    public Transform target;
    public Transform mainCamera;

    public Sprite[] allActionBubbles;

    public Vector3 offset = new Vector3(0, 4, 0);
    [Space(15)]
    public DayNightCycle dayNightScript;
    public manageTime manageTimeScript;
    public string currentTime;
    public Flowchart actionBubbleFlowchart;

    bool turnOff;

    public enum TestBubble
    {
        Blacksmith, Reading, Drinking, Eating, Gardening, TeaCoffee, Painting,
        Smoking, Writing, Tarot, Guitar, Stargazing, Letters, Computer, Off
    }

    private void Update()
    {
        transform.position = target.transform.position + offset;

        var lookPos = mainCamera.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);

        if (turnOff)
        {
            GetComponent<Image>().enabled = false;
        } else
        {
            GetComponent<Image>().enabled = true;
        }

        #region testBubbbles
        /*
        switch (currentBubble)
        {
            case TestBubble.Blacksmith:
                Blacksmithing();
                break;
            case TestBubble.Reading:
                Reading();
                break;
            case TestBubble.Drinking:
                Drinking();
                break;
            case TestBubble.Eating:
                Eating();
                break;
            case TestBubble.Gardening:
                Gardening();
                break;
            case TestBubble.TeaCoffee:
                TeaCoffee();
                break;
            case TestBubble.Painting:
                Painting();
                break;
            case TestBubble.Smoking:
                Smoking();
                break;
            case TestBubble.Writing:
                Writing();
                break;
            case TestBubble.Tarot:
                Tarot();
                break;
            case TestBubble.Guitar:
                Guitar();
                break;
            case TestBubble.Stargazing:
                StarGazing();
                break;
            case TestBubble.Letters:
                Letters();
                break;
            case TestBubble.Computer:
                Computer();
                break;
            case TestBubble.Off:
                Off();
                break;

        }
        */
        #endregion

    }

    public void LateUpdate()
    {
        //automatically call whatever block is currently related to time of day + current day
        currentTime = manageTimeScript.CurrentDay.ToString() + "_" + dayNightScript.currentTime.ToString();
        actionBubbleFlowchart.ExecuteBlock(currentTime);
    }


    #region All Bubble Methods
    public void Blacksmithing()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[0];
   }
    public void Reading()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[1];
    }
    public void Drinking()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[2];
    }
    public void Eating()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[3];
    }
    public void Gardening()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[4];
    }
    public void TeaCoffee()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[5];
    }
    public void Painting()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[6];
    }
    public void Smoking()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[7];
    }
    public void Writing()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[8];
    }
    public void Tarot()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[9];
    }
    public void Guitar()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[10];
    }
    public void StarGazing()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[11];
    }
    public void Letters()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[12];
    }
    public void Computer()
    {
        turnOff = false;
        GetComponent<Image>().sprite = allActionBubbles[13];
    }
    public void Off()
    {
        turnOff = true;
    }
    #endregion

}
