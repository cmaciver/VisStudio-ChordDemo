using UnityEngine;
using UnityEngine.InputSystem;

public class AudioController
{
#pragma warning disable
    private readonly struct Pitch {
        public Pitch(float m3, float M3, float d5, float p5, float a5, float d7, float h7, float m7, float M7)
        {
            semitone = 1.05946309436f;
            wholetone = 1.12246204831f;
            this.m3 = m3;
            this.M3 = M3;
            this.d5 = d5;
            this.p5 = p5;
            this.a5 = a5;
            this.d7 = d7;
            this.h7 = h7;
            this.m7 = m7;
            this.M7 = M7;
            p8 = 2f;
        }

        public float semitone { get; }
        public float wholetone { get; }
        public float m3 { get; }
        public float M3 { get; }
        public float d5 { get; }
        public float p5 { get; }
        public float a5 { get; }
        public float d7 { get; }
        public float h7 { get; }
        public float m7 { get; }
        public float M7 { get; }
        public float p8 { get; }
    }

    private static readonly Pitch equal = new(1.189207115f, 1.25992104989f, 1.41421356237f, 1.49830707688f,
        1.58740105197f, 1.68179283051f, 1.78179743628f, 1.78179743628f, 1.88774862536f);
    private static readonly Pitch mean = new(1.19630340736f, 1.25f, 1.43105917886f, 1.49536741369f,
        1.6f, 1.67181668772f, 1.78880980357f, 1.78880980357f, 1.86919446035f);
    private static readonly Pitch just = new(1.2f, 1.25f, 1.4f, 1.5f, 1.5625f, 1.728f, 1.75f, 1.8f, 1.875f);

    public enum Tuning { Equal, Just, Mean };

    private readonly Pitch pitch;

    private AudioClip[] clips;
    private int clip = 0;

    public AudioController(Tuning tuning) {
        switch (tuning)
        {
            case Tuning.Equal:
                pitch = equal;
                break;
            case Tuning.Just:
                pitch = just;
                break;
            case Tuning.Mean:
                pitch = mean;
                break;
        }

        clips = Resources.LoadAll<AudioClip>("Audio/Sounds");
    }

    public Chord GetChord(Gamepad gamepad) {
        float rootPitch;
        float thirdMult;
        float fifthMult;
        float topMult;
        
        bool up = gamepad.dpad.up.isPressed; // G (5)
        bool right = gamepad.dpad.right.isPressed; // F (4)
        bool down = gamepad.dpad.down.isPressed; // C (1)
        bool left = gamepad.dpad.left.isPressed; // D (2)

        if (up || right || down || left) {
            if (down) // C
                rootPitch = 1f;
            else if (right) // F
                rootPitch = 1.33483985417f;
            else if (left) // D
                rootPitch = 1.12246204831f;
            else // G
                rootPitch = 1.49830707688f;

            bool rsDead = gamepad.rightStick.ReadValue().magnitude < 0.4f;
            bool rsVert = Mathf.Abs(gamepad.rightStick.y.ReadValue()) >= Mathf.Abs(gamepad.rightStick.x.ReadValue());
            bool rsUp = !rsDead && gamepad.rightStick.y.ReadValue() > 0.0f && rsVert;
            bool rsDown = !rsDead && gamepad.rightStick.y.ReadValue() < -0.0f && rsVert;
            bool rsRight = !rsDead && gamepad.rightStick.x.ReadValue() > 0.0f && !rsVert;
            bool rsLeft = !rsDead && gamepad.rightStick.x.ReadValue() < -0.0f && !rsVert;

            if (rsUp)
                rootPitch *= pitch.wholetone;
            else if (rsDown)
                rootPitch /= pitch.wholetone;
            else if (rsRight)
                rootPitch *= pitch.semitone;
            else if (rsLeft)
                rootPitch /= pitch.semitone;

            bool lsDead = gamepad.leftStick.ReadValue().magnitude < 0.4f;
            bool lsVert = Mathf.Abs(gamepad.leftStick.y.ReadValue()) >= Mathf.Abs(gamepad.leftStick.x.ReadValue());
            bool lsUp = !lsDead && gamepad.leftStick.y.ReadValue() > 0.0f && lsVert;
            bool lsDown = !lsDead && gamepad.leftStick.y.ReadValue() < -0.0f && lsVert;
            bool lsRight = !lsDead && gamepad.leftStick.x.ReadValue() > 0.0f && !lsVert;
            bool lsLeft = !lsDead && gamepad.leftStick.x.ReadValue() < -0.0f && !lsVert;

            if (lsUp)
                rootPitch *= pitch.p8 * pitch.p8;
            else if (lsDown)
                rootPitch /= pitch.p8 * pitch.p8;
            else if (lsRight)
                rootPitch *= pitch.p8;
            else if (lsLeft)
                rootPitch /= pitch.p8;

            bool north = gamepad.buttonNorth.isPressed;
            bool east = gamepad.buttonEast.isPressed;
            bool south = gamepad.buttonSouth.isPressed;
            bool west = gamepad.buttonWest.isPressed;

            thirdMult = south ? pitch.m3 : pitch.M3;
            fifthMult = west ? (south ? pitch.d5 : pitch.a5) : pitch.p5;
            Chord.SeventhType seventh;
            if (north)
            {
                if (east)
                {
                    topMult = pitch.d7;
                    seventh = Chord.SeventhType.Diminished;
                }
                else
                {
                    topMult = pitch.M7;
                    seventh = Chord.SeventhType.Major;
                }
            }
            else
            {
                if (east)
                {
                    if (south)
                        topMult = pitch.m7;
                    else
                        topMult = pitch.h7;
                    
                    seventh = Chord.SeventhType.Minor;
                }
                else
                {
                    topMult = pitch.p8;
                    seventh = Chord.SeventhType.None;
                }
            }

            Chord chord = new(
                rootPitch,
                rootPitch * thirdMult,
                rootPitch * fifthMult,
                rootPitch * topMult,
                seventh
            );

            return chord;
        }

        return null;
    }

    public AudioClip CurrentClip()
    {
        return clips[clip];
    }

    public AudioClip NextClip()
    {
        return clips[clip = (clip + 1) % clips.Length];
    }

    public AudioClip PrevClip()
    {
        return clips[clip = (clip + clips.Length - 1) % clips.Length];
    }

    public AudioClip[] ListClips()
    {
        return clips;
    }

    public AudioClip SelectClip(int index)
    {
        return clips[clip = index];
    }
}

public class Chord
{
    public enum SeventhType { Diminished, Minor, Major, None }

    public Chord(float root, float third, float fifth, float top, SeventhType seventh)
    {
        Root = root;
        Third = third;
        Fifth = fifth;
        Top = top;
        Seventh = seventh;
    }

    public float Root { get; }
    public float Third { get; }
    public float Fifth { get; }
    public float Top { get; }
    public SeventhType Seventh { get; }
}