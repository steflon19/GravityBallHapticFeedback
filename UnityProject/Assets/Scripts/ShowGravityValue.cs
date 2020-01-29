using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGravityValue : MonoBehaviour
{
    Text gravityText;
    // Start is called before the first frame update
    void Start()
    {
        gravityText = GetComponent<Text>();
    }

    public void textUpdate(float value)
    {
        gravityText.text = Math.Round(value, 2) + " m/s^2";
    }
}
