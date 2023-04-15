using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class FunkySky : MonoBehaviour
{
    private Camera[] cameras;
    private AudioController ac;
    private Chord lastChord;

    //[SerializeField] private GameObject wand;
    
    // Start is called before the first frame update
    void Start()
    {
        ac = new(AudioController.Tuning.Equal);

        cameras = Camera.allCameras;

        //foreach (Camera cam in cameras)
        //{
        //    cam.clearFlags = CameraClearFlags.SolidColor;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        Chord currChord = ac.GetChord(Gamepad.all[0]);

        if (currChord != null)
        {
            lastChord = currChord;
        } else
        {
            return;
        }

        float root = lastChord.Root;

        Color bgColor = GetColor(root);

        foreach (Camera cam in cameras)
        {
            cam.backgroundColor = bgColor;
        }
    }

    public Color GetColor(float root)
    {
        Color bgColor;

        if (root >= 2)
        {
            return GetColor(root / 2);
        } 
        else if (root < 1) 
        {
            return GetColor(root * 2);
        }

        // B: Purple-Red
        if (root >= 1.88f)
        {
            bgColor = new Color(245, 66, 156);
        }

        // Bb: Magenta
        else if (root >= 1.77f)
        {
            bgColor = new Color(245, 66, 245);
        }

        // A: Blurple
        else if (root >= 1.67f)
        {
            bgColor = new Color(156, 66, 245);
        }

        // Ab: Blue
        else if (root >= 1.58f)
        {
            bgColor = new Color(66, 66, 245);
        }

        // G: Turquoise
        else if (root >= 1.49f)
        {
            bgColor = new Color(66, 245, 156);
        }

        // Gb: Green
        else if (root >= 1.41f)
        {
            bgColor = new Color(66, 245, 66);
        }

        // F: Yellow-Green
        else if (root >= 1.33f)
        {
            bgColor = new Color(156, 245, 66);
        }

        // E: Yellow
        else if (root >= 1.25f)
        {
            bgColor = new Color(245, 245, 66);
        }

        // Eb: Orange-Yellow
        else if (root >= 1.18f)
        {
            bgColor = new Color(245, 215, 66);
        }

        // D: Orange
        else if (root >= 1.12f)
        {
            bgColor = new Color(245, 156, 66);
        }

        // Db: Red-Orange
        else if (root >= 1.05f)
        {
            bgColor = new Color(245, 100, 66);
        }

        // C: Red
        else
        {
            bgColor = new Color(245, 66, 66);
        }

        bgColor.r /= 255;
        bgColor.g /= 255;
        bgColor.b /= 255;

        return bgColor;
    }
}

/**
 * Notes:                        | Colors:
 * C = 1                         | Red
 * C# = 1.05946309436f           | 
 * D = 1.12246204831f            | Orange
 * D# = 1.18920711500f           | 
 * E = 1.25992104990f            | Yellow
 * F = 1.33483985417f            | 
 * F# = 1.41421356238f           | Green
 * G = 1.49830707688f            | 
 * G# = 1.58740105198f           | Blue
 * A = 1.68179283052f            |
 * A# = 1.78179743629f           | Indigo
 * B = 1.88774862538f            |
 * B#                            | Violet
 * C = 2
 */






























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