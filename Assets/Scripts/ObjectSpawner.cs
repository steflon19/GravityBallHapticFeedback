using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Vector2 extends;
    // init pos 13, 10.2, 239
    public GameObject targetBase;
    private GameObject activeTarget;

    public List<CustomThrowable> Balls;
    private CustomThrowable activeBall;

    private Vector3 BallStartPosition;

    private Observer observer;
    // Start is called before the first frame update
    void Start()
    {
        // TODO: Set values properly, currently for debugging.
        BallStartPosition = new Vector3(203.5f, 11f, 130.5f);// new Vector3(206.4f, 11f, 130f);
        observer = FindObjectOfType<Observer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(activeTarget) Destroy(activeTarget);
            float x = targetBase.transform.localPosition.x;
            float z = targetBase.transform.localPosition.z;
            float y = targetBase.transform.localPosition.y;
            float scale = Random.Range(0.5f, 1.3f);
            Vector3 newPos = new Vector3(Random.Range(x, x + extends.x), y, Random.Range(z, z + extends.y));
            activeTarget = Instantiate(targetBase);
            activeTarget.transform.parent = this.transform;
            activeTarget.transform.localPosition = newPos;
            activeTarget.transform.localScale = new Vector3(scale, scale, 1);
            activeTarget.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.B))
        {
            if (activeBall) Destroy(activeBall.gameObject);
            CustomThrowable ballToSpawn = Balls.Find(b => b.type == observer.activeBallVariant);
            activeBall = Instantiate(ballToSpawn);
            activeBall.transform.position = BallStartPosition;
            Debug.Log("stuff " + activeBall);
            // TODO: REMOVE THIS LINE
            observer.throwNumber++;
        }


    }
}
