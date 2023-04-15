using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WandController : MonoBehaviour
{
    private GameObject[] walls;

    private AudioController ac;

    private GameObject activeWall = null;

    float[] pitchMult = null;

    // Start is called before the first frame update
    void Start()
    {
        ac = new(AudioController.Tuning.Equal);

        walls = GameObject.FindGameObjectsWithTag("SoundWall");
    }

    // Update is called once per frame
    void Update()
    {
        Gamepad gamepad = Gamepad.all[0];

        // LS Press
        if (gamepad.leftShoulder.wasPressedThisFrame)
        {
            Chord chord = ac.GetChord(gamepad);

            if (chord == null)
            {
                foreach (GameObject wall in walls)
                    wall.GetComponent<WallScript>().StopAudio();
            }
            else
            {
                float[] pitchMult = GetVoicing(chord);

                foreach (GameObject wall in walls)
                    wall.GetComponent<WallScript>().PlayPrimed(pitchMult);
            }
        }

        // LT Release
        if (gamepad.leftShoulder.wasReleasedThisFrame && !gamepad.leftTrigger.isPressed)
        {
            foreach (GameObject wall in walls)
                wall.GetComponent<WallScript>().Reprime();
        }

        // LT Press
        if (gamepad.leftTrigger.wasPressedThisFrame)
        {
            Chord chord = ac.GetChord(gamepad);
            if (chord == null)
            {
                pitchMult = null;
            }
            else
            {
                pitchMult = GetVoicing(chord);
            }
        }

        // LT Held
        if (gamepad.leftTrigger.isPressed && activeWall != null)
        {
            if (pitchMult == null)
            {
                activeWall.GetComponent<WallScript>().StopAudio();
            }
            else
            {
                activeWall.GetComponent<WallScript>().PlayPrimed(pitchMult);
            }
        }

        // LT Release
        if (gamepad.leftTrigger.wasReleasedThisFrame && !gamepad.leftShoulder.isPressed)
        {
            foreach (GameObject wall in walls)
                wall.GetComponent<WallScript>().Reprime();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("SoundWall"))
            activeWall = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SoundWall") && activeWall == other.gameObject)
            activeWall = null;
    }

    private float[] GetVoicing(Chord chord)
    {
        float[] voicing = new float[] { chord.Root / 4, chord.Root / 2,  chord.Fifth / 2, chord.Third,
                                        chord.Fifth,    chord.Root * 2,  chord.Third * 2, chord.Fifth * 2,
                                        chord.Top * 2,  chord.Third * 4, chord.Fifth * 4, chord.Top * 4};

        switch (chord.RootLoc)
        {
            case Chord.RootLocation.BbC:
                switch (chord.Seventh)
                {
                    case Chord.SeventhType.None:
                        voicing = new float[] { chord.Root / 4, chord.Root / 2, chord.Fifth / 2, chord.Root,
                                                chord.Third, chord.Fifth, chord.Root * 2, chord.Third * 2,
                                                chord.Fifth * 2, chord.Root * 4, chord.Third * 4, chord.Fifth * 4}; // F-G6 MAX
                        break;

                    case Chord.SeventhType.Major:
                        voicing = new float[] { chord.Root / 4, chord.Root / 2, chord.Fifth / 2, chord.Root,
                                                chord.Third, chord.Fifth, chord.Top, chord.Third * 2,
                                                chord.Fifth * 2, chord.Top * 2, chord.Third * 4, chord.Fifth * 4}; // F-G6 MAX
                        break;

                    case Chord.SeventhType.Minor:
                        voicing = new float[] { chord.Root / 4, chord.Root / 2, chord.Fifth / 2, chord.Root,
                                                chord.Third, chord.Fifth, chord.Top, chord.Third * 2,
                                                chord.Fifth * 2, chord.Top * 2, chord.Third * 4, chord.Fifth * 4}; // F-G6 MAX
                        break;

                    case Chord.SeventhType.Diminished:
                        voicing = new float[] { chord.Root / 8, chord.Root / 4,  chord.Fifth / 4, chord.Third / 2,
                                                chord.Root,     chord.Fifth,     chord.Third * 2, chord.Fifth * 2,
                                                chord.Root * 4, chord.Third * 4, chord.Fifth * 4, chord.Root * 8};
                        break;

                } break;
            case Chord.RootLocation.DbEb:
                switch (chord.Seventh)
                {
                    case Chord.SeventhType.None:
                        voicing = new float[] { chord.Root / 4, chord.Root / 2,  chord.Fifth / 2, chord.Third,
                                                chord.Fifth,    chord.Root * 2,  chord.Third * 2, chord.Fifth * 2,
                                                chord.Top * 2,  chord.Third * 4, chord.Fifth * 4, chord.Top * 4};
                        break;

                    case Chord.SeventhType.Major:
                        //voicing = { };
                        break;

                    case Chord.SeventhType.Minor:

                        break;

                    case Chord.SeventhType.Diminished:

                        break;

                } break;
            case Chord.RootLocation.EGb:
                switch (chord.Seventh)
                {
                    case Chord.SeventhType.None:
                        voicing = new float[] { chord.Root / 4, chord.Root / 2,  chord.Fifth / 2, chord.Third,
                                                chord.Fifth,    chord.Root * 2,  chord.Third * 2, chord.Fifth * 2,
                                                chord.Top * 2,  chord.Third * 4, chord.Fifth * 4, chord.Top * 4};
                        break;

                    case Chord.SeventhType.Major:
                        //voicing = { };
                        break;

                    case Chord.SeventhType.Minor:

                        break;

                    case Chord.SeventhType.Diminished:

                        break;

                } break;
            case Chord.RootLocation.GA:
                switch (chord.Seventh)
                {
                    case Chord.SeventhType.None:
                        voicing = new float[] { chord.Root / 4, chord.Root / 2,  chord.Fifth / 2, chord.Third,
                                                chord.Fifth,    chord.Root * 2,  chord.Third * 2, chord.Fifth * 2,
                                                chord.Top * 2,  chord.Third * 4, chord.Fifth * 4, chord.Top * 4};
                        break;

                    case Chord.SeventhType.Major:
                        //voicing = { };
                        break;

                    case Chord.SeventhType.Minor:

                        break;

                    case Chord.SeventhType.Diminished:

                        break;

                } break;
        }

        //print("this");
        return voicing;
    }
}
