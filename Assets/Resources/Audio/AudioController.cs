using UnityEngine;
using UnityEngine.InputSystem;

public class AudioController
{
#pragma warning disable
    private readonly struct Pitch
    {
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

    private readonly struct Mode
    {
        public Mode(float[][] pitches, Note.Name[][] names)
        {
            this.pitches = pitches;
            this.names = names;
        }

        public float[] GetPitch(int degree)
        {
            float mult = 1f;

            degree--;
            while (degree < 0)
            {
                degree += pitches.Length;
                mult *= 0.5f;
            }
            while (degree >= pitches.Length)
            {
                degree -= pitches.Length;
                mult *= 2f;
            }

            float[] ret = pitches[degree];
            for (int i = 0; i < ret.Length; i++)
                ret[i] *= mult;

            return ret;
        }

        public Note.Name[] GetName(int degree)
        {
            return names[(degree - 1) % names.Length];
        }

        private float[][] pitches { get; }
        private Note.Name[][] names { get; }
    }

    private static readonly Mode major = new(
        new float[][] {
            new float[] { 1f, 1.25992104989f, 1.49830707688f, 1.88774862536f },
            new float[] { 1.12246204831f, 1.33483985417f, 1.68179283051f, 2f },
            new float[] { 1.25992104989f, 1.49830707688f, 1.49830707688f, 2f * 1.12246204831f },
            new float[] { 1.33483985417f, 1.68179283051f, 2f, 2f * 1.25992104989f },
            new float[] { 1.49830707688f, 1.88774862536f, 2f * 1.12246204831f, 2f * 1.33483985417f },
            new float[] { 1.68179283051f , 2f, 2f * 1.25992104989f, 2f * 1.49830707688f },
            new float[] { 1.88774862536f, 2f * 1.12246204831f, 2f * 1.33483985417f, 2f * 1.68179283051f } },
        new Note.Name[][] {
            new Note.Name[] { Note.Name.C, Note.Name.E, Note.Name.G, Note.Name.B },
            new Note.Name[] { Note.Name.D, Note.Name.F, Note.Name.A, Note.Name.C },
            new Note.Name[] { Note.Name.E, Note.Name.G, Note.Name.B, Note.Name.D },
            new Note.Name[] { Note.Name.F, Note.Name.A, Note.Name.C, Note.Name.E },
            new Note.Name[] { Note.Name.G, Note.Name.B, Note.Name.D, Note.Name.F },
            new Note.Name[] { Note.Name.A, Note.Name.C, Note.Name.E, Note.Name.G },
            new Note.Name[] { Note.Name.B, Note.Name.D, Note.Name.F, Note.Name.A }});

    public enum Tuning { Equal, Just, Mean };
    public enum Layout { Scale, Melodic, Advanced };


    private readonly Pitch pitch;
    private Layout layout;
    private Mode mode;
    private int offset;

    private AudioClip[] clips;
    private int clip = 0;

    public AudioController(Tuning tuning, Layout layout)
    {
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

        mode = major;
        offset = (int)Note.Name.Bb - 2;

        this.layout = layout;

        clips = Resources.LoadAll<AudioClip>("Audio/Sounds");
    }

    public void SetLayout(Layout layout)
    {
        this.layout = layout;
    }

    public Chord GetChord(Gamepad gamepad)
    {
        // Default values to stop compiler from giving errors
        float rootPitch = 1f;
        float thirdPitch = pitch.M3;
        float fifthPitch = pitch.p5;
        float topPitch = pitch.p8;

        Note.Name rootName = Note.Name.C;
        Note.Name thirdName = Note.Name.E;
        Note.Name fifthName = Note.Name.G;
        Note.Name topName = Note.Name.C;

        Chord.RootLocation rootLoc = Chord.RootLocation.BbC;
        Chord.ThirdTypeE thirdType = Chord.ThirdTypeE.Major;
        Chord.SeventhTypeE seventhType = Chord.SeventhTypeE.None;

        string rootPos;

        switch (layout) {
            case Layout.Advanced:
                float thirdMult;
                float fifthMult;
                float topMult;

                bool up = gamepad.dpad.up.isPressed; // G (5)
                bool right = gamepad.dpad.right.isPressed; // F (4)
                bool down = gamepad.dpad.down.isPressed; // C (1)
                bool left = gamepad.dpad.left.isPressed; // D (2)

                if (up || right || down || left)
                {
                    if (down) // C
                    {
                        rootPitch = 1f;
                        rootName = Note.Name.C;
                    }
                    else if (right) // F
                    {
                        rootPitch = 1.33483985417f;
                        rootName = Note.Name.F;
                    }
                    else if (left) // D
                    {
                        rootPitch = 1.12246204831f;
                        rootName = Note.Name.D;
                    }
                    else // G
                    {
                        rootPitch = 1.49830707688f;
                        rootName = Note.Name.G;
                    }

                    bool rsDead = gamepad.rightStick.ReadValue().magnitude < 0.4f;
                    bool rsVert = Mathf.Abs(gamepad.rightStick.y.ReadValue()) >= Mathf.Abs(gamepad.rightStick.x.ReadValue());
                    bool rsUp = !rsDead && gamepad.rightStick.y.ReadValue() > 0.0f && rsVert;
                    bool rsDown = !rsDead && gamepad.rightStick.y.ReadValue() < -0.0f && rsVert;
                    bool rsRight = !rsDead && gamepad.rightStick.x.ReadValue() > 0.0f && !rsVert;
                    bool rsLeft = !rsDead && gamepad.rightStick.x.ReadValue() < -0.0f && !rsVert;

                    if (rsUp)
                    {
                        rootPitch *= pitch.wholetone;
                        rootName = Note.Up(rootName, 2);
                    }
                    else if (rsDown)
                    {
                        rootPitch /= pitch.wholetone;
                        rootName = Note.Down(rootName, 2);
                    }
                    else if (rsRight)
                    {
                        rootPitch *= pitch.semitone;
                        rootName = Note.Up(rootName);
                    }
                    else if (rsLeft)
                    {
                        rootPitch /= pitch.semitone;
                        rootName = Note.Down(rootName);
                    }

                    if (rootPitch < 1.02930223664f)
                        rootLoc = Chord.RootLocation.BbC;
                    else if (rootPitch < 1.2240535433f)
                        rootLoc = Chord.RootLocation.DbEb;
                    else if (rootPitch < 1.45565318284f)
                        rootLoc = Chord.RootLocation.EGb;
                    else
                        rootLoc = Chord.RootLocation.GA;

                    bool north = gamepad.buttonNorth.isPressed;
                    bool east = gamepad.buttonEast.isPressed;
                    bool south = gamepad.buttonSouth.isPressed;
                    bool west = gamepad.buttonWest.isPressed;

                    if (south)
                    {
                        thirdMult = pitch.m3;
                        thirdName = Note.Up(rootName, 3);
                        thirdType = Chord.ThirdTypeE.Minor;
                    }
                    else
                    {
                        thirdMult = pitch.M3;
                        thirdName = Note.Up(rootName, 4);
                        thirdType = Chord.ThirdTypeE.Major;
                    }

                    if (west)
                    {
                        if (south)
                        {
                            fifthMult = pitch.d5;
                            fifthName = Note.Up(rootName, 6);
                        }
                        else
                        {
                            fifthMult = pitch.a5;
                            fifthName = Note.Up(rootName, 8);
                        }
                    }
                    else
                    {
                        fifthMult = pitch.p5;
                        fifthName = Note.Up(rootName, 7);
                    }

                    if (north)
                    {
                        if (east)
                        {
                            topMult = pitch.d7;
                            seventhType = Chord.SeventhTypeE.Diminished;
                            topName = Note.Down(rootName, 3);
                        }
                        else
                        {
                            topMult = pitch.M7;
                            seventhType = Chord.SeventhTypeE.Major;
                            topName = Note.Down(rootName, 1);
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

                            seventhType = Chord.SeventhTypeE.Minor;
                            topName = Note.Down(rootName, 2);
                        }
                        else
                        {
                            topMult = pitch.p8;
                            seventhType = Chord.SeventhTypeE.None;
                            topName = rootName;
                        }
                    }

                    thirdPitch = rootPitch * thirdMult;
                    fifthPitch = rootPitch * fifthMult;
                    topPitch = rootPitch * topMult;
                }
                else
                    return null;
                break;

            case Layout.Melodic:
                // TODO
                break;

            case Layout.Scale:
                bool one = gamepad.dpad.down.wasPressedThisFrame || gamepad.buttonNorth.wasPressedThisFrame;
                bool two = gamepad.dpad.left.wasPressedThisFrame;
                bool three = gamepad.dpad.right.wasPressedThisFrame;
                bool four = gamepad.dpad.up.wasPressedThisFrame;
                bool five = gamepad.buttonSouth.wasPressedThisFrame;
                bool six = gamepad.buttonWest.wasPressedThisFrame;
                bool seven = gamepad.buttonEast.wasPressedThisFrame;
                bool silence = gamepad.leftTrigger.wasPressedThisFrame;

                int degree = 0;
                float pitchMult = Mathf.Pow(pitch.semitone, offset);

                if (silence)
                    return null;
                else if (one)
                    degree = 1;
                else if (two)
                    degree = 2;
                else if (three)
                    degree = 3;
                else if (four)
                    degree = 4;
                else if (five)
                    degree = 5;
                else if (six)
                    degree = 6;
                else if (seven)
                    degree = 7;

                float[] pitches = mode.GetPitch(degree);
                Note.Name[] names = mode.GetName(degree);

                rootPitch = pitches[0] * pitchMult;
                rootName = Note.Up(names[0], offset);
                thirdPitch = pitches[1] * pitchMult;
                thirdName = Note.Up(names[1], offset);
                fifthPitch = pitches[2] * pitchMult;
                fifthName = Note.Up(names[2], offset);
                topPitch = pitches[0] * 2f * pitchMult;
                topName = Note.Up(names[0], offset);

                if (rootPitch < 1.02930223664f)
                    rootLoc = Chord.RootLocation.BbC;
                else if (rootPitch < 1.2240535433f)
                    rootLoc = Chord.RootLocation.DbEb;
                else if (rootPitch < 1.45565318284f)
                    rootLoc = Chord.RootLocation.EGb;
                else
                    rootLoc = Chord.RootLocation.GA;

                break;
        }

        Chord chord = new(
            rootPitch,
            thirdPitch,
            fifthPitch,
            topPitch,
            rootName,
            thirdName,
            fifthName,
            topName,
            rootLoc,
            thirdType,
            seventhType
        );

        return chord;
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
    public enum RootLocation { BbC, DbEb, EGb, GA }
    public enum ThirdTypeE { Minor, Major }
    public enum SeventhTypeE { Diminished, Minor, Major, None }

    public Chord(float root, float third, float fifth, float top,
        Note.Name rootName, Note.Name thirdName, Note.Name fifthName, Note.Name topName, RootLocation rootLoc, ThirdTypeE thirdType, SeventhTypeE seventhType)
    {
        Root = root;
        Third = third;
        Fifth = fifth;
        Top = top;
        RootName = rootName;
        ThirdName = thirdName;
        FifthName = fifthName;
        TopName = topName;
        RootLoc = rootLoc;
        ThirdType = thirdType;
        SeventhType = seventhType;
    }

    public float Root { get; }
    public float Third { get; }
    public float Fifth { get; }
    public float Top { get; }
    public Note.Name RootName { get; }
    public Note.Name ThirdName { get; }
    public Note.Name FifthName { get; }
    public Note.Name TopName { get; }
    public RootLocation RootLoc { get; }
    public ThirdTypeE ThirdType { get; }
    public SeventhTypeE SeventhType { get; }
}

public class Note
{
    public enum Name { Bb, B, C, Db, D, Eb, E, F, Gb, G, Ab, A }

    public static Name Up(Name n, int steps = 1)
    {
        if (steps < 0)
            return Down(n, -steps);

        Name ret = n;

        for (int i = steps; i > 0; i--)
        {
            if (ret == Name.A)
                ret = Name.Bb;
            else
                ret++;
        }

        return ret;
    }

    public static Name Down(Name n, int steps = 1)
    {
        if (steps < 0)
            return Up(n, -steps);

        Name ret = n;

        for (int i = steps; i > 0; i--)
        {
            if (ret == Name.Bb)
                ret = Name.A;
            else
                ret--;
        }

        return ret;
    }
}