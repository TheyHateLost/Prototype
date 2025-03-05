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
    private float originalTimeScale;
    public gameState currentState;

    public enum gameState
    {
        Paused,
        InMenu,
        Normal
    }

    private void Awake()
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
            currentState = gameState.Normal;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case gameState.Normal:
                ResumeTime();
                break;
            case gameState.Paused:
                StopTime();
                break;
            case gameState.InMenu:
                CursorModeOn();
                break;
        }
    }
    //Time stops, cursor is usable, and pause menu comes up
    public void StopTime()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    //Time resumes, cursor goes away, and pause menu goes away
    public void ResumeTime()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = originalTimeScale;
    }
    //Cursor and menu goes away
    public void CursorModeOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
