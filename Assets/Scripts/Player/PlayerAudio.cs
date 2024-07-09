using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource Walk;
    [SerializeField] AudioSource Jump;


    public enum AudioType
    {
        Walk,
        Jump
    }

    public void Play(AudioType audioType, bool playerState)
    {
        AudioSource audioSource = null;
        switch (audioType)
        {
            case AudioType.Walk:
                audioSource = Walk;
                break;
            case AudioType.Jump:
                audioSource = Jump;
                break;
        }
        if (audioSource != null)
        {
            if (playerState)
                audioSource.Play();
            else
                audioSource.Stop();
        }
    }
}