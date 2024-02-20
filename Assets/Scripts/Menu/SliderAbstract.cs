using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliderAbstract : MenuInteractableAbstract
{
    [SerializeField] GameObject knob;
    private float leftPos;

    private const int NOTCHES = 9;
    private int current;

    public void Init(int index)
    {
        leftPos = knob.transform.localPosition.x;
        current = index;
        knob.transform.localPosition = new(leftPos + current * 55, knob.transform.localPosition.y, 0);
    }

    public override void NavigateLeft()
    {
        if (current > 0)
        {
            current--;
            knob.transform.localPosition = new(leftPos + current * 55, knob.transform.localPosition.y, 0);
            SwitchTo(current);
        }
    }

    public override void NavigateRight()
    {
        if (current < NOTCHES - 1)
        {
            current++;
            knob.transform.localPosition = new(leftPos + current * 55, knob.transform.localPosition.y, 0);
            SwitchTo(current);
        }
    }

    public override void Select()
    {
        // Empty
    }

    public abstract void SwitchTo(int index);
}
