using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomThrowable : MonoBehaviour
{
    private string throwableName;
    private Vector3 startPos;
    private Vector3 landingPos;
    private Rigidbody rb;

    public BallVariants type;
    [System.NonSerialized]
    public CustomBlackboard blackboard;
    [System.NonSerialized]
    public Observer observer;

    [System.NonSerialized]
    public bool isThrown;
    [System.NonSerialized]
    public bool isGrabbed;
    [System.NonSerialized]
    public bool collisionHandled;
    
    // Start is called before the first frame update
    void Start()
    {
        this.blackboard = FindObjectOfType<CustomBlackboard>();
        this.observer = FindObjectOfType<Observer>();
        this.isThrown = this.collisionHandled = this.isGrabbed = false;
        this.throwableName = this.gameObject.name;
        this.startPos = this.transform.position;
        this.rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown) {
            //print("object thrown from " + this.transform.position);
            // TODO: see what/why here
            this.isThrown = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // only handle first collision.
        if (this.collisionHandled)
            return;

        if (col.gameObject.name == "field")
        {
            landingPos = rb != null ? rb.position : col.transform.position;
            // TODO: make sure this is necessary or not
            //observer.throwNumber++;
            // to avoid bouncing on target to be counted as hit.
            this.collisionHandled = true;
            //print("collided with field?");
        }

        if (col.gameObject.name.Contains("target"))
        {
            landingPos = rb != null ? rb.position : col.transform.position;
            observer.throwNumber++;
            this.collisionHandled = true;

            print("collided with target?");
        }
    }
}
