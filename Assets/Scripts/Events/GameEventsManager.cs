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

    private void Awake()
    {
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
}
