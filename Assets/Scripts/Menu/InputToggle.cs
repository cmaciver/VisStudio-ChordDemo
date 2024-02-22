using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToggle : ToggleAbstract
{
    [SerializeField] MenuInteractableAbstract key;
    [SerializeField] MenuInteractableAbstract mode;

    public override void SwitchTo(int index)
    {
        WandController wandController = FindObjectOfType<WandController>();
        switch (index)
        {
            case 0:
                wandController.SetLayout(WandController.Layout.Scale);
                key.SetInteractable(true);
                mode.SetInteractable(true);
                break;
            case 1:
                wandController.SetLayout(WandController.Layout.Advanced);
                key.SetInteractable(false);
                mode.SetInteractable(false);
                break;
        }

    }
}
