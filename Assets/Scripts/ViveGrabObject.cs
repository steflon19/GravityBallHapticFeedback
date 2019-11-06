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
    private Vector3 lastPos;
    private List<Vector3> lastPosList;
    private int posToSave = 5;


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
        //print("object grabbed " + objectInHand.name);
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();

        if (objectInHand.GetComponent<CustomThrowable>())
        {
            //print("grabbed throwable");
            objectInHand.GetComponent<CustomThrowable>().isGrabbed = true;
        }
    }

    // 3
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
            Destroy(GetComponent<FixedJoint>());
            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

            float lf = Time.fixedDeltaTime;
            Vector3 dir = rb.position - lastPosList[0];
            float dist = Vector3.Distance(rb.position, lastPos);
            float v = 0;
            Vector3 avgDist = GetDistAvg();
            v = (dist);
            Debug.Log("average " + dist + v);
            Vector3 velo = controllerPose.GetVelocity();

            // v = rb.mass * (((dist / lf)/lf);
            //Vector3 force = dist * (rb.mass * v.magnitude);
            Vector3 force = (rb.mass) * (v) * dir;
            // force = rb.mass * velo;
            Debug.Log("force " + force);
            //rb.AddForce(force, ForceMode.Impulse);

            rb.velocity = (avgDist / lf) * (1/rb.mass);

            rb.angularVelocity = controllerPose.GetAngularVelocity();
            // add some check for a proper throw?

            if (objectInHand.GetComponent<CustomThrowable>())
            {
                //print("threw throwable");
                objectInHand.GetComponent<CustomThrowable>().isThrown = true;
                objectInHand.GetComponent<CustomThrowable>().isGrabbed = false;
            }

        }
        objectInHand = null;
    }

    Vector3 GetDistAvg() {
        Vector3 dist = Vector3.zero;
        int index = 0;
        foreach (Vector3 pos in lastPosList) {
            if (index == 0) {
                index++;
                continue;
            }

            dist += (pos - lastPosList[index - 1]);

            index++;
        }
        dist.x /= (lastPosList.Count - 1);
        dist.y /= (lastPosList.Count - 1);
        dist.z /= (lastPosList.Count - 1);
        return dist; // (lastPosList.Count - 1);
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
            lastPos = objectInHand.GetComponent<Rigidbody>().position;
            if (lastPosList.Count >= posToSave) {
                lastPosList.RemoveAt(0);
            }
            lastPosList.Add(lastPos);
        }
    }
}
