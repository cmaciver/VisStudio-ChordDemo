using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WandController : MonoBehaviour
{
    private GameObject[] walls;

    private GameObject bassWall;

    private AudioController ac;

    private GameObject activeWall = null;

    float[] pitchMult = null;

    // Start is called before the first frame update
    void Start()
    {
        ac = new(AudioController.Tuning.Equal);

        walls = GameObject.FindGameObjectsWithTag("SoundWall");

        bassWall = GameObject.FindGameObjectWithTag("BassWall");
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
                    wall.GetComponent<WallScript>().StopPrimed();

                bassWall.GetComponent<BassWallScript>().StopAudio();
            }
            else
            {
                float[] pitchMult = GetVoicing(chord);

                foreach (GameObject wall in walls)
                    wall.GetComponent<WallScript>().PlayPrimed(pitchMult);
                
                bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
            }
        }

        // LS Release
        if (gamepad.leftShoulder.wasReleasedThisFrame && !gamepad.leftTrigger.isPressed) // COME BACK TO ME
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
                if (!gamepad.rightTrigger.isPressed) // RT NOT Held
                {
                    bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
                }
            }
        }

        // LT Held
        if (gamepad.leftTrigger.isPressed && activeWall != null)
        {
            if (pitchMult == null)
            {
                activeWall.GetComponent<WallScript>().StopPrimed();
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

        // RS Press
        if (gamepad.rightShoulder.wasPressedThisFrame)
        {
            Chord chord = ac.GetChord(gamepad);
            if (chord == null)
            {
                bassWall.GetComponent<BassWallScript>().StopAudio();
            } else
            {
                bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
            }
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
                voicing = new float[] { chord.Root / 2, chord.Third / 2,  chord.Fifth / 2, chord.Top / 2, // LOWEST Bb2
                                        chord.Root    , chord.Third    ,  chord.Fifth    , chord.Top    ,
                                        chord.Root * 2, chord.Third * 2,  chord.Fifth * 2, chord.Top * 2};  // HIGHEST C6
                break;
            
            case Chord.RootLocation.DbEb:
                voicing = new float[] { chord.Top / 4, chord.Root / 2, chord.Third / 2,  chord.Fifth / 2, // LOWEST Bb2
                                        chord.Top / 2, chord.Root    , chord.Third    ,  chord.Fifth    ,
                                        chord.Top    , chord.Root * 2, chord.Third * 2,  chord.Fifth * 2}; // HIGHEST Bb5
                break;

            case Chord.RootLocation.EGb:
                voicing = new float[] { chord.Fifth / 4, chord.Top / 4, chord.Root / 2, chord.Third / 2, // LOWEST B2
                                        chord.Fifth / 2, chord.Top / 2, chord.Root    , chord.Third    ,
                                        chord.Fifth    , chord.Top    , chord.Root * 2, chord.Third * 2}; // HIGHEST Bb5
                break;

            case Chord.RootLocation.GA:
                voicing = new float[] { chord.Third / 4, chord.Fifth / 4, chord.Top / 4, chord.Root / 2, // LOWEST Bb2
                                        chord.Third / 2, chord.Fifth / 2, chord.Top / 2, chord.Root    ,
                                        chord.Third    , chord.Fifth    , chord.Top    , chord.Root * 2}; // HIGHEST A5
                break;
        }

        //print("this");
        return voicing;
    }
}



// OLD SWITCH STATEMENT IN CASE I NEEED IT LATER
//switch (chord.Seventh)
//{
//    case Chord.SeventhType.None:
//        voicing = new float[] { chord.Root / 4, chord.Root / 2, chord.Fifth / 2, chord.Root,
//                                chord.Third, chord.Fifth, chord.Root * 2, chord.Third * 2,
//                                chord.Fifth * 2, chord.Root * 4, chord.Third * 4, chord.Fifth * 4}; // F-G6 MAX
//        break;

//    case Chord.SeventhType.Major:
//        voicing = new float[] { chord.Root / 4, chord.Root / 2, chord.Fifth / 2, chord.Root,
//                                chord.Third, chord.Fifth, chord.Top, chord.Third * 2,
//                                chord.Fifth * 2, chord.Top * 2, chord.Third * 4, chord.Fifth * 4}; // F-G6 MAX
//        break;

//    case Chord.SeventhType.Minor:
//        voicing = new float[] { chord.Root / 4, chord.Root / 2, chord.Fifth / 2, chord.Root,
//                                chord.Third, chord.Fifth, chord.Top, chord.Third * 2,
//                                chord.Fifth * 2, chord.Top * 2, chord.Third * 4, chord.Fifth * 4}; // F-G6 MAX
//        break;

//    case Chord.SeventhType.Diminished:
//        voicing = new float[] { chord.Root / 8, chord.Root / 4,  chord.Fifth / 4, chord.Third / 2,
//                                chord.Root,     chord.Fifth,     chord.Third * 2, chord.Fifth * 2,
//                                chord.Root * 4, chord.Third * 4, chord.Fifth * 4, chord.Root * 8};
//        break;

//} break;