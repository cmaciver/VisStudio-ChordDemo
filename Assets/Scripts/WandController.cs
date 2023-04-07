using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WandController : MonoBehaviour
{
    private AudioController ac;
    // Start is called before the first frame update
    void Start()
    {
        ac = new(AudioController.Tuning.Equal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Gamepad gamepad = Gamepad.all[0];

        if (gamepad.leftTrigger.wasPressedThisFrame)
        {
            print(ac.GetChord(gamepad)?.Seventh);
        }

    }
}
