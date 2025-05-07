using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    //The numbers are the index of the sound in the soundList array
    //PlayerSounds
    Player_Footsteps = 0,
    //MonsterSounds
    Monster_Wandering = 1,
    Monster_SpottedPlayer = 2,
    //TaskSounds
    Task_Complete = 3,

    GasCan_Filling = 4,
}
public enum SoundSource
{
    Player = 0,
    Monster = 1,
}
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    [SerializeField] private AudioSource[] audioSource;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(SoundSource source, SoundType sound, float volume = 1, float pitch = 1)
    {
        //Debug.Log("SOUND: " + sound + " / " + instance.soundList[(int)sound]);
        instance.audioSource[(int)source].pitch = pitch;
        instance.audioSource[(int)source].PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
