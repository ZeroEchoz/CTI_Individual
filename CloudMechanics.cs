using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMechanics : MonoBehaviour {

    public ParticleSystem cloudParticles;
    public ParticleSystem fixedCloudParticles;
    public GameObject player;

    public bool emit = false;
    public bool emitFixed = false;
    public float currentInterval;
    public float slowInterval = 0.06f;
    public float fastInterval = 0.010f;

    CharControl charControlScript;
    public float currentVelocityModifier;

    public bool inCloud;

	// Use this for initialization
	void Start () {
        charControlScript = player.GetComponent<CharControl>();
        currentVelocityModifier = cloudParticles.velocityOverLifetime.speedModifierMultiplier;
	}
	
	// Update is called once per frame
	void Update () {

        var cloudEmission = cloudParticles.emission;
        var fixedCloudEmission = fixedCloudParticles.emission;


        //control normal cloud particle modules and parameters
        cloudEmission.enabled = emit;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[cloudEmission.burstCount];
        cloudEmission.GetBursts(bursts);
        bursts[0].repeatInterval = currentInterval;
        cloudEmission.SetBursts(bursts);

        //control fixed cloud particles
        fixedCloudEmission.enabled = emitFixed;

        if (inCloud)
        {
            if (charControlScript.isGliding)
            {
                if (charControlScript.windSfxParameter > 0.5f)
                {
                    currentInterval = fastInterval;
                    currentVelocityModifier = 1.5f;
                }
                else
                {
                    currentInterval = slowInterval;
                    currentVelocityModifier = 0.2f;
                }


                //if the speed is near nothing
                if (charControlScript.windSfxParameter < 0.3f)
                {
                    emit = false;
                    emitFixed = true;

                }
                else
                {
                    emit = true;
                    emitFixed = false;
                }
            }
            else
            {
                if (charControlScript.isGrounded)
                {
                    if (Input.GetAxis("Vertical") > 0
                        || Input.GetAxis("Horizontal") > 0)
                    {
                        emit = true;
                        emitFixed = false;
                        currentInterval = slowInterval;
                        currentVelocityModifier = 0.2f;

                        if (Input.GetButton("Sprint"))
                        {
                            currentInterval = fastInterval;
                            currentVelocityModifier = 1.5f;
                        }
                    }
                    else
                    {
                        emitFixed = true;
                        emit = false;
                    }
                }
                else
                {
                    emit = true;
                    emitFixed = false;
                }
            }

        }
        else
        {
            emit = false;
            emitFixed = false;
        }

        if (!inCloud 
            && !charControlScript.isGliding)
        {
            emit = false;
            emitFixed = false;
        }

	}
}
