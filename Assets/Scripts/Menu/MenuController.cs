using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] SubMenuAbstract head;
    [SerializeField] SubMenuAbstract[] other;

    public float volume = 0.75f;

    public void Update()
    {
        if (Gamepad.all.Count > 0)
        {
            Gamepad gamepad = Gamepad.all[0];

            if (gamepad.startButton.wasPressedThisFrame)
                Menu();

            if (gamepad.buttonSouth.wasPressedThisFrame)
                Select();

            if (gamepad.buttonEast.wasPressedThisFrame)
                Back();

            if (gamepad.dpad.up.wasPressedThisFrame)
                NavigateUp();
            if (gamepad.dpad.down.wasPressedThisFrame)
                NavigateDown();
            if (gamepad.dpad.left.wasPressedThisFrame)
                NavigateLeft();
            if (gamepad.dpad.right.wasPressedThisFrame)
                NavigateRight();
        }
    }

    public void Menu()
    {
        if (menuCanvas.activeSelf)
            Close();
        else
            Open();
    }

    private void Open()
    {
        foreach (SubMenuAbstract menu in other)
            menu.gameObject.SetActive(false);

        foreach (WandController wandController in FindObjectsOfType<WandController>())
            wandController.DisableInput();

        menuCanvas.SetActive(true);
        head.Open();

        AudioListener.volume = volume * 0.5f;
    }

    private void Close()
    {
        foreach (WandController wandController in FindObjectsOfType<WandController>())
            wandController.EnableInput();

        menuCanvas.SetActive(false);

        AudioListener.volume = volume;
    }

    public void NavigateUp()
    {
        if (menuCanvas.activeSelf)
            head.NavigateUp();
    }

    public void NavigateDown()
    {
        if (menuCanvas.activeSelf)
            head.NavigateDown();
    }

    public void NavigateLeft()
    {
        if (menuCanvas.activeSelf)
            head.NavigateLeft();
    }

    public void NavigateRight()
    {
        if (menuCanvas.activeSelf)
            head.NavigateRight();
    }

    public void Select()
    {
        if (menuCanvas.activeSelf)
            head.Select();
    }

    public void Back()
    {
        if (menuCanvas.activeSelf)
            if (head.Back())
                Close();
    }
}