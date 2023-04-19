using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallScript : MonoBehaviour
{
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

    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        currentSource = sources[0];

        foreach (AudioSource source in sources)
        {
            source.clip = soundfont;
            source.Play();
        }

        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.color = Color.Lerp(renderer.material.color, destinationColor, 0.02f);
    }

    public void PlayPrimed(float[] pitchMult, Note.Name[] pitchNames)
    {
        if (primed)
        {
            PlayAudio(pitchMult);
            destinationColor = ColorPicker.GetColor(pitchNames[wallNumber]);

            //if (pitchMult[wallNumber] < 1)
            //{
            //    destinationColor.r /= 2;
            //    destinationColor.g /= 2;
            //    destinationColor.b /= 2;
            //} else if (pitchMult[wallNumber] < 2)
            //{
            //    destinationColor.r /= 1.5f;
            //    destinationColor.g /= 1.5f;
            //    destinationColor.b /= 1.5f;
            //}
        }

        primed = false;
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
        destinationColor = Color.black;
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
