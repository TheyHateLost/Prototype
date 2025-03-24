using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Player_Walking,
    Player_Sprinting,
    Player_Crouching,
    Monster_Walking,
    Monster_Running,
    Monster_SeesPlayer,
}
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        //AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        //AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        //instance.audioSource.PlayOneShot(randomClip, volume);

        //instance.audioSource.PlayOneShot(clips, volume);
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
