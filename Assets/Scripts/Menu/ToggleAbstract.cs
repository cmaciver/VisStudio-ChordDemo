using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToggleAbstract : MenuInteractableAbstract
{
    [SerializeField] GameObject[] backgrounds;

    private int current;

    public override void NavigateLeft()
    {
        if (current > 0)
        {
            backgrounds[current].SetActive(true);
            current--;
            backgrounds[current].SetActive(false);
            SwitchTo(current);
        }
    }

    public override void NavigateRight()
    {
        if (current < backgrounds.Length - 1)
        {
            backgrounds[current].SetActive(true);
            current++;
            backgrounds[current].SetActive(false);
            SwitchTo(current);
        }
    }

    public override void Select()
    {
        backgrounds[current].SetActive(true);
        current++;
        if (current >= backgrounds.Length)
            current = 0;
        backgrounds[current].SetActive(false);
        SwitchTo(current);
    }

    public abstract void SwitchTo(int index);
}
