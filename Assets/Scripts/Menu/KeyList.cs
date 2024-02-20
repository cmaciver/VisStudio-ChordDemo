using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyList : ListAbstract
{
    public void Start()
    {
        Init(Enum.GetNames(typeof(Note.Name)), 2);
    }

    public override void SwitchTo(int index)
    {
        FindObjectOfType<WandController>().SetKey((Note.Name) Enum.GetValues(typeof(Note.Name)).GetValue(index));
    }
}
