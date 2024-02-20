using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : ButtonAbstract
{
    [SerializeField] SubMenuAbstract credits;

    public override void Select()
    {
        SubMenuAbstract container = GetContainer();
        container.SetCurrent(credits);
        container.gameObject.SetActive(false);
        credits.Open();
    }
}
