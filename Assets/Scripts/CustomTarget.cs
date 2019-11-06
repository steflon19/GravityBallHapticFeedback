using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomTarget : MonoBehaviour
{
    private Vector3 localCenter;

    public CustomBlackboard blackboard;
    public Observer observer;

    // Start is called before the first frame update
    void Start()
    {
        this.localCenter = this.transform.localPosition;
    }

    void OnCollisionEnter(Collision col)
    {
        CustomThrowable throwable = col.gameObject.GetComponent<CustomThrowable>();
        if (throwable && !throwable.collisionHandled)
        {
            // avoid showing bounce collision as valid throw
            throwable.collisionHandled = true;

            Vector3 contactPoint = transform.InverseTransformPoint(col.GetContact(0).point);
            // print("col.GetContact(0) " + contactPoint.x + " " + contactPoint.y);
            float distance = Mathf.Clamp(Vector3.Distance(this.localCenter, contactPoint) * 2, 0, 1);

            // convert 0..1 to 1..4
            float points = Mathf.Ceil((1 - distance) / 0.25f);// Mathf.Clamp(Mathf.Round(4*((float)Math.Round(distance, 1))), 1, 4);
            print("throwable \"" + col.gameObject.name + "\" hit target, distance to center: " + distance + " and points: " + points);
        }
    }
}
