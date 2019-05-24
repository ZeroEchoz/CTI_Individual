using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CharControl : MonoBehaviour {


    #region Public Variables
    [SerializeField]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public SfxControl sfxControlScript;
    public Transform startingRespawnPoint;

    [Space(15)]

    [Tooltip("Cinemachine camera")]
    public CinemachineFreeLook cineCam;
    [Tooltip("Main Camera")]
    public GameObject cam;
    [Tooltip("Glider GameObject")]
    public GameObject glider;
    [Tooltip("Full map in canvas")]
    public GameObject map;
    public GameObject upgradeWindow;
    public GameObject milestoneWindow;
    public GameObject notesWindow;
    

    [Space(10)]

    public Text speedText;
    public Text altitudeText;
    public Image boostMetre;
    [Tooltip("Marks where player will respawn")]
    public Transform respawnPoint;
    [Tooltip("How far back you want the respawn point to be (The more negative, the further back ; 0 = on the spot)")]
    [Range(-5, 0)]
    public float respawnPointPosition = -1f;
    public GameObject playerModel;

    [Space(15)]

    [Tooltip("Default move speed on the ground")]
    public float defaultSpeed = 6f;
    [Tooltip("Speed when sprinting")]
    public float sprintSpeed = 10f;
    [Tooltip("Strength of jump")]
    [Range (5, 15)]
    public float jumpVelocity = 10f;
    [Range(1, 10)]
    [Tooltip("How fast the player falls normally")]
    public float fallMultiplier = 4f;
    [Tooltip("Checks to see if the player is making contact with the ground, no need to change this")]
    public bool isGrounded = true;

    [Space(10)]

    [Tooltip("Base speed when moving forwards/diving. Also affects speed when rising")]
    public float defaultGliderSpeed = 15f;
    [Range(0.1f, 9.7f)]
    [Tooltip("Force that makes the player float when the glider is active")]
    public float defaultUpwardsForce = 9.0f;
    float upwardsForce;
    [Tooltip("Increase this using upgrades for more base turning speed")]
    public float defaultBankSpeed = 15f;
    float bankSpeed;

    [Space(15)]

    public float currentFuel;
    [Tooltip("Fuel for boost")]
    public float fuelCapacity = 250f;
    [Tooltip("Force of boost. This replaces the glider speed when boosting")]
    public float boostForce = 25f;
    [Tooltip("The rate at which fuel is refilled")]
    public float fuelRefill = 0.25f;
    [Tooltip("The rate at which fuel is used")]
    public float fuelUsage = 1f;
    [Tooltip("Time (in seconds) before fuel recharges when empty")]
    public float fuelRechargeGap = 1.0f;

    [Space(15)]

    [Tooltip("How far will the player falls before respawning")]
    public float respawnDepth = 40f;
    #endregion

    public bool isGliding = false;
    public bool hasFuel = true;

    public float windSfxParameter;
    [Tooltip("Updates Automatically. Returns the tag of the current object the player is walking on")]
    public string currentWalkingMaterial;

    public bool invertGliding;
    //how far up the ground you need to be, to be able to deploy the glider
    public float deployHeight = 2.0f;
    [Space(10)]
    public GameObject storedItem;

    #region Private Variables

    //ints/floats
    private float moveSpeed;
    private float moveX;
    private float moveZ;
    private float zRot;
    private float xRot;
    private float gliderSpeed;
    private float inputVal;
    bool windowOpen;

    [HideInInspector]
    public bool canGlide;

    Rigidbody rb;
    ConstantForce cf;
    Animator animController;
    
    #endregion

    private void Awake()
    {
        upwardsForce = defaultUpwardsForce;
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
        zRot = transform.rotation.eulerAngles.z;
        xRot = transform.rotation.eulerAngles.x;
        currentFuel = fuelCapacity;
        animController = transform.GetChild(1).GetComponent<Animator>();
        inputVal = 0.1f;
        map.SetActive(false);
        respawnPoint.position = startingRespawnPoint.position;
        windowOpen = false;
    }

    void Start () {
        moveSpeed = defaultSpeed;
        //disable glide movement keys for this long
    }

    private void Update()
    {
        //put inputs in update, not in fixed update

        #region INPUTS

        switch (isGliding)
        { 
            case true:
                #region During Gliding

                //disable glider
                if (Input.GetButtonDown("Jump")
                    || isGrounded)
                {
                    //switch back to ground movement
                    animController.SetBool("Glide", false);
                    isGliding = false;
                    sfxControlScript.GliderClose();
                }
                #endregion
                break;
            case false:
                #region During Ground Movement
                //jump
                //ground movement
                if (Input.GetButtonDown("Jump")
                    && isGrounded)
                {
                    //jump if grounded
                    rb.velocity = Vector3.up * jumpVelocity;
                }
                else if (Input.GetButtonDown("Jump") && !isGrounded
                    && canGlide)
                {
                    //switch to gliding movement
                    sfxControlScript.GliderOpen();
                    isGliding = true;
                    glider.SetActive(true);
                    GliderRebound();
                }
                #endregion
                break;
        }


        //TURN CURSOR VISIBLE WHILE HOLDING DOWN ALT
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
        } else
        {
            Cursor.visible = false;
        }
        #endregion


        #region Constantly Running
        CheckGrounded();
        RespawnPlayer();
        ControlMap();
        UpgradeWindowControl();
        MilestoneTrackerWindow();
        if (invertGliding)
        {
            inputVal *= -1f;
        }
        #endregion
    }
    
    void FixedUpdate () {
        windSfxParameter = gliderSpeed / boostForce;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        //max speed: 150km/h
        speedText.text = "" + Mathf.Round(Mathf.Abs(rb.velocity.magnitude)) + "m/s";
        altitudeText.text = "Altitude: " + Mathf.Round(transform.position.y) + "m";

        currentFuel = Mathf.Clamp(currentFuel, 0, fuelCapacity);
        boostMetre.fillAmount = currentFuel / fuelCapacity;
        //print(currentFuel);
        if (currentFuel >= 1.1f)
        {
            hasFuel = true;
            currentFuel += fuelRefill / 6;
        } else if (currentFuel < 1)
        {
            hasFuel = false;
        }

        if (!hasFuel
            && currentFuel < fuelCapacity)
        {
            InvokeRepeating("Refill", fuelRechargeGap, 1.0f);

        }

        if (currentFuel >= fuelCapacity)
        {
            CancelInvoke("Refill");
        }

        //fall faster when not gliding
        if (rb.velocity.y < 0
            && !isGliding)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        DetectGlideDeployementHeight();

        #region Check if Gliding
        //if gliding is not active, use ground movements, else use gliding movement

        switch(isGliding)
        {
            case true:
                GlidingMovement();
                ApplyForce();
                break;
            case false:
                GroundMovement();
                break;
            default:
                print("Invalid statement");
                break;
        }
        #endregion
    }

    private void LateUpdate()
    {
        if (storedItem != null)
        {
            storedItem = null;
        }
    }

    void DetectGlideDeployementHeight()
    {
        RaycastHit hitNew = new RaycastHit();

        //check to see if grounded
        if (Physics.Raycast(transform.position, Vector3.down, out hitNew, deployHeight))
        {
            canGlide = false;
            print("can't glide");
        } else
        {
            canGlide = true;
        }
        Debug.DrawRay(transform.position, Vector3.down * deployHeight, Color.blue);

    }


    public void GroundMovement()
    {
        //reset glider speed
        gliderSpeed = 0f;
        //turn off upwards force
        cf.enabled = false;
        glider.SetActive(false);


        //var camForward = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
        //var camRight = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);

        //basic movement
        //WASD movement
        moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveZ = Input.GetAxisRaw("Vertical") * moveSpeed;

        var localVel = transform.InverseTransformDirection(rb.velocity);
        localVel.x = moveX;
        localVel.z = moveZ;

        rb.velocity = transform.TransformDirection(localVel);


        //control anims
        //TEMPORARY - WILL MAKE BETTER W/ MORE ANIMS
        if (Mathf.Abs(moveX) > 0
            || Mathf.Abs(moveZ) > 0)
        {
            animController.SetInteger("Movement", 1);
            if (Input.GetKey(sprintKey)
                || Input.GetButton("Sprint"))
            {
                animController.SetBool("Sprint", true);
            }
            else
            {
                animController.SetBool("Sprint", false); ;
            }
        } else
        {
            animController.SetInteger("Movement", 0);
        }


        //look towards where the player is moving
        //if (localVel != Vector3.zero)
        //{
        //    transform.localRotation = Quaternion.LookRotation(localVel, Vector3.up);
        //}

        
        //this makes the player avatar look where the camera is looking
        if (Input.GetAxis("Vertical") > 0.1f | Input.GetAxis("Vertical") < -0.1f)
        {
            Quaternion turnAngle = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, turnAngle, 0.7f);
        }
        //make the avatar look left or right relative to the camera direction
        //RIGHT
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            Quaternion turnAngle = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, turnAngle, 0.7f);
        }
        //LEFT
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            Quaternion turnAngle = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, turnAngle, 0.7f);
        }



        //sprint when key is held down, otherwise use normal run speed
        switch (Input.GetKey(sprintKey)
            || Input.GetButton("Sprint"))
        {
            case true:
                //moveSpeed = sprintSpeed;
                if (Input.GetAxis("Vertical") > 0
                    || Input.GetAxis("Horizontal") > 0)
                {
                    //rb.AddForce(transform.forward * sprintSpeed);
                    moveSpeed = Mathf.SmoothStep(moveSpeed, sprintSpeed, 0.5f);
                }
                //print("Vel: " + rb.velocity.z);
                break;
            case false:
                moveSpeed = Mathf.SmoothStep(moveSpeed, defaultSpeed, 0.5f);
                break;
        }


    }

    public void CheckGrounded()
    {
        Vector3 groundVelocity = new Vector3(rb.velocity.x - 5f, 0, rb.velocity.z - 5f);

        RaycastHit hit = new RaycastHit();

        //check to see if grounded
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.12f))
        {
            animController.SetBool("Jump", false);

            if (hit.collider.tag != "rings"
                && hit.collider.tag != "cloud"
                && hit.collider.tag != "rainCloud")
            {
                isGrounded = true;
            } else
            {
                isGrounded = false;
            }

            if (hit.collider.tag == "Island")
            {
                respawnPoint.position = playerModel.transform.TransformPoint(0, 0, respawnPointPosition);
            }
            currentFuel += fuelRefill / 2;
            currentWalkingMaterial = hit.collider.tag;
        }
        else
        {
            animController.SetBool("Jump", true);
            isGrounded = false;
        }
        Debug.DrawRay(transform.position, Vector3.down * 1.12f, Color.red);
    }

    public void GlidingMovement()
    {
        //print(transform.localEulerAngles.x);
        

        var rotation = transform.rotation;
        var diveRotation = Quaternion.Euler (50.0f, cam.transform.eulerAngles.y, 0);
        var riseRotation = Quaternion.Euler(-40.0f, cam.transform.eulerAngles.y, 0);
        var bankLeft = Quaternion.Euler(0, cam.transform.eulerAngles.y, 25.0f);
        var bankRight = Quaternion.Euler(0, cam.transform.eulerAngles.y, -25.0f);
        var defaultRotation = Quaternion.Euler(0,cam.transform.eulerAngles.y,0);

        animController.SetBool("Glide", true);

        #region Vertical Movements

        //move forward (and dive downwards)
        if (((Input.GetAxis("Vertical") > 0.1f) && !invertGliding)
            || ((Input.GetAxis("Vertical") < -0.1f) && invertGliding))
        {
            transform.localRotation = Quaternion.Lerp(rotation, diveRotation, 0.04f);
            //rotate the character in the direction the camera is facing         
        }

        //move upwards (also tilt upwards)
        if (((Input.GetAxis("Vertical") < -0.1f) && !invertGliding)
            || ((Input.GetAxis("Vertical") > 0.1f) && invertGliding))
        {
            transform.localRotation = Quaternion.Lerp(rotation, riseRotation, 0.04f);
        }

        #endregion

        #region Horizontal/Banking Movements

        if (Input.GetAxisRaw("Horizontal") < -0.1f)
        {
            transform.localRotation = Quaternion.Lerp(rotation, bankLeft, 0.04f);
        }

        if (Input.GetAxisRaw("Horizontal") > 0.1f)
        {
            transform.localRotation = Quaternion.Lerp(rotation, bankRight, 0.04f);
        }

        #endregion

        //if nothing is pressed, default orientation back to normal/default
        
        if ((Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.1f) && (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.1f))
        {
            transform.localRotation = Quaternion.Lerp(rotation, defaultRotation, 0.04f);
            print((Mathf.Abs(Input.GetAxisRaw("Vertical")) + " " + (Mathf.Abs(Input.GetAxisRaw("Horizontal")))));
        }
        
        /*
        //if nothing is pressed, default orientation back to normal/default
        if (!Input.anyKey)
        {
            transform.localRotation = Quaternion.Lerp(rotation, defaultRotation, 0.04f);
        }
        */

        //BOOST
        if (Input.GetKey(sprintKey)
            && hasFuel)
        {
            Boost();
        } else
        {
            gliderSpeed = Mathf.Lerp(gliderSpeed, 0, 0.001f);
        }
    }

    public void Boost()
    {

        currentFuel -= fuelUsage;
        gliderSpeed = Mathf.Lerp(gliderSpeed, boostForce + defaultGliderSpeed, 1 * Time.deltaTime);

    }

    void Refill()
    {
        currentFuel += fuelRefill;
        //print("refilling");
    }

    public void ApplyForce()
    {
        //Debug.Log((gliderSpeed / boostForce));
        float forwardVelocity = rb.velocity.z;
        float verticalVelocity = Mathf.Abs(rb.velocity.y);

        float defaultDrag = 0.35f;
        float increasedDrag = 1.0f;

        //print(forwardVelocity);
        //activate constant force upwards
        cf.enabled = true;
        cf.force = new Vector3(0, upwardsForce, 0);
        cf.relativeForce = new Vector3(bankSpeed, 0, gliderSpeed);

        //move forward (also input when diving)
        if (((Input.GetAxis("Vertical") > 0.1f) && !invertGliding)
            || ((Input.GetAxis("Vertical") < -0.1f) && invertGliding))
        {
            //gliderSpeed = defaultGliderSpeed;
            upwardsForce = defaultUpwardsForce - 2;
            if (!Input.GetKey(sprintKey))
            {
                gliderSpeed = defaultGliderSpeed;
            }

        } else
        {
            gliderSpeed = Mathf.Lerp(gliderSpeed, 0, 0.001f);
        }

        //move upward (also input when rising)
        if (((Input.GetAxisRaw("Vertical") < -0.1f) && !invertGliding)
            || ((Input.GetAxisRaw("Vertical") > 0.1f) && invertGliding)) //Input.GetAxis("Vertical") < -0.1f
        {
            //if falling, add falling velocity to upwards force
            if (rb.velocity.y < -0.1f)
            {
                upwardsForce = defaultUpwardsForce + Mathf.Abs(forwardVelocity) + Mathf.Abs(rb.velocity.y);
                gliderSpeed = Mathf.Lerp(gliderSpeed, boostForce + defaultGliderSpeed, 1 * Time.deltaTime);
                //increase drag gradually
                //print(rb.angularVelocity.x);
            }
            //resets upwards force
            upwardsForce = Mathf.Lerp(upwardsForce, defaultUpwardsForce, 0.05f);
            rb.drag = Mathf.Lerp(rb.drag, increasedDrag, 0.5f * Time.deltaTime);
        } else if (!Input.GetKey(KeyCode.S))
        {
            upwardsForce = Mathf.Lerp(upwardsForce, defaultUpwardsForce, 0.05f);
            gliderSpeed = Mathf.Lerp(gliderSpeed, 0, 0.2f * Time.deltaTime);
            //reset drag
            rb.drag = defaultDrag;
        }

        if (Input.GetAxisRaw("Horizontal") < -0.1f)
        {
            bankSpeed = -defaultBankSpeed;
        } else if (Input.GetAxisRaw("Horizontal") > 0.1f)
        {
            bankSpeed = defaultBankSpeed;
        } else
        {
            bankSpeed = 0;
        }

    }

    public void GliderRebound()
    {
        float reboundForce = ((10.0f + defaultUpwardsForce) * Mathf.Abs(rb.velocity.y) * 3);
        //when falling without a glider, add extra up force when deploying the glider to prevent plumeting when glider is opened
        if (rb.velocity.y <= 3.0f)
        {
            rb.AddRelativeForce(Vector3.up * reboundForce);
        }
    }

    public void RespawnPlayer()
    {

        //respawn if player falls at this depth
        if (transform.position.y < -respawnDepth)
        {
            animController.SetBool("Glide", false);
            isGliding = false;
            transform.position = respawnPoint.position;
            transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

        }
    }
    public void ControlMap()
    {
        if (Input.GetKeyDown(KeyCode.M)
            && !map.activeSelf)
        {
            map.SetActive(true);
            Cursor.visible = true;
            sfxControlScript.InventoryOpen();
        } else if (Input.GetKeyDown(KeyCode.M)
            && map.activeSelf)
        {
            map.SetActive(false);
            Cursor.visible = false;
            sfxControlScript.InventoryClose();
        }
    }

    public void UpgradeWindowControl()
    {
        if (Input.GetKeyDown(KeyCode.U)
            && !upgradeWindow.activeSelf)
        {
            FreezeCam();
            upgradeWindow.SetActive(true);
            sfxControlScript.InventoryOpen();
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.U)
          && upgradeWindow.activeSelf)
        {
            UnfreezeCam();
            upgradeWindow.SetActive(false);
            sfxControlScript.InventoryClose();
            Cursor.visible = false;
        }
    }

    public void NotesWindowControl()
    {
        if (Input.GetKeyDown(KeyCode.N)
            && !upgradeWindow.activeSelf)
        {
            FreezeCam();
            notesWindow.SetActive(true);
            sfxControlScript.InventoryOpen();
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.N)
          && upgradeWindow.activeSelf)
        {
            UnfreezeCam();
            notesWindow.SetActive(false);
            sfxControlScript.InventoryClose();
            Cursor.visible = false;
        }
    }


    public void MilestoneTrackerWindow()
    {

        if (Input.GetKeyDown(KeyCode.K) //OPEN
            && !milestoneWindow.activeSelf)
        {
            FreezeCam();
            milestoneWindow.SetActive(true);
            //milestoneWindow.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            sfxControlScript.InventoryOpen();
            windowOpen = true;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) //CLOSE
            && milestoneWindow.activeSelf)
        {
            UnfreezeCam();
            milestoneWindow.SetActive(false);
            //milestoneWindow.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            sfxControlScript.InventoryClose();
            windowOpen = false;
            Cursor.visible = false;
        }
    }

    public void FreezeCam()
    {
        cineCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0;
        cineCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0;
    }

    public void UnfreezeCam()
    {
        cineCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 250;
        cineCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2;
        Cursor.visible = false;
    }
}
