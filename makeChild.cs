using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class makeChild : MonoBehaviour {

    public GameObject parentObj;
    public CinemachineFreeLook freelookCam;

    public CinemachineOrbitalTransposer _COT_Top;
    public CinemachineOrbitalTransposer _COT_Mid;
    public CinemachineOrbitalTransposer _COT_Bot;

    private void Start()
    {
        parentObj = this.gameObject;
        _COT_Top = freelookCam.GetRig(0).GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _COT_Mid = freelookCam.GetRig(1).GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _COT_Bot = freelookCam.GetRig(2).GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Avatar"
            && !GetComponent<AirshipMovement>().docked)
        {
            other.gameObject.transform.parent = parentObj.transform;
            //disable jumping
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            #region damping
            _COT_Top.m_XDamping = 0;
            _COT_Top.m_YDamping = 0;
            _COT_Top.m_ZDamping = 0;


            _COT_Mid.m_XDamping = 0;
            _COT_Mid.m_YDamping = 0;
            _COT_Mid.m_ZDamping = 0;


            _COT_Bot.m_XDamping = 0;
            _COT_Bot.m_YDamping = 0;
            _COT_Bot.m_ZDamping = 0;
            #endregion
            print("is child");
        }
        else if (other.gameObject.name == "Avatar"
        && GetComponent<AirshipMovement>().docked)
        {
            other.gameObject.transform.parent = null;
            //enable jumping
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            #region damping
            _COT_Top.m_XDamping = 1;
            _COT_Top.m_YDamping = 1;
            _COT_Top.m_ZDamping = 1;


            _COT_Mid.m_XDamping = 1;
            _COT_Mid.m_YDamping = 1;
            _COT_Mid.m_ZDamping = 1;


            _COT_Bot.m_XDamping = 1;
            _COT_Bot.m_YDamping = 1;
            _COT_Bot.m_ZDamping = 1;
            #endregion
            print("is NOT child");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Avatar")
        {
            other.gameObject.transform.parent = null;
            //enable jumping
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            #region damping
            _COT_Top.m_XDamping = 1;
            _COT_Top.m_YDamping = 1;
            _COT_Top.m_ZDamping = 1;


            _COT_Mid.m_XDamping = 1;
            _COT_Mid.m_YDamping = 1;
            _COT_Mid.m_ZDamping = 1;


            _COT_Bot.m_XDamping = 1;
            _COT_Bot.m_YDamping = 1;
            _COT_Bot.m_ZDamping = 1;
            #endregion

        }
    }

}
