using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioController
{
#pragma warning disable
    public readonly struct Pitch
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
        public Mode(Note.Name[][][] names)
        {
            this.names = names;
        }

        public Note.Name[] GetNames(int degree, bool spicy)
        {
            return names[(degree - 1) % names.Length][spicy ? 1 : 0];
        }

        private Note.Name[][][] names { get; }
    }

    private static readonly Mode major = new(
        new Note.Name[][][] {
            new Note.Name[][] { new Note.Name[] { Note.Name.C, Note.Name.E, Note.Name.G, Note.Name.C },
                                new Note.Name[] { Note.Name.C, Note.Name.E, Note.Name.G, Note.Name.B } },
            new Note.Name[][] { new Note.Name[] { Note.Name.D, Note.Name.F, Note.Name.A, Note.Name.D },
                                new Note.Name[] { Note.Name.D, Note.Name.F, Note.Name.A, Note.Name.C } },
            new Note.Name[][] { new Note.Name[] { Note.Name.E, Note.Name.G, Note.Name.B, Note.Name.E },
                                new Note.Name[] { Note.Name.E, Note.Name.G, Note.Name.B, Note.Name.D } },
            new Note.Name[][] { new Note.Name[] { Note.Name.F, Note.Name.A, Note.Name.C, Note.Name.F },
                                new Note.Name[] { Note.Name.F, Note.Name.A, Note.Name.C, Note.Name.E } },
            new Note.Name[][] { new Note.Name[] { Note.Name.G, Note.Name.B, Note.Name.D, Note.Name.G },
                                new Note.Name[] { Note.Name.G, Note.Name.B, Note.Name.D, Note.Name.F } },
            new Note.Name[][] { new Note.Name[] { Note.Name.A, Note.Name.C, Note.Name.E, Note.Name.A },
                                new Note.Name[] { Note.Name.A, Note.Name.C, Note.Name.E, Note.Name.G } },
            new Note.Name[][] { new Note.Name[] { Note.Name.B, Note.Name.D, Note.Name.G, Note.Name.B },
                                new Note.Name[] { Note.Name.B, Note.Name.D, Note.Name.G, Note.Name.A } } });
    private static readonly Mode hminor = new(
        new Note.Name[][][] {
            new Note.Name[][] { new Note.Name[] { Note.Name.C, Note.Name.Eb, Note.Name.G, Note.Name.C },
                                new Note.Name[] { Note.Name.C, Note.Name.Eb, Note.Name.G, Note.Name.Bb } },
            new Note.Name[][] { new Note.Name[] { Note.Name.D, Note.Name.F, Note.Name.A, Note.Name.D },
                                new Note.Name[] { Note.Name.D, Note.Name.F, Note.Name.A, Note.Name.C } },
            new Note.Name[][] { new Note.Name[] { Note.Name.Eb, Note.Name.G, Note.Name.Bb, Note.Name.Eb },
                                new Note.Name[] { Note.Name.Eb, Note.Name.G, Note.Name.Bb, Note.Name.D } },
            new Note.Name[][] { new Note.Name[] { Note.Name.F, Note.Name.Ab, Note.Name.C, Note.Name.F },
                                new Note.Name[] { Note.Name.F, Note.Name.Ab, Note.Name.C, Note.Name.Eb } },
            new Note.Name[][] { new Note.Name[] { Note.Name.G, Note.Name.B, Note.Name.D, Note.Name.G },
                                new Note.Name[] { Note.Name.G, Note.Name.B, Note.Name.D, Note.Name.F } },
            new Note.Name[][] { new Note.Name[] { Note.Name.Ab, Note.Name.C, Note.Name.Eb, Note.Name.Ab },
                                new Note.Name[] { Note.Name.Ab, Note.Name.C, Note.Name.Eb, Note.Name.G } },
            new Note.Name[][] { new Note.Name[] { Note.Name.B, Note.Name.D, Note.Name.G, Note.Name.B },
                                new Note.Name[] { Note.Name.B, Note.Name.D, Note.Name.G, Note.Name.A } } });
    private static readonly Mode phrygiand = new(
        new Note.Name[][][] {
            new Note.Name[][] { new Note.Name[] { Note.Name.C, Note.Name.Eb, Note.Name.G, Note.Name.C },
                                new Note.Name[] { Note.Name.C, Note.Name.E, Note.Name.G, Note.Name.C } },
            new Note.Name[][] { new Note.Name[] { Note.Name.Db, Note.Name.F, Note.Name.Ab, Note.Name.Db },
                                new Note.Name[] { Note.Name.Db, Note.Name.F, Note.Name.Ab, Note.Name.C } },
            new Note.Name[][] { new Note.Name[] { Note.Name.Eb, Note.Name.G, Note.Name.Bb, Note.Name.Eb },
                                new Note.Name[] { Note.Name.Eb, Note.Name.G, Note.Name.Bb, Note.Name.Db } },
            new Note.Name[][] { new Note.Name[] { Note.Name.F, Note.Name.Ab, Note.Name.C, Note.Name.F },
                                new Note.Name[] { Note.Name.F, Note.Name.Ab, Note.Name.C, Note.Name.Eb } },
            new Note.Name[][] { new Note.Name[] { Note.Name.G, Note.Name.B, Note.Name.D, Note.Name.G },
                                new Note.Name[] { Note.Name.G, Note.Name.B, Note.Name.D, Note.Name.F } },
            new Note.Name[][] { new Note.Name[] { Note.Name.Ab, Note.Name.C, Note.Name.Eb, Note.Name.Ab },
                                new Note.Name[] { Note.Name.Ab, Note.Name.C, Note.Name.Eb, Note.Name.G } },
            new Note.Name[][] { new Note.Name[] { Note.Name.Bb, Note.Name.D, Note.Name.F, Note.Name.Bb },
                                new Note.Name[] { Note.Name.Bb, Note.Name.D, Note.Name.F, Note.Name.Ab } } });

    public enum Tuning { Equal, Just, Mean };
    public enum ScaleMode { Major, HarmonicMinor, PhrygianDominant };

    private Mode mode;
    private int offset;

    private AudioClip[] clips;
    private int clip = 0;

    public AudioController(Tuning tuning, Note.Name key, ScaleMode mode)
    {
        SetTuning(tuning);

        SetKey(key);
        SetMode(mode);

        clips = Resources.LoadAll<AudioClip>("Audio/Sounds");
    }

    public void SetKey(Note.Name key)
    {
        offset = (int)key - 2;
    }

    public void SetMode(ScaleMode scaleMode)
    {
        mode = scaleMode switch
        {
            ScaleMode.Major => major,
            ScaleMode.HarmonicMinor => hminor,
            ScaleMode.PhrygianDominant => phrygiand,
        };
    }

    public void SetTuning(Tuning tuning)
    {
        switch (tuning)
        {
            case Tuning.Equal:
                Chord.SetPitch(equal);
                break;
            case Tuning.Just:
                Chord.SetPitch(just);
                break;
            case Tuning.Mean:
                Chord.SetPitch(mean);
                break;
        }
    }

    public Chord GetChordAdvanced(Gamepad gamepad)
    {
        Note.Name rootName;
        Note.Name thirdName;
        Note.Name fifthName;
        Note.Name topName;

        string rootPos;

        bool up = gamepad.dpad.up.isPressed; // G (5)
        bool right = gamepad.dpad.right.isPressed; // F (4)
        bool down = gamepad.dpad.down.isPressed; // C (1)
        bool left = gamepad.dpad.left.isPressed; // D (2)

        if (up || right || down || left)
        {
            if (down) // C
                rootName = Note.Name.C;
            else if (right) // F
                rootName = Note.Name.F;
            else if (left) // D
                rootName = Note.Name.D;
            else // G
                rootName = Note.Name.G;

            bool rsDead = gamepad.rightStick.ReadValue().magnitude < 0.4f;
            bool rsVert = Mathf.Abs(gamepad.rightStick.y.ReadValue()) >= Mathf.Abs(gamepad.rightStick.x.ReadValue());
            bool rsUp = !rsDead && gamepad.rightStick.y.ReadValue() > 0.0f && rsVert;
            bool rsDown = !rsDead && gamepad.rightStick.y.ReadValue() < -0.0f && rsVert;
            bool rsRight = !rsDead && gamepad.rightStick.x.ReadValue() > 0.0f && !rsVert;
            bool rsLeft = !rsDead && gamepad.rightStick.x.ReadValue() < -0.0f && !rsVert;

            if (rsUp)
                rootName = Note.Up(rootName, 2);
            else if (rsDown)
                rootName = Note.Down(rootName, 2);
            else if (rsRight)
                rootName = Note.Up(rootName);
            else if (rsLeft)
                rootName = Note.Down(rootName);

            bool north = gamepad.buttonNorth.isPressed;
            bool east = gamepad.buttonEast.isPressed;
            bool south = gamepad.buttonSouth.isPressed;
            bool west = gamepad.buttonWest.isPressed;

            if (south)
                thirdName = Note.Up(rootName, 3);
            else
                thirdName = Note.Up(rootName, 4);

            if (west)
            {
                if (south)
                    fifthName = Note.Up(rootName, 6);
                else
                    fifthName = Note.Up(rootName, 8);
            }
            else
                fifthName = Note.Up(rootName, 7);

            if (north)
            {
                if (east)
                    topName = Note.Down(rootName, 3);
                else
                    topName = Note.Down(rootName, 1);
            }
            else
            {
                if (east)
                    topName = Note.Down(rootName, 2);
                else
                    topName = rootName;
            }
        }
        else
            return null;

        Chord chord = new(
            rootName,
            thirdName,
            fifthName,
            topName
        );

        return chord;
    }
    public Chord GetChordMelodic(Gamepad gamepad)
    {
        // Default values to stop compiler from giving errors
        Note.Name rootName = Note.Name.C;
        Note.Name thirdName = Note.Name.E;
        Note.Name fifthName = Note.Name.G;
        Note.Name topName = Note.Name.C;

        string rootPos;

        // TODO

        Chord chord = new(
            rootName,
            thirdName,
            fifthName,
            topName
        );

        return chord;
    }

    public Chord GetChordScale(Gamepad gamepad)
    {
        string rootPos;

        bool one = gamepad.dpad.down.wasPressedThisFrame;
        bool two = gamepad.dpad.left.wasPressedThisFrame;
        bool three = gamepad.dpad.right.wasPressedThisFrame;
        bool four = gamepad.dpad.up.wasPressedThisFrame;
        bool five = gamepad.buttonSouth.wasPressedThisFrame;
        bool six = gamepad.buttonWest.wasPressedThisFrame;
        bool seven = gamepad.buttonEast.wasPressedThisFrame;
        bool eight = gamepad.buttonNorth.wasPressedThisFrame;

        bool silence = gamepad.leftTrigger.wasPressedThisFrame;
        bool spicy = gamepad.rightTrigger.isPressed;

        int degree = 0;

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
        else if (eight)
            degree = 8;

        Note.Name[] names = mode.GetNames(degree, spicy);

        Note.Name rootName = Note.Up(names[0], offset);
        Note.Name thirdName = Note.Up(names[1], offset);
        Note.Name fifthName = Note.Up(names[2], offset);
        Note.Name topName = Note.Up(names[3], offset);

        if (names.Length == 4)
            return new Chord(
                rootName,
                thirdName,
                fifthName,
                topName
            );
        else
            return new Chord(
                rootName,
                thirdName,
                fifthName,
                topName,
                Note.Up(names[4], offset)
            );
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
    private static AudioController.Pitch pitch { get; set; }

    public static void SetPitch(AudioController.Pitch newPitch)
    {
        pitch = newPitch;
    }

    public enum RootLocation { BbC, DbEb, EGb, GA }
    public enum ThirdTypeE { Minor, Major }
    public enum FifthTypeE { Diminished, Perfect, Augmented }
    public enum SeventhTypeE { Diminished, Minor, Major, None }

    public Chord(Note.Name rootName, Note.Name thirdName, Note.Name fifthName, Note.Name topName)
    : this(rootName, thirdName, fifthName, topName, rootName) { }

    public Chord(Note.Name rootName, Note.Name thirdName, Note.Name fifthName, Note.Name topName, Note.Name bassName)
    {
        RootName = rootName;
        ThirdName = thirdName;
        FifthName = fifthName;
        TopName = topName;
        BassName = bassName;

        RootLoc = RootName switch
        {
            <= Note.Name.C => RootLocation.BbC,
            >= Note.Name.Db and <= Note.Name.Eb => RootLocation.DbEb,
            >= Note.Name.E and <= Note.Name.Gb => RootLocation.EGb,
            >= Note.Name.G => RootLocation.GA
        };

        int numNotes = Enum.GetNames(typeof(Note.Name)).Length;

        ThirdType = ((ThirdName - RootName + numNotes) % numNotes) switch
        {
            3 => ThirdTypeE.Minor,
            4 => ThirdTypeE.Major
        };

        FifthType = ((FifthName - RootName + numNotes) % numNotes) switch
        {
            6 => FifthTypeE.Diminished,
            7 => FifthTypeE.Perfect,
            8 => FifthTypeE.Augmented
        };

        SeventhType = ((TopName - RootName + numNotes) % numNotes) switch
        {
            0 => SeventhTypeE.None,
            9 => SeventhTypeE.Diminished,
            10 => SeventhTypeE.Minor,
            11 => SeventhTypeE.Major
        };

        Root = Mathf.Pow(pitch.semitone, (int)RootName - 2);
        Bass = Mathf.Pow(pitch.semitone, (int)BassName - 2);

        Third = ThirdType switch
        {
            ThirdTypeE.Minor => Root * pitch.m3,
            ThirdTypeE.Major => Root * pitch.M3
        };
        Fifth = FifthType switch
        {
            FifthTypeE.Diminished => Root * pitch.d5,
            FifthTypeE.Perfect => Root * pitch.p5,
            FifthTypeE.Augmented => Root * pitch.a5
        };
        Top = SeventhType switch
        {
            SeventhTypeE.Diminished => Root * pitch.d7,
            SeventhTypeE.Minor => (ThirdType == ThirdTypeE.Minor) ? Root * pitch.m7 : Root * pitch.h7,
            SeventhTypeE.Major => Root * pitch.M7,
            SeventhTypeE.None => Root * pitch.p8,
        };
    }

    public float Root { get; }
    public float Third { get; }
    public float Fifth { get; }
    public float Top { get; }
    public float Bass { get; }
    public Note.Name RootName { get; }
    public Note.Name ThirdName { get; }
    public Note.Name FifthName { get; }
    public Note.Name TopName { get; }
    public Note.Name BassName { get; }
    public RootLocation RootLoc { get; }
    public ThirdTypeE ThirdType { get; }
    public FifthTypeE FifthType { get; }
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