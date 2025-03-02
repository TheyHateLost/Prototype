using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class enemyController : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime, sightDistance, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime;
    public bool walking, chasing;
    public Transform player;
    Transform currentDest;
    Vector3 dest;
    public Vector3 rayCastOffset;
    public string deathScene;
    public float aiDistance;
    public GameObject hideText, stopHideText; 

    void Start()
    {
        walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }
    void Update()
    {
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
            //its destination becomes the player 
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            //aiAnim.ResetTrigger("walk");
            //aiAnim.ResetTrigger("idle");
            //aiAnim.SetTrigger("sprint");
            //distance between player and enemy
            //if enemy catches player
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
        //if not chasing player
        if (walking == true)
        {
            //Destination becomes the positions set and anims play
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            //aiAnim.ResetTrigger("sprint");
            //aiAnim.ResetTrigger("idle");
            //aiAnim.SetTrigger("walk");
            //if reached destination, become idle there then choose next destination randomly
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
}
