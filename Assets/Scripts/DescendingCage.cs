using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
//using static StateRotation;
using System.Linq;

public class DescendingCage: MonoBehaviour
{
    //Will NOT MOVE at start, activated by player collision, waits only ATSTOP until player leaves 
    //Will MOVE between destinations (back and forth) as long as player is on it
    //Will CONTINUE on its path when player leaves, then RETURN

    public List<Vector3> Destinations;
    private int CurrentDest;
    public float Speed = 0.1f;
    public float returnSpeed = 0.5f;
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
                PlatformActive();
                break;
            case platformMode.RETURN:
                PlatformMoveBack();
                break;
            case platformMode.ATSTOP:
                if (playerIsOn == false)
                {
                    destTimer -= Time.deltaTime;
                    if (destTimer <= 0)
                    {
                        CurrentDest = 0;
                        platMode = platformMode.RETURN;
                    }
                }
                break;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
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
        platMode = platformMode.MOVING;

        playerIsOn = true;
    }

    private void OnCollisionExit(Collision other)
    {
        Riders.Remove(other.transform);
        playerIsOn = false;

        //Call the elevator back after a certain amount of time
        Invoke("SetState2AtStop", elevatorCallBackSpeed);
    }

    void PlatformActive()
    {
        if (Destinations.Count == 0) return;
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, Speed);
        Vector3 movement = transform.position - old;
        Vector3 lastStop = Destinations.Last();
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }

        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;
        }
        if (Vector3.Distance(transform.position, lastStop) < 0.01f)
        {
            destTimer = DestTimer;
            platMode = platformMode.ATSTOP;
        }
    }
    //Returns platform to its original destination(reverses the list of destinations)
    void PlatformMoveBack()
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

    //Bugfix: sets platform back to ATSTOP when player gets back on platform when platform is at its destination
    void SetState2AtStop()
    {
        destTimer = DestTimer;
        platMode = platformMode.ATSTOP;
    }
}
