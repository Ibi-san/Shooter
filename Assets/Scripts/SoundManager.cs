using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _headshotSFX;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void PlayHeadshot() => _audio.PlayOneShot(_headshotSFX);
}
