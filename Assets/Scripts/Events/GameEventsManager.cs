using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public QuestEvents questEvents;
    public MiscEvents miscEvents;
    public GoldEvents goldEvents;

    public GameObject PauseMenu;
    public GameObject KeypadMenu;
    float originalTimeScale;

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
        if (KeypadMenu.activeInHierarchy)
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
