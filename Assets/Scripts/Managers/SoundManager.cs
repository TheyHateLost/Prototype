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
    Player_Heartbeat = 1,

    //Monster Sounds
    Monster_Wandering = 2,
    Monster_SpottedPlayer = 3,

    //Ambient / Other Sounds (played by the sound manager)
    Background_Ambience_V1 = 4,
    Item_Pickup = 5,

}
public enum SoundSource
{
    Player = 0,
    Monster = 1,
    SoundManager = 2,
}
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    [SerializeField] private AudioSource[] audioSource;
    float ambienceSound_Timer = 0f;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(SoundSource source, SoundType sound, float volume = 1, float pitch = 1, float maxDistance = 1)
    {
        instance.audioSource[(int)source].pitch = pitch;
        instance.audioSource[(int)source].maxDistance = maxDistance;
        instance.audioSource[(int)source].PlayOneShot(instance.soundList[(int)sound], volume * 0.65f);
    }

    void Update()
    {
        //Sound Timer
        ambienceSound_Timer -= Time.deltaTime;

        if (ambienceSound_Timer <= 0f)
        {
            PlaySound(SoundSource.SoundManager, SoundType.Background_Ambience_V1, 0.0875f, Random.Range(0.8f, 1.35f));
            ambienceSound_Timer = 11f;
        }
    }
}
