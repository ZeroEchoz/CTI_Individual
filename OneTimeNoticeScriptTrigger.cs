using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneTimeNoticeScriptTrigger : MonoBehaviour {

    SfxControl sfxScript;
    CharControl charControlScript;
    public GameObject notice;

    public void Start()
    {
        sfxScript = GameObject.Find("SFXControl").GetComponent<SfxControl>();
        charControlScript = GameObject.Find("Avatar").GetComponent<CharControl>();
        notice.SetActive(false);
    }

    public void ExecuteNotice()
    {
        Cursor.visible = true;
        notice.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Avatar")
        {
            ExecuteNotice();
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
