using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker
{
    /**
     * Converts a note name into a corresponding color
     * @param note the note name to be converted into a color
     * @return a color corresponding to the note name input
     */
    public static Color GetColor(Note.Name note)
    {
        Color bgColor = Color.red;

        switch (note)
        {
            case Note.Name.C:
                bgColor = new Color(245, 66, 66);
                break;
            case Note.Name.Db:
                bgColor = new Color(245, 100, 66);
                break;
            case Note.Name.D:
                bgColor = new Color(245, 156, 66);
                break;
            case Note.Name.Eb:
                bgColor = new Color(245, 215, 66);
                break;
            case Note.Name.E:
                bgColor = new Color(245, 245, 66);
                break;
            case Note.Name.F:
                bgColor = new Color(156, 245, 66);
                break;
            case Note.Name.Gb:
                bgColor = new Color(66, 245, 66);
                break;
            case Note.Name.G:
                bgColor = new Color(66, 245, 156);
                break;
            case Note.Name.Ab:
                bgColor = new Color(66, 66, 245);
                break;
            case Note.Name.A:
                bgColor = new Color(156, 66, 245);
                break;
            case Note.Name.Bb:
                bgColor = new Color(245, 66, 245);
                break;
            case Note.Name.B:
                bgColor = new Color(245, 66, 156);
                break;
            default:
                bgColor = new Color(0, 0, 0);
                break;
        }

        bgColor.r /= 255;
        bgColor.g /= 255;
        bgColor.b /= 255;

        return bgColor;
    }
}

//private Camera[] cameras;
//private AudioController ac;
//private Chord lastChord;

//[SerializeField] private GameObject wand;
//[SerializeField] private GameObject wallsParent;
//private Renderer[] walls;

//// Start is called before the first frame update
//void Start()
//{
//    ac = new(AudioController.Tuning.Equal);

//    walls = wallsParent.GetComponentsInChildren<Renderer>();

//    //foreach (Camera cam in cameras)
//    //{
//    //    cam.clearFlags = CameraClearFlags.SolidColor;
//    //}
//}

//// Update is called once per frame
//void Update()
//{
//    Chord currChord = ac.GetChord(Gamepad.all[0]);

//    if (currChord != null)
//    {
//        lastChord = currChord;
//    } else
//    {
//        return;
//    }

//    float root = lastChord.Root;

//    Note.Name name = lastChord.RootName;

//    Color bgColor = GetColor(name);

//    foreach (Renderer r in walls)
//    {
//        r.material.color = bgColor;
//    }
//}






























//private Color WavelengthToRGB(float wavelength, float gamma = 0.8f)
//{
//    float R, G, B;

//    if (wavelength >= 380 & wavelength <= 440)
//    {
//        float attenuation = 0.3f + 0.7f * (wavelength - 380) / (440 - 380);
//        R = Mathf.Pow(((-(wavelength - 440) / (440 - 380)) * attenuation),  gamma);
//        G = 0.0f;
//        B = Mathf.Pow((1.0f * attenuation), gamma);
//    }
//    else if (wavelength >= 440 & wavelength <= 490)
//    {
//        R = 0.0f;
//        G = Mathf.Pow((wavelength - 440) / (490 - 440), gamma);
//        B = 1.0f;
//    }
//    else if (wavelength >= 490 & wavelength <= 510)
//    {
//        R = 0.0f;
//        G = 1.0f;
//        B = Mathf.Pow(-(wavelength - 510) / (510 - 490), gamma);
//    }
//    else if (wavelength >= 510 & wavelength <= 580)
//    {
//        R = Mathf.Pow((wavelength - 510) / (580 - 510), gamma);
//        G = 1.0f;
//        B = 0.0f;
//    }
//    else if (wavelength >= 580 & wavelength <= 645)
//    {
//        R = 1.0f;
//        G = Mathf.Pow(-(wavelength - 645) / (645 - 580), gamma);
//        B = 0.0f;
//    }
//    else if (wavelength >= 645 & wavelength <= 750)
//    {
//        float attenuation = 0.3f + 0.7f * (750 - wavelength) / (750 - 645);
//        R = Mathf.Pow((1.0f * attenuation), gamma);
//        G = 0.0f;
//        B = 0.0f;
//    }
//    else
//    {
//        R = 0.0f;
//        G = 0.0f;
//        B = 0.0f;
//    }

//    //R *= 255;
//    //G *= 255;
//    //B *= 255;

//    return new Color(R, G, B);
//}