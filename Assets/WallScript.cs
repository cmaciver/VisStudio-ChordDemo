using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    private int wallNumber;

    [SerializeField]
    private AudioSource source;

    private float[] volume = {0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.65f, 0.6f, 0.55f, 0.5f };

    private bool primed = true;

    // Start is called before the first frame update
    void Start()
    {

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
        source.pitch = pitchMult[wallNumber];
        //source.Play();
        StartCoroutine(FadeAudioSource.StartFade(source, 0.5f, volume[wallNumber])); // TODO LATER, PLAY
    }

    public void Reprime()
    {
        primed = true;
    }

    public void StopAudio()
    {
        StartCoroutine(FadeAudioSource.StartFade(source, 1f, 0));  // TODO THIS DOES NOT WORK PROBABLY
        //source.Stop();
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
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }

    }
}
