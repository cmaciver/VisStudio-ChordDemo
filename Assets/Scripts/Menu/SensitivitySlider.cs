using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivitySlider : SliderAbstract
{
    public void Start()
    {
        Init(4);
    }

    public override void SwitchTo(int index)
    {
        FindObjectOfType<GyroController>().stickSensitivity = Mathf.Pow(2.0f, (index - 4) / 2.0f);
    }
}
