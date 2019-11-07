using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveGrabObject : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    private GameObject collidingObject;
    private GameObject objectInHand;
    private List<Vector3> lastPosList;
    private int posToSave = 10;
    private Observer observer;


    // Start is called before the first frame update
    void Start()
    {
        lastPosList = new List<Vector3>();
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
    }
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();

        if (objectInHand.GetComponent<CustomThrowable>())
        {
            objectInHand.GetComponent<CustomThrowable>().isGrabbed = true;
        }
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            //Destroy(GetComponent<FixedJoint>());
            foreach (FixedJoint joint in GetComponents<FixedJoint>()) Destroy(joint);

            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

            float lf = Time.fixedDeltaTime;

            Vector3 dist = rb.position - lastPosList[lastPosList.Count - 1];
            Vector3 avgVelo = GetAvgVelocity();

            Debug.Log("velo lastPos " + dist / lf + " velo last10Avg " + avgVelo);

            Vector3 force = (rb.mass) * (avgVelo/lf);
            //rb.velocity = force;
            rb.AddForce(force, ForceMode.Force);

            rb.angularVelocity = controllerPose.GetAngularVelocity();
            // add some check for a proper throw?

            if (objectInHand.GetComponent<CustomThrowable>())
            {
                objectInHand.GetComponent<CustomThrowable>().isThrown = true;
                objectInHand.GetComponent<CustomThrowable>().isGrabbed = false;
            }
            
        }
        objectInHand = null;
    }

    // maybe try getting distance velocity and then multiply normalised direction with it?
    Vector3 GetAvgVelocity() {
        Vector3 dist = Vector3.zero;
        
        for (int i = 0; i < lastPosList.Count;i++) {
            if (i == 0) {
                continue;
            }

            dist += (lastPosList[i] - lastPosList[i - 1]);

        }
        // the average distance from one frame to the next
        Vector3 vel = dist / (Time.fixedDeltaTime * (lastPosList.Count - 1));
        // dividing the distance by frametime should give the velocity
        return vel;// (lastPosList.Count - 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }

        if (objectInHand) {
            if (lastPosList.Count >= posToSave) {
                lastPosList.RemoveAt(0);
            }
            lastPosList.Add(objectInHand.GetComponent<Rigidbody>().position);
        }
    }
}
