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
    private AudioSource midSource;

    [SerializeField]
    private AudioSource highSource;

    private GameObject[] bassLights;

    //private float volume = 0.8f; // Negotiate this a little bit

    // Start is called before the first frame update
    void Start()
    {
        lowSource.clip = soundfont;
        lowSource.Play();

        midSource.clip = soundfont;
        midSource.Play();

        highSource.clip = soundfont;
        highSource.Play();

        bassLights = GameObject.FindGameObjectsWithTag("BassLight");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(Chord chord)
    {
        float root = chord.Bass;
        float distance = ((float)chord.BassName + 0.5f) / 12;

        lowSource.pitch = root / 8; // This really should be 16 in the real room but idk yet
        midSource.pitch = root / 4;
        highSource.pitch = root / 2;

        lowSource.volume = Mathf.Lerp( 0f, 2f / 3f, distance);
        midSource.volume = Mathf.Lerp(2f / 3f, 1f, 1f - 2f * Mathf.Abs(0.5f - distance));
        highSource.volume = Mathf.Lerp(2f / 3f, 0f, distance);

        foreach (GameObject light in bassLights)
        {
            light.GetComponent<BassLights>().ChangeColor(ColorPicker.GetColor(chord.BassName));
            //Debug.Log("Updating Bass Light");
        }
    }


    public void StopAudio()
    {
        lowSource.volume = 0f;
        midSource.volume = 0f;
        highSource.volume = 0f;

        foreach (GameObject light in bassLights)
        {
            light.GetComponent<BassLights>().ChangeColor(Color.black);
            //Debug.Log("Updating Bass Light");
        }
    }

}
