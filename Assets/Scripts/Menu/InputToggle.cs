using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToggle : ToggleAbstract
{
    public override void SwitchTo(int index)
    {
        WandController wandController = FindObjectOfType<WandController>();
        switch (index)
        {
            case 0:
                wandController.SetLayout(WandController.Layout.Scale);
                break;
            case 1:
                wandController.SetLayout(WandController.Layout.Advanced);
                break;
        }

    }
}
