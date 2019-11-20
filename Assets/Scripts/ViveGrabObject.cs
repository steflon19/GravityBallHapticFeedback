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
    private int posToSave = 9;
    private Observer observer;


    // Start is called before the first frame update
    void Start()
    {
        lastPosList = new List<Vector3>();
        observer = FindObjectOfType<Observer>();
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
        CustomThrowable throwable = null;
        if (observer.ActiveSceneType == SceneType.VR_Scene)
        {
            throwable = collidingObject.GetComponent<CustomThrowable>();
            // this is just because for some reason the throwable was nested. shouldnt be necessary.
            if (!throwable) throwable = collidingObject.GetComponentInParent<CustomThrowable>();
        }
        else if (observer.ActiveSceneType == SceneType.MR_Scene && observer.spawner.throwableGrabReady)
        {
            throwable = observer.spawner.activeThrowable;
            throwable.gameObject.SetActive(true);
            collidingObject = throwable.gameObject;
            observer.spawner.throwableGrabReady = false;
            Debug.Log("grabbing set active? ? " + throwable);
        }
        else {
            Debug.LogWarning("grab in MR with nothing ready.");
            return;
        }
        if (throwable)
        {
            objectInHand = collidingObject;
            collidingObject = null;

            var joint = AddFixedJoint();
            Debug.Log(objectInHand.name + " - " + objectInHand.transform.localPosition);
            // TODO: maybe remove this  if and just take the else
            if (throwable.snapAnchor)
            {
                throwable.transform.up = snapAnchor.transform.up;
                throwable.transform.position = snapAnchor.transform.position;
                Vector3 posDif = snapAnchor.transform.position - throwable.snapAnchor.transform.position;
                throwable.transform.position += posDif;
            }
            else
            {
                objectInHand.transform.position = snapAnchor.transform.position;
                objectInHand.transform.up = snapAnchor.transform.up;
            }
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
            foreach (FixedJoint joint in GetComponents<FixedJoint>())
            {
                joint.connectedBody = null;
                Destroy(joint);
            }

            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

            float lf = Time.fixedDeltaTime;
            if(lastPosList.Count <= 0 ) Debug.LogError("lastPosList empty on release");

            Vector3 dir = rb.position - lastPosList[0];

            float timeSquared = Mathf.Pow(lf * (lastPosList.Count - 1), 2);


            Vector3 force = (dir/(timeSquared)) * (rb.mass);
            force /= 10f;

            float yAngle = Mathf.Asin(force.normalized.y) * Mathf.Rad2Deg;
            
            observer.AddCurrentThrowData(rb.position, yAngle, force);
            rb.AddForce(force, ForceMode.Impulse);

            rb.angularVelocity = controllerPose.GetAngularVelocity();

            observer.WriteThrowToBlackboard(force.magnitude, yAngle);

            if (objectInHand.GetComponent<CustomThrowable>())
            {
                objectInHand.GetComponent<CustomThrowable>().isThrown = true;
                objectInHand.GetComponent<CustomThrowable>().isGrabbed = false;
            }
            
        }
        objectInHand = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject)
            {
                GrabObject();
            } else if(observer.ActiveSceneType == SceneType.MR_Scene)
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
