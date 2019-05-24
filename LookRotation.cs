using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour
{
	private Rigidbody _parentRB;
    private CharControl charControlScript;
    bool once;
    bool isColliding;
    //private CharControl2 upliftScript;

    private void Awake()
    {
        once = false;
        charControlScript = transform.parent.GetComponent<CharControl>();
        //upliftScript = transform.parent.GetComponent<CharControl2>();
    }
    private void Start()
    {
        _parentRB = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame

    void Update()
    {
        //if velocity is greater than zero, and is not gliding, and if movement buttons are pressed, rotate the character model to where the player is moving
        Vector3 groundVelocity = new Vector3(_parentRB.velocity.x, 0, _parentRB.velocity.z);
        Vector3 lookDir = new Vector3(charControlScript.GetComponent<Rigidbody>().velocity.x, 0, charControlScript.GetComponent<Rigidbody>().velocity.y);
        if (groundVelocity != Vector3.zero
            && !charControlScript.isGliding
            && (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.1f))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(groundVelocity), 0.2f);
        }

        //rotate this child object to parent forward 
        if (!once
            && charControlScript.isGliding)
        {
            transform.rotation = Quaternion.Euler(0, charControlScript.transform.eulerAngles.y, 0);
            once = true;
        }

        if (!charControlScript.isGliding
            && once)
        {
            once = false;
        }
    }
}
