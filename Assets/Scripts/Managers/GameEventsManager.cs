using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameEventsManager : MonoBehaviour
{
    [Header("GameObjects")]
    public static GameEventsManager instance { get; private set; }
    [SerializeField] GameObject elevator;
    [SerializeField] GameObject WinningArea;
    [SerializeField] GameObject Monster;
    [SerializeField] Text numberOfTasksLeft;

    [Header("EndGame")]
    [SerializeField] GameObject LeaveLevelPromptUI;
    [SerializeField] GameObject LeaveLevelCode;

    [Header("UI")]
    public GameObject PauseMenu;
    [SerializeField] GameObject[] TaskUI;

    MonsterController monsterScript;
    DescendingCage elevatorScript;

    public static bool PlayerInMenu;
    float originalTimeScale;

    [Header("Tasks")]
    public static int tasksRemaining = 4;
    [SerializeField] GameObject[] Green_TaskLight;
    [SerializeField] GameObject[] Red_TaskLight;

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
        monsterScript = Monster.GetComponent<MonsterController>();

        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;
    }

    void Update()
    {
        if (numberOfTasksLeft != null)
        numberOfTasksLeft.text = tasksRemaining.ToString();

        if (PlayerInMenu == false)
        {
            if (PauseMenu.activeInHierarchy && PauseMenu != null)
            {
                currentState = gameState.Paused;
            }
            else
            {
                ResumeTime();
                currentState = gameState.Normal;
            }
        }

        foreach (GameObject taskUI in TaskUI)
        {
            if (taskUI != null && taskUI.activeInHierarchy)
            {
                PlayerInMenu = true;
                currentState = gameState.InMenu;
                break;
            }
            else
            {
                PlayerInMenu = false;
            }
        }

        //Handles the task lights and the end game situation
        TaskManager();
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
    void TaskManager()
    {
        if (tasksRemaining == 3)
        {
            Red_TaskLight[0].SetActive(false);
            Green_TaskLight[0].SetActive(true);
        }
        if (tasksRemaining == 2)
        {
            Red_TaskLight[1].SetActive(false);
            Green_TaskLight[1].SetActive(true);
        }
        if (tasksRemaining == 1)
        {
            Red_TaskLight[2].SetActive(false);
            Green_TaskLight[2].SetActive(true);
        }
        if (tasksRemaining <= 0)
        {
            Red_TaskLight[3].SetActive(false);
            Green_TaskLight[3].SetActive(true);

            LeaveLevelCode.SetActive(true);
            WinningArea.SetActive(true);
            LeaveLevelPromptUI.SetActive(true);
            MonsterController.endGame = true;
            //elevatorScript.platMode = DescendingCage.platformMode.MOVING;
        }
    }
}
