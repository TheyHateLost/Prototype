using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public GameObject elevator;
    public GameObject player;
    public GameObject winning;
    //enemyController monsterAI;
    public GameObject monster;
    public GameObject allTasksCompleted;
    DescendingCage elevatorScript;

    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public QuestEvents questEvents;
    public MiscEvents miscEvents;
    public GoldEvents goldEvents;

    public GameObject PauseMenu;
    public GameObject KeypadMenu, KeypadMenu1, KeypadMenu2, KeypadMenu3;
    float originalTimeScale;

    public int tasksRemaining = 0;
    public Text numberOfTasksLeft;

    public enum gameState
    {
        Paused,
        InMenu,
        Normal
    }
    public gameState currentState;

    void Awake()
    {
        originalTimeScale = Time.timeScale;
        currentState = gameState.Normal;

        //elevatorScript = elevator.GetComponent<DescendingCage>();

        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

        // initialize all events
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
        questEvents = new QuestEvents();
        goldEvents = new GoldEvents();
        miscEvents = new MiscEvents();
    }

    void Update()
    {
        numberOfTasksLeft.text = tasksRemaining.ToString();

        //Debug.Log(tasksRemaining);

        if ((KeypadMenu.activeInHierarchy) || (KeypadMenu1.activeInHierarchy) || (KeypadMenu2.activeInHierarchy) || (KeypadMenu3.activeInHierarchy))
        {
            currentState = gameState.InMenu;
        }
        else if (PauseMenu.activeInHierarchy)
        {
            currentState = gameState.Paused;
        }
        else
        {
            ResumeTime();
            currentState = gameState.Normal;
        }

        if (tasksRemaining <= 0)
        {
            //elevatorScript = new DescendingCage();
            winning.SetActive(true);
            allTasksCompleted.SetActive(true);
            enemyController.endGame = true;
            //elevatorScript.platMode = DescendingCage.platformMode.MOVING;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case gameState.Paused:
                StopTime();
                break;
            case gameState.InMenu:
                CursorModeOn();
                break;
            case gameState.Normal:
                break;
        }
    }
    //Time stops, cursor is usable, and pause menu comes up
    public void StopTime()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
    //Time resumes, cursor goes away, and pause menu goes away
    public void ResumeTime()
    {
        Time.timeScale = originalTimeScale;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    //Cursor and menu goes away
    public void CursorModeOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
