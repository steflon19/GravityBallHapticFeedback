using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSlider : MonoBehaviour
{
    Rigidbody rb;

    public void AdjustMass(float slider_value) { 
        rb = GetComponent<Rigidbody>();
        if (rb) {
            rb.mass = slider_value;
        }
    }
}
