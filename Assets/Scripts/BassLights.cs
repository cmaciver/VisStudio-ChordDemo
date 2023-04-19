using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassLights : MonoBehaviour
{
    private Material mat;

    private Color destinationColor;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        Debug.Log(GetComponent<Renderer>().bounds.size);
    }

    private void Update()
    {
        mat.color = Color.Lerp(mat.color, destinationColor, 0.05f);
    }

    public void ChangeColor(Color note)
    {
        destinationColor = note;
    }
}
