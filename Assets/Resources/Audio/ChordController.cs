using UnityEngine;
using UnityEngine.InputSystem;

public class ChordController : MonoBehaviour
{
    public AudioController.Tuning tuning;
    private AudioController controller;

    private const float MAX_VOL = 0.5f;
    private const float MIN_VOL = 0.05f;
    private const float VOL_STEP = 0.05f;

    public int player = 0;

    public GameObject rootObj;
    public GameObject thirdObj;
    public GameObject fifthObj;
    public GameObject topObj;

    private AudioSource root;
    private AudioSource third;
    private AudioSource fifth;
    private AudioSource top;

    // Start is called before the first frame update
    void Start()
    {
        controller = new(tuning);

        root = rootObj.GetComponent<AudioSource>();
        third = thirdObj.GetComponent<AudioSource>();
        fifth = fifthObj.GetComponent<AudioSource>();
        top = topObj.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count > player)
        {
            Gamepad gamepad = Gamepad.all[player];

            bool select = gamepad.selectButton.wasPressedThisFrame;

            if (select)
                SetClip(controller.NextClip());
        
            bool volUp = gamepad.rightShoulder.wasPressedThisFrame;
            bool volDown = gamepad.rightTrigger.wasPressedThisFrame;

            if (volUp && !volDown)
            {
                root.volume = Mathf.Min(MAX_VOL, root.volume + VOL_STEP);
                third.volume = Mathf.Min(MAX_VOL, third.volume + VOL_STEP);
                fifth.volume = Mathf.Min(MAX_VOL, fifth.volume + VOL_STEP);
                top.volume = Mathf.Min(MAX_VOL, top.volume + VOL_STEP);
            }
            else if (volDown && !volUp)
            {
                root.volume = Mathf.Max(MIN_VOL, root.volume - VOL_STEP);
                third.volume = Mathf.Max(MIN_VOL, third.volume - VOL_STEP);
                fifth.volume = Mathf.Max(MIN_VOL, fifth.volume - VOL_STEP);
                top.volume = Mathf.Max(MIN_VOL, top.volume - VOL_STEP);
            }

            if (gamepad.leftTrigger.wasPressedThisFrame || gamepad.leftShoulder.wasPressedThisFrame)
            {
                Chord chord = controller.GetChord(gamepad);
                if (chord != null)
                {
                    SetChord(chord);
                    Play();
                }
                else
                    Stop();
            }
        
            if (gamepad.leftShoulder.wasReleasedThisFrame)
                Stop();
        }
        else
            Stop();
    }

    private void SetChord(Chord chord)
    {
        root.pitch = chord.Root;
        third.pitch = chord.Third;
        fifth.pitch = chord.Fifth;
        top.pitch = chord.Top;
    }

    private void Play()
    {
        root.Play();
        third.Play();
        fifth.Play();
        top.Play();
    }

    private void Stop()
    {
        root.Stop();
        third.Stop();
        fifth.Stop();
        top.Stop();
    }

    private void SetClip(AudioClip clip)
    {
        bool play = root.isPlaying;
        root.clip = clip;
        third.clip = clip;
        fifth.clip = clip;
        top.clip = clip;
        if (play)
            Play();
    }
}
