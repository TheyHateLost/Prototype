using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using System.Linq;

public class Elevator: MonoBehaviour
{
    //Platform destinations
    public List<Vector3> Destinations;
    private int CurrentDest;

    //Platform Speeds
    public float Speed = 0.1f;
    public float returnSpeed = 0.5f;

    //List of players/objects that will stay on the platform
    private List<Transform> Riders = new List<Transform>();

    private bool playerIsOn = false;
    public float DestTimer = 4;
    float destTimer;
    public float elevatorCallBackSpeed = 2f;
    public enum platformMode
    {
        IDLE,
        MOVING,
        RETURN,
        ATSTOP
    }
    private void Start()
    {
        destTimer = DestTimer;
        platMode = platformMode.IDLE;
    }
    public platformMode platMode;

    void FixedUpdate()
    {
        //Different modes the platform will switch through
        switch (platMode)
        {
            case platformMode.IDLE:
                break;
            case platformMode.MOVING:
                MoveToDestination();
                break;
            case platformMode.RETURN:
                //PlatformMoveBack();
                break;
            case platformMode.ATSTOP:
                if (playerIsOn == false)
                {
                    destTimer -= Time.deltaTime;
                    if (destTimer <= 0)
                    {
                        CurrentDest = 0;
                        platMode = platformMode.MOVING;
                    }
                }
                break;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        playerIsOn = true;

        if (!Riders.Contains(other.transform))
            Riders.Add(other.transform);

        if (platMode == platformMode.RETURN)
        {
            CurrentDest++;
            if (CurrentDest >= Destinations.Count)
            {
                CurrentDest = 0;
            }
        }
        //platMode = platformMode.MOVING;
    }

    void OnCollisionExit(Collision other)
    {
        playerIsOn = false;
        Riders.Remove(other.transform);

        //Calls platform back if player is not on it 
        Invoke("SetState2AtStop", elevatorCallBackSpeed);
    }

    void MoveToDestination()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, Speed);
        Vector3 movement = transform.position - old;

        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }
        
        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;
        }

    }
    void ReturnDest()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, returnSpeed);
        Vector3 movement = transform.position - old;

        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }

        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest--;
            if (CurrentDest < 0)
            {
                CurrentDest = 0;
                platMode = platformMode.IDLE;
            }
        }
    }


    //Bugfix: sets state to ATSTOP when player gets back on platform when reached the destination
    void SetState2AtStop()
    {
        if (playerIsOn == false)
        {
            destTimer = DestTimer;
            platMode = platformMode.ATSTOP;
        }
    }
}
