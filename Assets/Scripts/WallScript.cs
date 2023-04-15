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

    private float[] volume = {0.775f, 0.75f, 0.725f, 0.7f, 0.675f, 0.65f, 0.625f, 0.6f, 0.575f, 0.55f, 0.525f, 0.5f };

    private bool primed = true;

    // Start is called before the first frame update
    void Start()
    {
        currentSource = sources[0];

        foreach (AudioSource source in sources)
        {
            source.clip = soundfont;
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPrimed(float[] pitchMult)
    {
        if (primed)
            PlayAudio(pitchMult);

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
