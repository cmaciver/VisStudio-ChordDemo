using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    private bool testing;

    [SerializeField]
    private int wallNumber;

    [SerializeField]
    private AudioClip soundfont;

    [SerializeField]
    private AudioSource[] sources;

    private AudioSource currentSource;

    private bool currentIsFirst = true;

    private float[] volume = {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };

    private bool primed = true;

    Color destinationColor;

    private Material mat;


    // Start is called before the first frame update
    void Start()
    {
        currentSource = sources[0];

        foreach (AudioSource source in sources)
        {
            source.clip = soundfont;
            source.Play();
        }

        mat = GetComponent<Renderer>().material;

        if (!testing)
        {
            GetComponentInChildren<TextMeshProUGUI>().alpha = 0;
        }

        // enharmonic calculator test here ?
    }

    // Update is called once per frame
    void Update()
    {
        mat.color = Color.Lerp(mat.color, destinationColor, 0.05f);
    }

    public void PlayPrimed(float[] pitchMult, Note.Name[] pitchNames, Chord chord) // dont use the chord unless you have too
    {
        if (primed)
        {
            PlayAudio(pitchMult);
            destinationColor = ColorPicker.GetColor(pitchNames[wallNumber]);
            GetComponentInChildren<TextMeshProUGUI>().text = getEnharmonic(pitchNames[wallNumber], chord) + getOctave();// NEW
        }

        primed = false;
    }

    private int getOctave()
    {
        return (int)Math.Floor(Math.Log(currentSource.pitch, 2)+0.0001) + 4; // float point moment
    } 

    private string getEnharmonic(Note.Name pitchName, Chord chord) // MAKE BETTER LATER (OR DONT)
    {
        String note = Enum.GetName(typeof(Note.Name),pitchName); // maybe casting to string is unneccesary, but it's easier for now
        Note.Name root = chord.RootName;
        switch (root)
        {
            case Note.Name.G:
            case Note.Name.Gb:
                if (chord.ThirdType == Chord.ThirdTypeE.Major) { // maybe we don't need all of this
                    if (note.Equals("Db")) note = "C#";
                    else if (note.Equals("Eb")) note = "D#";
                    else if (note.Equals("Gb")) note = "F#";
                    else if (note.Equals("Ab")) note = "G#";
                    else if (note.Equals("Bb")) note = "A#";
                }
                break;
            case Note.Name.D:
            case Note.Name.A:
            case Note.Name.E:
            case Note.Name.B:
                if (note.Equals("Db")) note = "C#";
                else if (note.Equals("Eb")) note = "D#";
                else if (note.Equals("Gb")) note = "F#";
                else if (note.Equals("Ab")) note = "G#";
                else if (note.Equals("Bb")) note = "A#";
                break;
        }
        return note;
    }

    public void PlayAudio(float[] pitchMult)
    {
        StopAllCoroutines();
        //source.Play();
        if (sources[0].volume > sources[1].volume)
        {
            currentSource = sources[0];
            currentIsFirst = true;
        }
        else
        {
            currentSource = sources[1];
            currentIsFirst = false;
        }

        StartCoroutine(FadeAudioSource.StartFade(currentSource, 0.5f, 0));

        currentSource = currentIsFirst ? sources[1] : sources[0];
        currentIsFirst = !currentIsFirst;

        currentSource.pitch = pitchMult[wallNumber];
        StartCoroutine(FadeAudioSource.StartFade(currentSource, 0.5f, volume[wallNumber]));
    }

    public void Reprime()
    {
        primed = true;
    }

    public void StopAudio()
    {
        //update the things on the wall itself
        destinationColor = Color.black;
        GetComponentInChildren<TextMeshProUGUI>().text = "";   // NEW

        StopAllCoroutines();
        StartCoroutine(FadeAudioSource.StartFade(currentSource, .25f, 0));
        currentSource = currentIsFirst ? sources[1] : sources[0];
        currentIsFirst = !currentIsFirst;
        StartCoroutine(FadeAudioSource.StartFade(currentSource, .25f, 0));
    }

    public void StopPrimed()
    {
        if (primed)
        {
            StopAudio();
        }

        primed = false;
    }

    // code borrowed from online, fades audio out
    // use:
    // StartCoroutine(FadeAudioSource.StartFade(AudioSource audioSource, float duration, float targetVolume));
    public static class FadeAudioSource
    {
        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;
            float diff = targetVolume - start;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = start + diff * Mathf.Sqrt(currentTime / duration); // curr / duration is lerp(0,1)
                yield return null;
            }
            yield break;
        }

    }
}
