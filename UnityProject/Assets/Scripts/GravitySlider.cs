using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySlider : MonoBehaviour
{
    
    public void AdjustGrav(float slider_value)
    {
        Vector3 oldGrav = Physics.gravity;
        Physics.gravity = new Vector3(0.0f, -1.0f * slider_value, 0.0f);
    }
}
