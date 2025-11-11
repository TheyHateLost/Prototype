using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class MonsterController : MonoBehaviour
{
    [Header("AI Pathing")]
    public Vector3 rayCastOffset;
    Transform currentDest;
    float aiDistance;
    float extraRotationSpeed;

    [Header("Monster Stats")]
    float currentSpeed;
    public float normalSpeed, angrySpeed, enragedSpeed;
    public float chaseSpeedMultiplier;

    public float minIdleTime, maxIdleTime, sightDistance, catchDistance, minChaseTime, maxChaseTime, jumpscareTime;
    float idleTime, chaseTime;
    public string deathScene;

    [Header("Booleans")]
    public bool IgnorePlayer = false;
    //bool playerFound = true;

    [Header("Timers")]
    [SerializeField] float wanderingAudioTimer = 9.5f;

    [Header("References")]
    [SerializeField] NavMeshAgent monsterAI;
    [SerializeField] Transform player;
    [SerializeField] List<Transform> PatrolDestinations;
    [SerializeField] GameObject EnterHide_Text, ExitHide_Text;
    [HideInInspector] public static Vector3 target;

    public TargetState targetState;
    // Defines what monster is currently doing
    public enum TargetState
    {
        Wandering,
        Idle,
        Chasing,
    }
    public AggressiveState aggressiveState;
    // Defines monster speed and behavior
    public enum AggressiveState
    {
        Normal, //Starting State and Slowest Speed
        Angry,
        Enraged,
    }

    void Start()
    {
        //wandering = true;
        if(PatrolDestinations.Count > 0)
            currentDest = PatrolDestinations[Random.Range(0, PatrolDestinations.Count)];
    }
    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        aiDistance = Vector3.Distance(player.position, this.transform.position);
        SpeedHandler();
        MonsterStateHandler();

        // Player Detection RayCast
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            // Monster sees player
            if (hit.collider.gameObject.tag == "Player" && IgnorePlayer == false)
            {
                // Initiate Chase
                target = player.position;
                //wandering = false;
                targetState = TargetState.Chasing;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
            }
            else
            {
                //chasing = false;
            }
        }
    }

    public void stopChase()
    {
        targetState = TargetState.Wandering;
        //chasing = false;
        StopCoroutine("chaseRoutine");
        currentDest = PatrolDestinations[Random.Range(0, PatrolDestinations.Count)];
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        targetState = TargetState.Wandering;
        if (PatrolDestinations.Count > 0)
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
        PlayerController.TimesDied += 1;
        SceneManager.LoadScene(deathScene);
    }

    void SpeedHandler()
    {
        if (PlayerController.TimesDied >= 3)
            chaseSpeedMultiplier = 1.2f;

        monsterAI.speed = currentSpeed * chaseSpeedMultiplier;
    }

    void MonsterStateHandler()
    {
        switch (targetState)
        {
            case TargetState.Wandering:
                WanderAround();
                break;
            case TargetState.Idle:
                break;
            case TargetState.Chasing:
                ChasePlayer();
                break;
        }
        switch (aggressiveState)
        {
            case AggressiveState.Normal:
                currentSpeed = normalSpeed;  
                break;
            case AggressiveState.Angry:
                currentSpeed = angrySpeed;
                break;
            case AggressiveState.Enraged:
                currentSpeed = enragedSpeed;
                break;
        }
    }
    void WanderAround()
    {
        //Walking Sound
        wanderingAudioTimer -= Time.deltaTime;

        if (wanderingAudioTimer <= 0f)
        {
            SoundManager.PlaySound(SoundSource.Monster, SoundType.Monster_Wandering, 0.8f, Random.Range(0.75f, 1.1f), 35f);
            wanderingAudioTimer = 9.5f;
        }

        // Change speed and Wander to determined destination
        if (currentDest != null)
        {
            target = currentDest.position;
            monsterAI.destination = target;
            //monsterAI.speed = walkSpeed;
        }

        // Reached destination, idle there then choose next destination randomly
        if (monsterAI.remainingDistance <= monsterAI.stoppingDistance + 0.1f)
        {
            monsterAI.speed = 0;
            StopCoroutine("stayIdle");
            StartCoroutine("stayIdle");
            targetState = TargetState.Idle;
        }
    }

    void ChasePlayer()
    {
        SoundManager.PlaySound(SoundSource.Monster, SoundType.Monster_SpottedPlayer, 1f, 1f, 25f);
        // Change speed and destination becomes player
        monsterAI.destination = target;
        //monsterAI.speed = chaseSpeed;

        // Distance between monster and player - (Monster catches player)
        if (aiDistance <= catchDistance && IgnorePlayer == false)
        {
            // Kill player
            EnterHide_Text.SetActive(false);
            ExitHide_Text.SetActive(false);
            StartCoroutine(deathRoutine());
        }
    }
}
