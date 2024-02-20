using System;
using System.Collections.Generic;
using UnityEngine;

public class ModeList : ListAbstract
{
    public void Start()
    {
        Init(Enum.GetNames(typeof(AudioController.ScaleMode)), 0);
    }

    public override void SwitchTo(int index)
    {
        FindObjectOfType<WandController>().SetMode((AudioController.ScaleMode) Enum.GetValues(typeof(AudioController.ScaleMode)).GetValue(index));
    }
}
