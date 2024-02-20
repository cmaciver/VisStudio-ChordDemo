using UnityEngine;

public abstract class MenuInteractableAbstract : MonoBehaviour
{
    private SubMenuAbstract container;
    [SerializeField] bool interactable = true;

    public abstract void NavigateLeft();
    public abstract void NavigateRight();
    public abstract void Select();

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
        }
    }
}
