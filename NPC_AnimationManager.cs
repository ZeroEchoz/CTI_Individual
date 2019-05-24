using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AnimationManager : MonoBehaviour {

    [Tooltip("Change this value to change the NPC's animation. 0 = idle ; 1 = walk ; 2 = sit ; 3 = jog/run (only available for waylon)")]
    [Range (0, 3)]
    public int movementValue;

    public float npcSpeed;

    Animator animator;

    [HideInInspector]
    public bool sitting;

    public BoxCollider thisTrigger;

    Vector3 defaultTriggerSize;
    Vector3 smallTriggerSize;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CalculateSpeed());
        thisTrigger = transform.Find("trigger").gameObject.GetComponent<BoxCollider>();

        defaultTriggerSize = thisTrigger.size;
        smallTriggerSize = defaultTriggerSize * 0.5f;

    }

    private void Update()
    {
        //npcSpeed = npcWalkPatrolScript.speed;
        animator.SetInteger("Movement", movementValue);

    }

    private void FixedUpdate()
    {
        if (npcSpeed <= 0)
        {
            thisTrigger.size = smallTriggerSize;
            if (!sitting)
            {
                movementValue = 0;
            }
            else if (sitting)
            {
                movementValue = 2;
            }
        }
        if (npcSpeed > 0
            && npcSpeed < 2)
        {
            movementValue = 1;
            thisTrigger.size = defaultTriggerSize;
        }
        if (npcSpeed > 2
            && this.gameObject.name == "Waylon")
        {
            movementValue = 3;
            thisTrigger.size = defaultTriggerSize;
        }

    }

    IEnumerator CalculateSpeed()
    {
        while (Application.isPlaying)
        {
            //set previous pos
            var prevPos = transform.position.magnitude;
            yield return new WaitForEndOfFrame();
            //calculate velocity
            npcSpeed = Mathf.Abs((prevPos - transform.position.magnitude) / Time.deltaTime);
        }
    }
}
