using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    [SerializeField] float sprintRechargeDelay = 2;
    float sprintRechargeTimer;
    float currentMoveSpeed;
    float desiredMoveSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchYScale;
    float startYScale;
    [SerializeField] float distanceAbovePlayer = 0.5f;
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

    [Header("Booleans")]
    public static bool sprinting = false, crouching = false, walking, canSprint = true, playerIsMoving, Paused;

    public Transform orientation;
    Vector3 spawnPoint;
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    public GameObject pauseMenu;
    public GameObject taskMenu;
    public GameObject FadeScreen_In;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FadeScreen_In.SetActive(true);
        desiredMoveSpeed = walkSpeed;
        sprintRechargeTimer = sprintRechargeDelay;

        sprintTime = maxSprintTime;

        spawnPoint = transform.position;

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

        //Debug.Log(currentMoveSpeed);
        Debug.Log(sprinting);
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

        StopAllCoroutines();
        StartCoroutine(SmoothlyLerpMoveSpeed());

        // Pausing
        if (Input.GetKeyDown(togglePause))
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
                    SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Crouching, 0.1f, Random.Range(0.9f, 1.2f));
                    crouchSound_Timer = 0.7f;
                }
            }

            // Start Crouching
            if (Input.GetKeyDown(crouchKey) && CanCrouch())
            {
                desiredMoveSpeed = crouchSpeed;

                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

                crouching = true;
                sprinting = false;

                Debug.Log("Crouching");
            }

            // Stop Crouching
            else if (Input.GetKeyUp(crouchKey) && grounded)
            {
                desiredMoveSpeed = walkSpeed;

                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                crouching = false;
            }

            // Sprinting
            else if (Input.GetKey(sprintKey) && grounded && crouching == false && sprintTime > 0f)
            {
                desiredMoveSpeed = sprintSpeed;
                sprintRechargeTimer = sprintRechargeDelay;
                sprinting = true;

                sprintTime -= Time.deltaTime;

                // Running sound
                if (sprintSound_Timer <= 0 && sprinting == true && playerIsMoving == true && canSprint == true)
                {
                    SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Sprinting, 0.3f, Random.Range(0.7f, 1.2f));
                    sprintSound_Timer = 0.2375f;
                }
                Debug.Log("Running");
            }

            // Jumping - Probably will not be used
            else if (Input.GetKey(jumpKey) && readyToJump && grounded && crouching == false)
            {
                PlayerJump();
            }

            // Walking
            else if (grounded && crouching == false)
            {
                desiredMoveSpeed = walkSpeed;

                sprinting = false;

                Debug.Log("Walking");

                // Walking sound
                if (walkingSound_Timer <= 0 && walking == true)
                {
                    SoundManager.PlaySound(SoundSource.Player, SoundType.Player_Walking, 0.2f, Random.Range(0.8f, 1.2f));
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

    // Current speed lerps to the speed of the current state(walking, sprinting, etc)
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // lerp movementSpeed to desired speed
        float difference = Mathf.Abs(desiredMoveSpeed - currentMoveSpeed);
        //float startValue = currentMoveSpeed;
        
            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, desiredMoveSpeed, Time.deltaTime);

            yield return null;
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
        if (!Input.GetKey(sprintKey) || sprintTime < maxSprintTime)
        {
             sprintRechargeTimer -= Time.deltaTime;

            if (sprintRechargeTimer <= 0f)
            {
                sprintTime += Time.deltaTime;
            }
        }

        /*// In case the timer goes above max allowed time
        if (sprintTime > maxSprintTime)
        {
            sprintTime = maxSprintTime;
        }*/
    }
}



