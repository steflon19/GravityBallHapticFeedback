using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OriginalPosition : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void resetBalls ()
    {
        transform.position = startPos;
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
