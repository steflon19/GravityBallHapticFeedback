using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveGrabObject : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public GameObject snapAnchor;

    private GameObject collidingObject;
    private GameObject objectInHand;
    private List<Vector3> lastPosList;
    private int posToSave = 10;
    private Observer observer;


    // Start is called before the first frame update
    void Start()
    {
        lastPosList = new List<Vector3>();
        observer = GameObject.FindObjectOfType<Observer>();
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
        CustomThrowable throwable = collidingObject.GetComponent<CustomThrowable>();
        if (!throwable) throwable = collidingObject.GetComponentInParent<CustomThrowable>();
        if (throwable)
        {
            objectInHand = collidingObject;
            collidingObject = null;

            var joint = AddFixedJoint();
            Debug.Log(objectInHand.name + " - " + objectInHand.transform.localPosition);
            if (throwable.snapAnchor)
            {
                throwable.snapAnchor.transform.position = snapAnchor.transform.position;
                throwable.snapAnchor.transform.up = snapAnchor.transform.up;
                // objectInHand.transform.position = snapAnchor.transform.position;
                // objectInHand.transform.localPosition = new Vector3(0, objectInHand.transform.localPosition.y, 0); //-= throwable.snapAnchor.transform.localPosition;
            }
            else
            {
                objectInHand.transform.position = snapAnchor.transform.position;
                objectInHand.transform.up = snapAnchor.transform.up; // Quaternion.Euler(0, 0, 0);
            }
            Debug.Log(objectInHand.transform.localPosition);
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
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
            //GetComponent<FixedJoint>().connectedBody = null;
            //Destroy(GetComponent<FixedJoint>());
            foreach (FixedJoint joint in GetComponents<FixedJoint>())
            {
                joint.connectedBody = null;
                Destroy(joint);
            }

            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

            float lf = Time.fixedDeltaTime;

            Vector3 dir = rb.position - lastPosList[0];
            float dist = Vector3.Distance(rb.position, lastPosList[0]);
            //Vector3 avgVelo = GetAvgVelocity();


            //Vector3 force = ((rb.mass) * (avgVelo/lf))/10;
            //rb.velocity = force;
            //rb.AddForce(force, ForceMode.Impulse);
            float timeSquared = Mathf.Pow(lf * (lastPosList.Count - 1), 2);

            Debug.Log("velo lastPos " + dir / timeSquared);
            // --
            //float signX, signY, signZ = 1;
            //signX = Mathf.Sign(dir.x);
            //signY = Mathf.Sign(dir.y);
            //signZ = Mathf.Sign(dir.z);
            // dir.Normalize();
            //dir.x *= signX;
            //dir.y *= signY;
            //dir.z *= signZ;
            // --

            Vector3 force = (dir/(timeSquared)) * (rb.mass); // * (dist / timeSquared);
            force /= 10f;
            //Debug.Log("force " + force + " rb.mass " + rb.mass);
            //Vector3 normalizedDir = new Vector3(dir.normalized.x - 1, dir.normalized.y - 1, dir.normalized.z - 1);
            //normalizedDir *= 2f;
            //force += Physics.gravity;

            float yAngle = Mathf.Asin(force.normalized.y) * Mathf.Rad2Deg;
            Debug.Log("yAngle " + yAngle);
            //Debug.Log("mass " + rb.mass);
            
            observer.AddCurrentThrowData(rb.position, yAngle, force);
            //Debug.Log("force " + force);
            //Debug.Log("name " + rb.name);
            rb.AddForce(force, ForceMode.Impulse);
            //rb.velocity = force;

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

        /*for (int i = 0; i < lastPosList.Count;i++) {
            if (i == 0) {
                continue;
            }

            dist += (lastPosList[i] - lastPosList[i - 1]);

        }*/
        dist = lastPosList[lastPosList.Count - 1] - lastPosList[0];
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
