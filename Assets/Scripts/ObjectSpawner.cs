using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Vector2 extends;
    // init pos 13, 10.2, 239
    public GameObject targetBase;
    [System.NonSerialized]
    public GameObject activeTarget;

    public List<CustomThrowable> Balls;
    private CustomThrowable activeBall;

    private Vector3 BallStartPosition;

    private Observer observer;
    private ViveActionHandler viveActionHandler;
    // Start is called before the first frame update
    void Start()
    {
        BallStartPosition = new Vector3(1.8f, 0.9f, 1.95f);
        observer = FindObjectOfType<Observer>();
        viveActionHandler = FindObjectOfType<ViveActionHandler>();
        activeTarget = Instantiate(targetBase);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) || viveActionHandler.GetTeleportDown())
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

        if (Input.GetKeyDown(KeyCode.B) || viveActionHandler.GetGripDown())
        {
            if (activeBall) Destroy(activeBall.gameObject);
            CustomThrowable ballToSpawn = Balls.Find(b => b.type == observer.currentBallVariant);
            activeBall = Instantiate(ballToSpawn);
            //activeBall.transform.position = BallStartPosition;
            activeBall.transform.position = new Vector3(-0.28f, 0.95f, -0.12f); // for debugging
            // observer.throwNumber++;
        }


    }
}
