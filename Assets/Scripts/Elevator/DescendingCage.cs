using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using System.Linq;

public class DescendingCage: MonoBehaviour
{
    //Platform destinations
    public List<Vector3> Destinations;
    int CurrentDest;

    //Platform Speeds
    public float Speed = 0.1f;
    public float returnSpeed = 0.5f;

    //List of players/objects that will stay on the platform
    private List<Transform> Riders = new List<Transform>();
    [SerializeField] Animator InsideElevatorDoorAnimator;
    [SerializeField] Animator OutsideElevatorDoorAnimator;

    [SerializeField] bool playerIsOn = false;

    float elevatorCallBackSpeed = 2.5f;
    float elevatorCallBackTimer = 3f;
    public enum platformMode
    {
        IDLE,
        MOVING,
        RETURN,
        ATSTOP
    }
    void Start()
    {
        OutsideElevatorDoorAnimator.enabled = false;
        InsideElevatorDoorAnimator.enabled = false;

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
                StartCoroutine(CloseElevatorDoors());
                Invoke("PlatformMoveBack", elevatorCallBackSpeed);
                //PlatformMoveBack();
                break;
            case platformMode.ATSTOP:

                OutsideElevatorDoorAnimator.enabled = true;
                InsideElevatorDoorAnimator.enabled = true;
                StartCoroutine(OpenElevatorDoors());

                if (playerIsOn == false)
                {
                    elevatorCallBackTimer -= Time.fixedDeltaTime;
                    if (elevatorCallBackTimer <= 0)
                    {
                        CurrentDest = 0;
                        platMode = platformMode.RETURN;
                    }
                }
                else if (playerIsOn == true)    
                {
                    elevatorCallBackTimer = 3f;
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
        if (platMode != platformMode.ATSTOP)
        {
            platMode = platformMode.MOVING;
        }
    }

    void OnCollisionExit(Collision other)
    {
        playerIsOn = false;
        Riders.Remove(other.transform);
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

    IEnumerator OpenElevatorDoors()
    {
        OutsideElevatorDoorAnimator.Play("OpenOutsideElevatorDoor");
        yield return new WaitForSeconds(0.5f);
        InsideElevatorDoorAnimator.Play("OpenElevatorDoor");
    }
    IEnumerator CloseElevatorDoors()
    {
        OutsideElevatorDoorAnimator.Play("CloseOutsideElevatorDoor");
        yield return new WaitForSeconds(0.5f);
        InsideElevatorDoorAnimator.Play("CloseElevatorDoor");
    }
}
