using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WandController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private GameObject[] walls;

    private GameObject bassWall;

    private AudioController ac;

    private GameObject activeWall = null;

    Chord chord;

    float[] pitchMult = null;

    Note.Name[] pitchNames = null;

    private ParticleSystem.MainModule sparkles;

    public enum Layout { Scale, Melodic, Advanced };
    [SerializeField] private Layout layout = Layout.Scale;
    [SerializeField] private AudioController.Tuning tuning = AudioController.Tuning.Equal;
    [SerializeField] private Note.Name key = Note.Name.C;
    [SerializeField] private AudioController.ScaleMode mode = AudioController.ScaleMode.Major;

    private static class ButtonsHeld
    {
        public enum Button { d_down, d_left, d_right, d_up, f_down, f_left, f_right, f_up, l_trigger, l_shoulder }

        private static readonly float[] DEFAULT = Enumerable.Repeat(-1f, Enum.GetValues(typeof(Button)).Length).ToArray();
        public static float[] timers = DEFAULT;

        private static readonly float HELD_TIME = 0.25f;

        public static void Reset()
        {
            timers = DEFAULT;
        }

        public static bool Update()
        {
            bool held = false;

            for (int i = 0; i < timers.Length; i++)
            {
                if (timers[i] >= HELD_TIME)
                    held = true;
                else if (timers[i] >= 0)
                    timers[i] += Time.deltaTime;
            }

            return held;
        }

        public static bool Press(Button button)
        {
            for (int i = 0; i < timers.Length; i++)
                if (timers[i] >= 0)
                    return false;

            timers[(int)button] = 0;
            return true;
        }

        public static bool Release(Button button)
        {
            if (timers[(int)button] == -1)
                return false;

            timers[(int)button] = -1;
            return true;
        }

        public static bool Held(Button button)
        {
            return timers[(int)button] >= HELD_TIME;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ac = new(tuning, key, mode);

        walls = GameObject.FindGameObjectsWithTag("SoundWall");

        bassWall = GameObject.FindGameObjectWithTag("BassWall");

        sparkles = GameObject.FindGameObjectWithTag("sparkles").GetComponent<ParticleSystem>().main;

        playerInput.deviceLostEvent.AddListener(Disconnect);
    }

    // Update is called once per frame
    void Update()
    {
        Gamepad gamepad = Gamepad.all[playerInput.playerIndex];
        switch (layout)
        {
            case Layout.Advanced:
                AdvancedLayout(gamepad);
                break;

            case Layout.Melodic:
                MelodicLayout(gamepad);
                break;

            case Layout.Scale:
                ScaleLayout(gamepad);
                break;
        }
    }

    private void AdvancedLayout(Gamepad gamepad)
    {
        // LS Press
        if (gamepad.leftShoulder.wasPressedThisFrame)
        {
            chord = ac.GetChordAdvanced(gamepad);

            //if (sparkles.k)

            sparkles.startColor = ColorPicker.GetColor(chord?.RootName);

            if (chord == null)
            {
                foreach (GameObject wall in walls)
                    wall.GetComponent<WallScript>().StopPrimed();

                bassWall.GetComponent<BassWallScript>().StopAudio();

                sparkles.startColor = Color.black;
            }
            else
            {
                float[] pitchMult = GetVoicing(chord);

                Note.Name[] pitchNames = GetVoicingNames(chord);

                foreach (GameObject wall in walls)
                    wall.GetComponent<WallScript>().PlayPrimed(pitchMult, pitchNames, chord);

                if (!gamepad.rightTrigger.isPressed) // RT NOT Held
                {
                    bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
                }
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
            chord = ac.GetChordAdvanced(gamepad);
            if (chord == null)
            {
                pitchMult = null;
                pitchNames = null;
            }
            else
            {
                pitchMult = GetVoicing(chord);
                pitchNames = GetVoicingNames(chord);
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
                activeWall.GetComponent<WallScript>().PlayPrimed(pitchMult, pitchNames, chord);
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
            chord = ac.GetChordAdvanced(gamepad);
            if (chord == null)
            {
                bassWall.GetComponent<BassWallScript>().StopAudio();

                sparkles.startColor = Color.black;
            }
            else
            {
                bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
            }
        }
    }

    private void MelodicLayout(Gamepad gamepad) // TODO
    {
        // LS Press
        if (gamepad.leftShoulder.wasPressedThisFrame)
        {
            chord = ac.GetChordMelodic(gamepad);

            //if (sparkles.k)

            sparkles.startColor = ColorPicker.GetColor(chord?.RootName);

            if (chord == null)
            {
                foreach (GameObject wall in walls)
                    wall.GetComponent<WallScript>().StopPrimed();

                bassWall.GetComponent<BassWallScript>().StopAudio();

                sparkles.startColor = Color.black;
            }
            else
            {
                float[] pitchMult = GetVoicing(chord);

                Note.Name[] pitchNames = GetVoicingNames(chord);

                foreach (GameObject wall in walls)
                    wall.GetComponent<WallScript>().PlayPrimed(pitchMult, pitchNames, chord);

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
            chord = ac.GetChordMelodic(gamepad);
            if (chord == null)
            {
                pitchMult = null;
                pitchNames = null;
            }
            else
            {
                pitchMult = GetVoicing(chord);
                pitchNames = GetVoicingNames(chord);

                bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
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
                activeWall.GetComponent<WallScript>().PlayPrimed(pitchMult, pitchNames, chord);
            }
        }

        // LT Release
        if (gamepad.leftTrigger.wasReleasedThisFrame && !gamepad.leftShoulder.isPressed)
        {
            foreach (GameObject wall in walls)
                wall.GetComponent<WallScript>().Reprime();
        }
    }

    private void ScaleLayout(Gamepad gamepad)
    {
        if (ButtonsHeld.Update() && activeWall != null)
        {
            sparkles.startColor = ColorPicker.GetColor(chord?.RootName);
            if (chord == null)
            {
                activeWall.GetComponent<WallScript>().StopPrimed();
                bassWall.GetComponent<BassWallScript>().StopAudio();
            }
            else
            {
                activeWall.GetComponent<WallScript>().PlayPrimed(pitchMult, pitchNames, chord);
                bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
            }
        }

        ScaleLayoutCheck(gamepad, gamepad.dpad.down, ButtonsHeld.Button.d_down);
        ScaleLayoutCheck(gamepad, gamepad.dpad.left, ButtonsHeld.Button.d_left);
        ScaleLayoutCheck(gamepad, gamepad.dpad.right, ButtonsHeld.Button.d_right);
        ScaleLayoutCheck(gamepad, gamepad.dpad.up, ButtonsHeld.Button.d_up);

        ScaleLayoutCheck(gamepad, gamepad.buttonSouth, ButtonsHeld.Button.f_down);
        ScaleLayoutCheck(gamepad, gamepad.buttonWest, ButtonsHeld.Button.f_left);
        ScaleLayoutCheck(gamepad, gamepad.buttonEast, ButtonsHeld.Button.f_right);
        ScaleLayoutCheck(gamepad, gamepad.buttonNorth, ButtonsHeld.Button.f_up);

        ScaleLayoutCheck(gamepad, gamepad.leftTrigger, ButtonsHeld.Button.l_trigger);

        ScaleLayoutCheck(gamepad, gamepad.leftShoulder, ButtonsHeld.Button.l_shoulder);
    }

    private void ScaleLayoutCheck(Gamepad gamepad, UnityEngine.InputSystem.Controls.ButtonControl buttonControl, ButtonsHeld.Button button)
    {
        if (buttonControl.wasPressedThisFrame && ButtonsHeld.Press(button))
        {
            chord = ac.GetChordScale(gamepad);
            if (chord != null)
            {
                pitchMult = GetVoicing(chord);
                pitchNames = GetVoicingNames(chord);
            }
        }
        if (buttonControl.wasReleasedThisFrame)
        {
            if (!ButtonsHeld.Held(button) && ButtonsHeld.Release(button))
            {
                sparkles.startColor = ColorPicker.GetColor(chord?.RootName);

                if (chord == null)
                {
                    foreach (GameObject wall in walls)
                        wall.GetComponent<WallScript>().StopPrimed();

                    bassWall.GetComponent<BassWallScript>().StopAudio();

                    sparkles.startColor = Color.black;
                }
                else
                {
                    foreach (GameObject wall in walls)
                        wall.GetComponent<WallScript>().PlayPrimed(pitchMult, pitchNames, chord);

                    bassWall.GetComponent<BassWallScript>().PlayAudio(chord);
                }
            }

            foreach (GameObject wall in walls)
                wall.GetComponent<WallScript>().Reprime();
            ButtonsHeld.Release(button);
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

    private Note.Name[] GetVoicingNames(Chord chord)
    {
        Note.Name[] voicing = new Note.Name[] { chord.RootName, chord.RootName,  chord.FifthName, chord.ThirdName,
                                        chord.FifthName,    chord.RootName,  chord.ThirdName, chord.FifthName,
                                        chord.TopName,  chord.ThirdName, chord.FifthName, chord.TopName};

        switch (chord.RootLoc)
        {
            case Chord.RootLocation.BbC:
                voicing = new Note.Name[] { chord.RootName, chord.ThirdName,  chord.FifthName, chord.TopName, // LOWEST Bb2
                                        chord.RootName   , chord.ThirdName   ,  chord.FifthName    , chord.TopName    ,
                                        chord.RootName, chord.ThirdName,  chord.FifthName, chord.TopName};  // HIGHEST C6
                break;

            case Chord.RootLocation.DbEb:
                voicing = new Note.Name[] { chord.TopName, chord.RootName, chord.ThirdName,  chord.FifthName, // LOWEST Bb2
                                        chord.TopName, chord.RootName   , chord.ThirdName   ,  chord.FifthName    ,
                                        chord.TopName    , chord.RootName, chord.ThirdName,  chord.FifthName}; // HIGHEST Bb5
                break;

            case Chord.RootLocation.EGb:
                voicing = new Note.Name[] { chord.FifthName, chord.TopName, chord.RootName, chord.ThirdName, // LOWEST B2
                                        chord.FifthName, chord.TopName, chord.RootName   , chord.ThirdName   ,
                                        chord.FifthName    , chord.TopName    , chord.RootName, chord.ThirdName}; // HIGHEST Bb5
                break;

            case Chord.RootLocation.GA:
                voicing = new Note.Name[] { chord.ThirdName, chord.FifthName, chord.TopName, chord.RootName, // LOWEST Bb2
                                        chord.ThirdName, chord.FifthName, chord.TopName, chord.RootName   ,
                                        chord.ThirdName   , chord.FifthName    , chord.TopName    , chord.RootName}; // HIGHEST A5
                break;
        }

        //print("this");
        return voicing;
    }

    private void OnValidate()
    {
        if (ac != null)
        {
            ac.SetTuning(tuning);
            ac.SetKey(key);
            ac.SetMode(mode);
        }
    }

    private void Disconnect(PlayerInput input)
    {
        Destroy(playerInput.gameObject);
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