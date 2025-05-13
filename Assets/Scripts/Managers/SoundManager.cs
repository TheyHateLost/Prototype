using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    //The numbers are the index of the sound in the soundList array

    //Player Sounds
    Player_Footsteps = 0,

    //Monster Sounds
    Monster_Wandering = 1,
    Monster_SpottedPlayer = 2,

    //Ambient / Other Sounds (played by the sound manager)
    Background_Ambience_V1 = 3,
    Item_Pickup = 4,
    Humming_Lights = 5,

    //Task Sounds

}
public enum SoundSource
{
    Player = 0,
    Monster = 1,
    SoundManager = 2,
    FuseBox = 3,
    Facility_Lights = 4,
}
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    [SerializeField] private AudioSource[] audioSource;
    float ambienceSound_Timer = 12f;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(SoundSource source, SoundType sound, float volume = 1, float pitch = 1)
    {
        instance.audioSource[(int)source].pitch = pitch;
        instance.audioSource[(int)source].PlayOneShot(instance.soundList[(int)sound], volume);
    }
    void Start()
    {
        PlaySound(SoundSource.SoundManager, SoundType.Background_Ambience_V1, 0.1f, Random.Range(0.9f, 1.1f));
    }

    void Update()
    {
        //Sound Timer
        ambienceSound_Timer -= Time.deltaTime;

        if (ambienceSound_Timer <= 0f)
        {
            PlaySound(SoundSource.SoundManager, SoundType.Background_Ambience_V1, 0.1f, Random.Range(0.9f, 1.1f));
            ambienceSound_Timer = 12f;
        }
    }
}
