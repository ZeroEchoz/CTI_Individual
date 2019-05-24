using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeScript : MonoBehaviour {

    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
        gameObject.SetActive(true);
	}
	

    public void Close()
    {
        anim.SetBool("CloseNotice", true);
    }
}
