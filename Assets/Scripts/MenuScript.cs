using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;

    private bool isActive;

    private enum whichOption { tuning, soundfont }

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all[0].selectButton.wasPressedThisFrame)
        {
            isActive = !isActive;
            canvas.SetActive(isActive);
        }

        if (Gamepad.all[0].dpad.down.wasPressedThisFrame)
        {

        }

        if (Gamepad.all[0].dpad.up.wasPressedThisFrame)
        {

        }

        if (Gamepad.all[0].buttonSouth.wasPressedThisFrame)
        {

        }
    }

    public void ChangeScheme(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //schemenum = (schemenum + 1) % 2; // TODO

        }

        gameObject.SetActive(true);
    }
}
