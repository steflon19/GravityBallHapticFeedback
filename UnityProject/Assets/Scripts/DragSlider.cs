using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSlider : MonoBehaviour
{
    Rigidbody rb;
    public void AdjustDrag(float slider_value) {
        rb = GetComponent<Rigidbody>();
        if (rb) {
            rb.drag = slider_value;
        }
    }
}
