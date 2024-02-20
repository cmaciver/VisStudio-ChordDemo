using UnityEngine;

public class SettingsController : SubMenuAbstract
{
    [SerializeField] MenuInteractableAbstract[] menuInteractables;
    [SerializeField] GameObject hover;

    private int hovered;

    public void Start()
    {
        foreach (MenuInteractableAbstract interactable in menuInteractables)
            interactable.Init(this);
    }

    public override void NavigateDown()
    {
        if (GetCurrent() == this)
        {
            hovered++;
            if (hovered >= menuInteractables.Length)
                hovered = 0;

            hover.transform.position = menuInteractables[hovered].transform.position;
        }
        else
            GetCurrent().NavigateDown();
    }

    public override void NavigateLeft()
    {
        if (GetCurrent() == this)
            menuInteractables[hovered].NavigateLeft();
        else
            GetCurrent().NavigateLeft();
    }

    public override void NavigateRight()
    {
        if (GetCurrent() == this)
            menuInteractables[hovered].NavigateRight();
        else
            GetCurrent().NavigateRight();
    }

    public override void NavigateUp()
    {
        if (GetCurrent() == this)
        {
            hovered--;
            if (hovered < 0)
                hovered = menuInteractables.Length - 1;

            hover.transform.position = menuInteractables[hovered].transform.position;
        }
        else
            GetCurrent().NavigateUp();
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        SetCurrent(this);
        hovered = 0;
        hover.transform.position = menuInteractables[hovered].transform.position;
    }

    public override void Select()
    {
        if (GetCurrent() == this)
            menuInteractables[hovered].Select();
        else
            GetCurrent().Select();
    }

    public override bool Back()
    {
        if (GetCurrent() == this)
            return true;

        if (GetCurrent().Back())
            SetCurrent(this);

        return false;
    }
}
