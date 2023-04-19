using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = gameObject.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all[0].selectButton.wasPressedThisFrame)
        {
            isActive = !isActive;
            gameObject.GetComponent<Renderer>().enabled = isActive;
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
