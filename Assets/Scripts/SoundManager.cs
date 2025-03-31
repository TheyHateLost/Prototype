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
    Player_Walking = 0,
    Player_Sprinting = 1,
    Player_Crouching = 2,
    Monster_Wandering = 3,
    Monster_Chasing = 4,
    Monster_SeesPlayer = 5,
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
    private AudioSource audioSource_old;
    [SerializeField] private AudioSource[] audioSource;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource[]>();
    }

    public static void PlaySound(SoundSource source, SoundType sound, float volume = 1, float pitch = 1)
    {
        //AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        //AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        //instance.audioSource.PlayOneShot(randomClip, volume);

        //instance.audioSource.PlayOneShot(clips, volume);



        //Debug.Log("SOUND: " + sound + " / " + instance.soundList[(int)sound]);
        //instance.audioSource.pitch = pitch;
        //instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
        instance.audioSource[(int)source].pitch = pitch;
        instance.audioSource[(int)source].PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
