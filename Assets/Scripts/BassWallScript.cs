using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BassWallScript : MonoBehaviour {
    [SerializeField]
    private AudioClip soundfont;

    [SerializeField]
    private AudioSource lowSource;

    [SerializeField]
    private AudioSource medSource;

    [SerializeField]
    private AudioSource highSource;

    //private float volume = 0.8f; // Negotiate this a little bit

    // Start is called before the first frame update
    void Start()
    {
        lowSource.clip = soundfont;
        lowSource.Play();

        medSource.clip = soundfont;
        medSource.Play();

        highSource.clip = soundfont;
        highSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(Chord chord) // FIX THIS STILL
    {
        float root = chord.Root;
        float distance = (float)chord.RootName / 12;

        lowSource.pitch = root / 8;
        medSource.pitch = root / 4;
        highSource.pitch = root / 2;

        lowSource.volume = Mathf.Lerp( 0f, 1f, distance);
        medSource.volume = 1;
        highSource.volume = Mathf.Lerp(1f, 0f, distance);
    }


    public void StopAudio()
    {
        lowSource.volume = 0f;
        highSource.volume = 0f;
    }

}
