using UnityEngine;

public abstract class MenuInteractableAbstract : MonoBehaviour
{
    private SubMenuAbstract container;
    [SerializeField] bool interactable = true;
    [SerializeField] CanvasRenderer hoverObject;

    private Color hoverColor = new(1,1,0);
    private Color normalColor;
    private bool hovered = false;

    public abstract void NavigateLeft();
    public abstract void NavigateRight();
    public abstract void Select();

    private void OnEnable()
    {
        if (!interactable)
            foreach (CanvasRenderer cr in gameObject.GetComponentsInChildren<CanvasRenderer>())
                cr.SetColor(cr.GetColor() / 2.0f);
        else if (hovered)
            hoverObject.SetColor(hoverColor);
    }

    public void Init(SubMenuAbstract container)
    {
        this.container = container;
    }

    public SubMenuAbstract GetContainer()
    {
        return container;
    }

    public bool GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(bool interactable)
    {
        if (interactable != this.interactable)
        {
            this.interactable = interactable;

            if (interactable)
            {
                foreach (CanvasRenderer cr in gameObject.GetComponentsInChildren<CanvasRenderer>())
                    cr.SetColor(cr.GetColor() * 2.0f);
            } else
            {
                foreach (CanvasRenderer cr in gameObject.GetComponentsInChildren<CanvasRenderer>())
                    cr.SetColor(cr.GetColor() / 2.0f);
            }
        }
    }

    public void Hover()
    {
        if (!hovered)
        {
            normalColor = hoverObject.GetColor();
            hoverObject.SetColor(hoverColor);
        }
        hovered = true;
    }

    public void Unhover()
    {
        if (hovered)
            hoverObject.SetColor(normalColor);
        hovered = false;
    }
}
