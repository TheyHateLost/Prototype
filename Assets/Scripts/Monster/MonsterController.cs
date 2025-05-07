using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour
{
    [Header("AI Pathing")]
    public Vector3 rayCastOffset;
    Transform currentDest;
    float aiDistance;
    float extraRotationSpeed;

    [Header("Monster Stats")]
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, sightDistance, catchDistance, minChaseTime, maxChaseTime, jumpscareTime;
    float idleTime, chaseTime;
    public string deathScene;

    [Header("Booleans")]
    [HideInInspector] public static bool wandering, chasing, endGame, IgnorePlayer = false;
    bool playerFound = true;

    [Header("Timers")]
    [SerializeField] float walkingAudio_Timer = 0.5f;
    [SerializeField] float runningAudio_Timer = 0f;

    [Header("References")]
    [SerializeField] NavMeshAgent monsterAI;
    [SerializeField] Transform player;
    [SerializeField] List<Transform> PatrolDestinations;
    [SerializeField] GameObject EnterHide_Text, ExitHide_Text;
    [HideInInspector] public static Vector3 target;

    void Start()
    {
        wandering = true;
        if(PatrolDestinations.Count > 0)
            currentDest = PatrolDestinations[Random.Range(0, PatrolDestinations.Count)];
    }
    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        aiDistance = Vector3.Distance(player.position, this.transform.position);

        // Player Detection RayCast
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            // Monster sees player
            if (hit.collider.gameObject.tag == "Player" && IgnorePlayer == false)
            {
                // Initiate Chase
                target = player.position;
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
            if (playerFound == true)
            {
                SoundManager.PlaySound(SoundSource.Monster, SoundType.Monster_SpottedPlayer, 1f);
                playerFound = false;
            }

            // Change speed and destination becomes player
            monsterAI.destination = target;
            monsterAI.speed = chaseSpeed;

            // Distance between monster and player - (Monster catches player)
            if (aiDistance <= catchDistance && IgnorePlayer == false)
            {
                // Kill player
                player.gameObject.SetActive(false);
                EnterHide_Text.SetActive(false);
                ExitHide_Text.SetActive(false);
                StartCoroutine(deathRoutine());
                chasing = false;
            }
        }

        // State - Wandering the map
        if (wandering == true)
        {
            if (playerFound == false)
            {
                playerFound = true;
            }

            //Walking Sound
            walkingAudio_Timer -= Time.deltaTime;

            if (walkingAudio_Timer <= 0f) 
            {
                SoundManager.PlaySound(SoundSource.Monster, SoundType.Monster_Footsteps, 0.6f, Random.Range(0.5f, 1.1f));
                walkingAudio_Timer = 0.6f;
            }

            // Change speed and Wander to determined destination
            if (currentDest != null)
            {
                target = currentDest.position;
                monsterAI.destination = target;
                monsterAI.speed = walkSpeed;
            }

            // Reached destination, idle there then choose next destination randomly
            if (monsterAI.remainingDistance <= monsterAI.stoppingDistance + 0.1f)
            {
                monsterAI.speed = 0;
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");
                wandering = false;
            }
        }
        // When tasks complete - know where player is and hunt them
        if (endGame == true)
        {
            target = player.position;
            monsterAI.destination = target;
            monsterAI.speed = chaseSpeed;
            wandering = false;
            chasing = false;
        }
    }

    public void stopChase()
    {
        wandering = true;
        chasing = false;
        StopCoroutine("chaseRoutine");
        currentDest = PatrolDestinations[Random.Range(0, PatrolDestinations.Count)];
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        wandering = true;
        if(PatrolDestinations.Count > 0)
            currentDest = PatrolDestinations[Random.Range(0, PatrolDestinations.Count)];
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
