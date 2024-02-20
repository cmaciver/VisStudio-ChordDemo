public class CreditsController : SubMenuAbstract
{
    public override void NavigateDown()
    {
        // Empty
    }

    public override void NavigateLeft()
    {
        // Empty
    }

    public override void NavigateRight()
    {
        // Empty
    }

    public override void NavigateUp()
    {
        // Empty
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Select()
    {
        // Empty
    }

    public override bool Back()
    {
        gameObject.SetActive(false);
        Parent().gameObject.SetActive(true);
        return true;
    }
}
