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
    private AudioSource highSource;

    private GameObject[] bassLights;

    //private float volume = 0.8f; // Negotiate this a little bit

    // Start is called before the first frame update
    void Start()
    {
        lowSource.clip = soundfont;
        lowSource.Play();

        highSource.clip = soundfont;
        highSource.Play();

        bassLights = GameObject.FindGameObjectsWithTag("BassLight");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(Chord chord) // FIX THIS STILL
    {
        float root = chord.Root;
        float distance = ((float)chord.RootName + .5f) / 12;

        lowSource.pitch = root / 8;
        highSource.pitch = root / 4;

        lowSource.volume = Mathf.Lerp( 0f, 1f, distance); 
        highSource.volume = Mathf.Lerp(1f, 0f, distance);

        foreach (GameObject light in bassLights)
        {
            light.GetComponent<BassLights>().ChangeColor(ColorPicker.GetColor(chord.RootName));
            //Debug.Log("Updating Bass Light");
        }
    }


    public void StopAudio()
    {
        lowSource.volume = 0f;
        highSource.volume = 0f;

        foreach (GameObject light in bassLights)
        {
            light.GetComponent<BassLights>().ChangeColor(Color.black);
            //Debug.Log("Updating Bass Light");
        }
    }

}
