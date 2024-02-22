using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : SliderAbstract
{
    [SerializeField] MenuController menuController;

    public void Start()
    {
        Init(6);
    }

    public override void SwitchTo(int index)
    {
        menuController.volume = Mathf.Pow(2, (index - 8) * 0.5f);
        StartCoroutine(menuController.Fade(menuController.volume * 0.5f));
    }
}
