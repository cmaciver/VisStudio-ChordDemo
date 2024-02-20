using UnityEngine;

public abstract class SubMenuAbstract : MonoBehaviour
{
    [SerializeField] SubMenuAbstract parent;
    private SubMenuAbstract currentMenu;

    public SubMenuAbstract()
    {
        currentMenu = this;
    }

    public abstract void Open();
    public abstract void NavigateUp();
    public abstract void NavigateDown();
    public abstract void NavigateLeft();
    public abstract void NavigateRight();
    public abstract void Select();

    /**
     * Enact back button press on this SubMenu.
     * 
     * @return true if the back button press closed this SubMenu, false otherwise
     */
    public abstract bool Back();

    public SubMenuAbstract Parent()
    {
        return parent;
    }

    public SubMenuAbstract GetCurrent()
    {
        return currentMenu;
    }

    public void SetCurrent(SubMenuAbstract menu)
    {
        currentMenu = menu;
    }
}
