using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    [SerializeField] float sprintRechargeDelay = 2;
    float sprintRechargeTimer;
    public float currentMoveSpeed;
    float desiredMoveSpeed;
    public bool InfiniteSprint = false;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchYScale;
    float startYScale;
    bool canStand;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;
    public float groundDrag;

    [Header("Keybinds")]
    public KeyCode toggleMenu = KeyCode.Tab;
    public KeyCode togglePause = KeyCode.Escape;
    public KeyCode crouchKey = KeyCode.C;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Timers")]
    public float walkingSound_Timer = 0, sprintSound_Timer = 0, crouchSound_Timer = 0;
    public float maxSprintTime = 5;
    float sprintTime;
    public float Heartbeat_Timer = 2f;

    [Header("Booleans")]
    public bool sprinting = false, crouching = false, walking, canSprint = true, playerIsMoving, Paused;

    public Transform orientation;
    Vector3 SpawnPlayerPoint;
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    [Header("References")]
    [SerializeField] Transform monsterTransform;
    public GameObject pauseMenu;
    public GameObject taskMenu;
    public GameObject FadeScreen_In;
    public static int TimesDied = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FadeScreen_In.SetActive(true);
        desiredMoveSpeed = walkSpeed;
        sprintRechargeTimer = sprintRechargeDelay;

        SpawnPlayerPoint = new Vector3(1,2,-32);
        sprintTime = maxSprintTime;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //ground check -- 0.5f is half the players height and 0.2f is extra length
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        walking = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.2f);

        playerIsMoving = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f);

        Heartbeat();
        MyInput();
        speedControl();
        RechargeSprint();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        if (gameObject.transform.position.y < -5f)
        {
            gameObject.transform.position = SpawnPlayerPoint;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //Here are the controls that switch the player to different movement states
    //and toggle menus with buttons
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        walkingSound_Timer -= Time.deltaTime;
        sprintSound_Timer -= Time.deltaTime;
        crouchSound_Timer -= Time.deltaTime;

        // Pausing
        if (Input.GetKeyDown(togglePause) && GameEventsManager.PlayerInMenu == false)
        {
            if (pauseMenu.activeInHierarchy)
            {
                Paused = true;
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
                Paused = false;
            }
        }

        if (Paused == false)
        {
            // PlayerMap / TaskMenu
            if (Input.GetKeyDown(toggleMenu))
            {
                if (taskMenu.activeInHierarchy)
                {
                    taskMenu.SetActive(false);
                }
                else
                {
                    taskMenu.SetActive(true);
                }
            }

            // Detects if crouching
            if (Input.GetKey(crouchKey) && CanCrouch())
            {
                // Crouching sound
                if (crouchSound_Timer <= 0 && crouching == true && playerIsMoving == true)
                {
                    SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Footsteps, 0.01f, Random.Range(0.9f, 1.2f));
                    crouchSound_Timer = 0.7f;
                }
            }

            // Start Crouching
            if (Input.GetKeyDown(crouchKey) && CanCrouch())
            {
                currentMoveSpeed = crouchSpeed;

                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 100f, ForceMode.Impulse);

                crouching = true;
                sprinting = false;

                //Debug.Log("Crouching");
            }

            // Stop Crouching
            else if (Input.GetKeyUp(crouchKey) && grounded)
            {
                currentMoveSpeed = walkSpeed;

                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                crouching = false;
            }

            // Sprinting
            else if (Input.GetKey(sprintKey) && crouching == false && sprintTime > 0f && canSprint)
            {
                currentMoveSpeed = sprintSpeed;
                sprintRechargeTimer = sprintRechargeDelay;
                sprinting = true;

                if (!InfiniteSprint)
                sprintTime -= Time.deltaTime;

                // Running sound
                if (sprintSound_Timer <= 0 && sprinting == true && playerIsMoving == true && canSprint == true)
                {
                    SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Footsteps, 0.35f, Random.Range(0.7f, 1.2f));
                    sprintSound_Timer = 0.2375f;
                }
                //Debug.Log("Running");

                if (sprintTime <= 0)
                {
                    canSprint = false;
                    StartCoroutine(SprintExhuastionDelay());
                }
            }

            // Jumping - Probably will not be used
            else if (Input.GetKey(jumpKey) && readyToJump && grounded && crouching == false)
            {
                PlayerJump();
            }

            // Walking
            else if (grounded && crouching == false)
            {
                currentMoveSpeed = walkSpeed;

                sprinting = false;

                //Debug.Log("Walking");

                // Walking sound
                if (walkingSound_Timer <= 0 && walking == true)
                {
                    SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Footsteps, 0.25f, Random.Range(0.8f, 1.2f));
                    walkingSound_Timer = 0.475f;
                }
            }
        }
    }
    public bool CanCrouch()
    {
        return grounded;
    }

    // Here is the method used to move the player
    void PlayerMovement()
    {
        rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Walking Boolean
        if (walking)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }
    }
    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit velocity
        if (flatVel.magnitude > desiredMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * desiredMoveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    void PlayerJump()
    {
        readyToJump = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }


    // Prevents jumping multiple times
    void ResetJump()
    {
        readyToJump = true;
    }

    // Recharges sprint when not running
    void RechargeSprint()
    {
        if (!Input.GetKey(sprintKey) && sprintTime < maxSprintTime)
        {
            if (TimesDied < 3)
                sprintTime += Time.deltaTime / 1.5f;
            else
            {
                sprintTime += Time.deltaTime;
            }
        }

        if (sprintTime == maxSprintTime)
        {
            canSprint = true;
        }
    }
    IEnumerator SprintExhuastionDelay()
    {
        yield return new WaitForSeconds(sprintRechargeDelay);
        canSprint = true;
    }

    void Heartbeat()
    {
        float DistanceFromMonster = Vector3.Distance(monsterTransform.position, gameObject.transform.position);

        if (DistanceFromMonster <= 50f)
        {
            Heartbeat_Timer -= Time.deltaTime;

            if (Heartbeat_Timer <= 0)
            {
                SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Heartbeat, 2f / DistanceFromMonster);

                if (DistanceFromMonster > 30 && DistanceFromMonster < 35)
                {
                    Heartbeat_Timer = 1.9f;
                }
                else if (DistanceFromMonster > 25 && DistanceFromMonster < 30)
                {
                    Heartbeat_Timer = 1.75f;
                }
                else if (DistanceFromMonster > 20 && DistanceFromMonster < 25)
                {
                    Heartbeat_Timer = 1.50f;
                }
                else if (DistanceFromMonster > 15 && DistanceFromMonster < 20)
                {
                    Heartbeat_Timer = 1.30f;
                }
                else if (DistanceFromMonster > 10 && DistanceFromMonster < 15)
                {
                    Heartbeat_Timer = 1.15f;
                }
                else if (DistanceFromMonster > 5 && DistanceFromMonster < 10)
                {
                    Heartbeat_Timer = 0.9f;
                }
                else if (DistanceFromMonster > 0 && DistanceFromMonster < 5)
                {
                    Heartbeat_Timer = .75f;
                }
            }
        }
        else
        {
            Heartbeat_Timer = 1.9f;
        }
    }
}



