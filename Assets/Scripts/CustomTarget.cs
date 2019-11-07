using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomTarget : MonoBehaviour
{
    [NonSerialized]
    public Vector3 localCenter;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: probably not necessary, check maybe and remove..
        this.localCenter = this.transform.localPosition;
    }
}
