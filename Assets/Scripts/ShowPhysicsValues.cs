using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using HTC.UnityPlugin.Vive;

public class ShowPhysicsValues : MonoBehaviour
{
    Vector3 startPos;
    Vector3 lastPos;
    Rigidbody rb;
    Vector3 a;
    Vector3 lastv;
    float lasttime;
    float currenttime;
    BasicGrabbable grab;
    Vector3 my_velocity;
    float force;
    Text forceApplied;
    Text angleApplied;
    Text distanceTraveled;
    float angle;
    Vector3 ballLanding;
    Vector3 ballStarting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        a = lastv = my_velocity = Vector3.zero;
        grab = GetComponent<BasicGrabbable>();
        lasttime = 0;
        currenttime = 0;
        lastPos = startPos = rb.position;
        forceApplied = GameObject.Find("CanvasB1/B1ForceValue").GetComponent<Text>();
        angleApplied = GameObject.Find("CanvasB1/B1AngleValue").GetComponent<Text>();
        distanceTraveled = GameObject.Find("CanvasB1/B1DistanceValue").GetComponent<Text>();
        startPos = rb.position;
        force = 0;
        angle = 0;
        ballStarting = rb.position;
        ballLanding = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb)
        {

            float lf = Time.deltaTime;
            Vector3 dist = rb.position - lastPos;
            Vector3 v = Vector3.zero;
            


            v = (2.0f / (lf * lf)) * dist;
            if (grab.isGrabbed)
            {

                a += v - lastv;
                my_velocity = rb.velocity;
                force = rb.mass * a.magnitude;
                angle = (float)((Math.Asin(a.normalized.y) * 180.0f) / Math.PI); // convert to degrees
            }
            else
            {
                //set a to zero
                a = Vector3.zero;
            }
            lastv = v;

             
            lastPos = rb.position;
            lasttime = currenttime;
            Debug.Log("F: " + force);
            Debug.Log("A: " + angle * 4);
            Debug.Log("D: " + dist);
            Debug.Log("Velocity: " + my_velocity.magnitude);

            forceApplied.text = Math.Round(force, 2) + " N";
            angleApplied.text = Math.Round(angle, 2) + " deg";

            if (ballLanding.magnitude == 0)
            {
                distanceTraveled.text = 0 + " m";
            } 
            else {
                distanceTraveled.text = Math.Round((ballLanding-ballStarting).magnitude, 2) + " m";
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Terrain")
        {
            ballLanding = rb.position;
        }
    }
}
