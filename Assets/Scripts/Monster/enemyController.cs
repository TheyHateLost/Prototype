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
    public bool walking, chasing;
    public static bool endGame;

    public Animator aiAnim;
    public GameObject hideText, stopHideText;

    [Header("Timers")]
    float walkingAudio_Timer = 0f;
    float runningAudio_Timer = 0f;

    void Start()
    {
        walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }
    void Update()
    {
        extraRotation();
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        aiDistance = Vector3.Distance(player.position, this.transform.position);

        //rayCast for detecting the player(its sight)
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            //if it sees the player
            if (hit.collider.gameObject.tag == "Player")
            {
                //start chasing and initiate a chase
                walking = false;
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

        //if chasing the player
        if (chasing == true)
        {
            chasePlayer();
        }

        //if not chasing player
        if (walking == true)
        {
            walkAround();

            //Walking Sound
            walkingAudio_Timer -= Time.deltaTime;

            if (walkingAudio_Timer <= 0 && walking)
            {
                SoundManager.PlaySound(SoundType.Monster_Walking);
                walkingAudio_Timer = 0.5f;
            }
        }

        if (endGame == true)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            walking = false;
            chasing = false;
        }
    }

    public void chasePlayer()
    {
        //its destination becomes the player and speed changes
        dest = player.position;
        ai.destination = dest;
        ai.speed = chaseSpeed;

        //RunningAudio();

        //aiAnim.ResetTrigger("walk");
        //aiAnim.ResetTrigger("idle");
        //aiAnim.SetTrigger("sprint");

        //distance between player and enemy
        if (aiDistance <= catchDistance)
        {
            //player dies and plays jumpscare then loads selected scene 
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
        //Destination becomes the positions set and anims play
        dest = currentDest.position;
        ai.destination = dest;
        ai.speed = walkSpeed;

        //Animations
        //aiAnim.ResetTrigger("sprint");
        //aiAnim.ResetTrigger("idle");
        //aiAnim.SetTrigger("walk");

        //Reached destination, idle there then choose next destination randomly
        if (ai.remainingDistance <= ai.stoppingDistance + 0.1f)
        {
            //aiAnim.ResetTrigger("sprint");
            //aiAnim.ResetTrigger("walk");
            //aiAnim.SetTrigger("idle");
            ai.speed = 0;
            StopCoroutine("stayIdle");
            StartCoroutine("stayIdle");
            walking = false;
        }
    }



    public void stopChase()
    {
        walking = true;
        chasing = false;
        StopCoroutine("chaseRoutine");
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    void extraRotation()
    {
        Vector3 lookrotation = ai.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
    }

    //Routines
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

        //Running sound
        if (runningAudio_Timer <= 0 && walking)
        {
            //monsterRunning.PlayOneShot(running_monsterSound);
            runningAudio_Timer = 4f;
        }
    }
}
