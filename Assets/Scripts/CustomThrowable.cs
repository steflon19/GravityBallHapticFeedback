using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomThrowable : MonoBehaviour
{
    private string throwableName;
    private Vector3 startPos;

    public ThrowableVariants type;
    //public GameObject snapAnchor;
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
        // only handle first collision. to prevent bouncing etc
        if (this.collisionHandled)
            return;
        // if the ball is just dropped, not thrown, we dont need to do anything further.
        if (col.gameObject.CompareTag("ground"))
        {
            this.collisionHandled = true;
            return;
        }

        CustomTarget target = null;
        if(observer.spawner.activeTarget)
            target = observer.spawner.activeTarget.GetComponentInChildren<CustomTarget>(); // col.gameObject.GetComponent<CustomTarget>();

        if (target == null) {
            Debug.Log("target error");
        }

        if (col.gameObject.name == "field" && !this.collisionHandled)
        {
            // avoid showing bounce collision as valid throw
            collisionHandled = true;

            Vector3 contactPoint = col.GetContact(0).point; // col.transform.InverseTransformPoint(col.GetContact(0).point);
            Vector3 tarPos;
            if (target)
                tarPos = target.transform.position;
            else
                tarPos = new Vector3(100, 100, 100);
            // 1.5 is wild! arbitrary. PROBABLY because of the parent scale 3 and so halfed = 1.5?
            float distance = Mathf.Clamp(Vector3.Distance(tarPos, contactPoint) / 1.5f, 0, 1);

            // convert 0..1 to 1..4
            float points = Mathf.Ceil((1 - distance) / 0.25f);// Mathf.Clamp(Mathf.Round(4*((float)Math.Round(distance, 1))), 1, 4);
            observer.HandleThrowableHit(this, (int)points);
            print("throwable \"" + gameObject.name + "\" hit target, distance to center: " + distance + " and points: " + points);
        }
        ////CustomTarget target = col.gameObject.GetComponent<CustomTarget>();
        //if (target && !this.collisionHandled)
        //{
        //    // avoid showing bounce collision as valid throw
        //    collisionHandled = true;

        //    Vector3 contactPoint = col.transform.InverseTransformPoint(col.GetContact(0).point);
        //    float distance = Mathf.Clamp(Vector3.Distance(target.localCenter, contactPoint) * 2, 0, 1);

        //    // convert 0..1 to 1..4
        //    float points = Mathf.Ceil((1 - distance) / 0.25f);// Mathf.Clamp(Mathf.Round(4*((float)Math.Round(distance, 1))), 1, 4);
        //    observer.HandleThrowableHit(this, (int)points);
        //    print("throwable \"" + gameObject.name + "\" hit target, distance to center: " + distance + " and points: " + points);
        //}
    }
}
