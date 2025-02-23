using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //UI for dots collected
    //Better player movement(sprint, two cameras)
    //Teleporters
    //Power Ups 
    //Enemy Ai(chasing player, not getting stuck, follow path)


    [Header("PlayerStats")]
    public float speed;
    public float jumpForce;
    public int lives = 3;
    int dotsCollected;

    [Header("Jumping")]
    public float jumpCooldown;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;
    public float groundDrag;

    [Header("Keybinds")]
    public KeyCode menuKey = KeyCode.I;

    public Transform orientation;
    Vector3 spawnPoint;
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    int buildIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyToJump = true;

        spawnPoint = transform.position;
        dotsCollected = 0;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //ground check       0.5f is half the players height and 0.2f is extra length
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        speedControl();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        if (dotsCollected >= 150)
        {
            SceneManager.LoadScene(buildIndex + 1);
        }
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        //caculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        //Jump if ready and grounded
        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded) 
        {
            Jump();
        }
    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit velocity
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //give player an extra jump when colliding with a powerup
        if (collider.gameObject.tag == "Dot")
        {
            Destroy(collider.gameObject);
            dotsCollected += 1;
            Debug.Log(dotsCollected);
        }
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
