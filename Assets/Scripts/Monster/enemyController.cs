using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class enemyController : MonoBehaviour
{
//https://www.youtube.com/watch?v=DU7cgVsU2rM&t=7s

    [Header("AI Pathing")]
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Transform player;
    public Vector3 rayCastOffset;
    Transform currentDest;
    Vector3 dest;
    public float aiDistance;
    float extraRotationSpeed;

    [Header("Monster Stats")]
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime, sightDistance, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime;
    public string deathScene;

    [Header("Booleans")]
    public bool wandering, chasing;
    public static bool endGame;

    public Animator aiAnim;
    public GameObject hideText, stopHideText;

    [Header("Timers")]
    float walkingAudio_Timer = 0f;
    float runningAudio_Timer = 0f;

    void Start()
    {
        wandering = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }
    void Update()
    {
        extraRotation();
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        aiDistance = Vector3.Distance(player.position, this.transform.position);

        // Player Detection RayCast
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            // Monster sees player
            if (hit.collider.gameObject.tag == "Player")
            {
                // Initiate Chase
                wandering = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
                chasing = true;
            }
            else
            {
                chasing = false;
            }
        }

        // State - Chasing the player
        if (chasing == true)
        {
            chasePlayer();
        }

        // State - Wandering the map
        if (wandering == true)
        {
            walkAround();

            //Walking Sound
            walkingAudio_Timer -= Time.deltaTime;

            if (walkingAudio_Timer <= 0)
            {
                SoundManager.PlaySound(SoundType.Monster_Wandering, 1f);
                walkingAudio_Timer = 0.5f;
            }
        }

        if (endGame == true)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            wandering = false;
            chasing = false;
        }
    }

    public void chasePlayer()
    {
        // Change speed and destination becomes player
        dest = player.position;
        ai.destination = dest;
        ai.speed = chaseSpeed;

        //RunningAudio();

        //aiAnim.ResetTrigger("walk");
        //aiAnim.ResetTrigger("idle");
        //aiAnim.SetTrigger("sprint");

        // Distance between monster and player - (Monster catches player)
        if (aiDistance <= catchDistance)
        {
            // Kill player and play jumpscare 
            player.gameObject.SetActive(false);
            //aiAnim.ResetTrigger("walk");
            //aiAnim.ResetTrigger("idle");
            hideText.SetActive(false);
            stopHideText.SetActive(false);
            //aiAnim.ResetTrigger("sprint");
            //aiAnim.SetTrigger("jumpscare");
            StartCoroutine(deathRoutine());
            chasing = false;
        }
    }
    public void walkAround()
    {
        // Change speed and Wander to determined destination
        dest = currentDest.position;
        ai.destination = dest;
        ai.speed = walkSpeed;

        //Animations
        //aiAnim.ResetTrigger("sprint");
        //aiAnim.ResetTrigger("idle");
        //aiAnim.SetTrigger("walk");

        // Reached destination, idle there then choose next destination randomly
        if (ai.remainingDistance <= ai.stoppingDistance + 0.1f)
        {
            //aiAnim.ResetTrigger("sprint");
            //aiAnim.ResetTrigger("walk");
            //aiAnim.SetTrigger("idle");
            ai.speed = 0;
            StopCoroutine("stayIdle");
            StartCoroutine("stayIdle");
            wandering = false;
        }
    }

    public void stopChase()
    {
        wandering = true;
        chasing = false;
        StopCoroutine("chaseRoutine");
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        wandering = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    void extraRotation()
    {
        Vector3 lookrotation = ai.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
    }

    // Routines
    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        stopChase();
    }

    IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }
    public void RunningAudio()
    {
        runningAudio_Timer -= Time.fixedDeltaTime;

        // Running sound
        if (runningAudio_Timer <= 0 && wandering)
        {
            //monsterRunning.PlayOneShot(running_monsterSound);
            runningAudio_Timer = 4f;
        }
    }
}
