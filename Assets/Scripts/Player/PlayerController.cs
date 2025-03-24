using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    float currentMoveSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;

    [Header("PlayerStats")]
    public int lives = 3;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;
    public float groundDrag;

    [Header("Keybinds")]
    public KeyCode toggleMenu = KeyCode.Tab;
    public KeyCode togglePause = KeyCode.Escape;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Timers")]
    public float walkingSound_Timer = 0;
    public float maxSprintTime = 5;
    float sprintTime;

    [Header("Booleans")]
    public static bool sprinting, crouching, walking, cannotSprint;

    public Transform orientation;
    Vector3 spawnPoint;
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    public GameObject pauseMenu;
    public GameObject taskMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyToJump = true;
        crouching = false;
        cannotSprint = false;
        currentMoveSpeed = walkSpeed;
        sprinting = false;

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

        walking = Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.2f;

        MyInput();
        speedControl();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        Debug.Log(sprintTime);
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //put player in spawn if they collide with enemy and they have lives left
        //restart the game if they dont have any lives
        if (collider.gameObject.tag == "Enemy")
        {
            lives--;

            if (lives <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                transform.position = spawnPoint;
            }
        }
    }

    //Here are the controls that switch the player to different movements 
    //and activate objects with buttons
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        walkingSound_Timer -= Time.deltaTime;

        //Walking sound
        if (walkingSound_Timer <= 0 && walking)
        {
            SoundManager.PlaySound(SoundType.Player_Walking, 1f);
            walkingSound_Timer = 0.475f;
        }

        // Jumping
        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded && crouching == false)
        {
            Jump();
        }

        // Start Crouch
        if (Input.GetKeyDown(crouchKey) && grounded && crouching == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
            cannotSprint = true;
        }

        // Stop Crouch
        if (Input.GetKeyUp(crouchKey) && grounded && crouching == true)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            crouching = false;
            cannotSprint = false;
        }

        // Pausing
        if (Input.GetKeyDown(togglePause))
        {
            if(pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
            }
        }
        // OtherMenu
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
    }
    //Here is wherre the player movement is calculated
    //Crouching, sprinting, and walking
    void PlayerMovement()
    {
        rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Switch to crouching
        if (crouching == true)
        {
            currentMoveSpeed = crouchSpeed;
        }

        //Switch to sprinting
        else if (grounded && Input.GetKey(KeyCode.LeftShift) && cannotSprint == false && crouching == false)
        {
            sprintTime -=  1 * Time.fixedDeltaTime;

            currentMoveSpeed = sprintSpeed;
            sprinting = true;

            if (sprintTime <= 0)
            {
                cannotSprint = true;
                sprinting = false;
                currentMoveSpeed = walkSpeed;
            }
            else
            {
                sprinting = true;
                cannotSprint = false;
            }
        }

        /*if (!Input.GetKey(KeyCode.LeftShift) && grounded && crouching == false && sprinting == false)
        {
            currentMoveSpeed = walkSpeed;
        }*/

        //Walking Boolean
        if (walking)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }

        //Recharges sprint when not sprinting
        if (sprintTime < maxSprintTime && sprinting == false)
        {
            sprintTime += 1 * Time.fixedDeltaTime;
        }
    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit velocity
        if (flatVel.magnitude > currentMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentMoveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        readyToJump = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}



