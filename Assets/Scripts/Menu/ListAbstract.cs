using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class ListAbstract : MenuInteractableAbstract
{
    [SerializeField] TextMeshProUGUI valueField;

    private string[] names;
    private int current;

    public void Init(string[] names, int index)
    {
        this.names = names;
        current = index;
        valueField.text = names[current];

        for (int i = 0; i < names.Length; i++)
            names[i] = names[i].Replace('_', ' ');
    }

    public override void NavigateLeft()
    {
        current--;
        if (current < 0)
            current = names.Length - 1;
        valueField.text = names[current];
        SwitchTo(current);
    }

    public override void NavigateRight()
    {
        current++;
        if (current >= names.Length)
            current = 0;
        valueField.text = names[current];
        SwitchTo(current);
    }

    public override void Select()
    {
        // Empty
    }

    public abstract void SwitchTo(int index);
}
