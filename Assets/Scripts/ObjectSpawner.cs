using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Vector2 extends;
    public GameObject baseTarget;
    [System.NonSerialized]
    public GameObject activeTarget;

    public List<CustomThrowable> Throwables;
    private CustomThrowable activeThrowable;

    private Vector3 BallStartPosition;

    private Observer observer;
    private ViveActionHandler viveActionHandler;
    // Start is called before the first frame update
    void Start()
    {
        BallStartPosition = new Vector3(1.8f, 0.9f, 1.95f);
        observer = FindObjectOfType<Observer>();
        viveActionHandler = FindObjectOfType<ViveActionHandler>();
        activeTarget = Instantiate(baseTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) || viveActionHandler.GetTeleportDown())
        {
            if(activeTarget) Destroy(activeTarget);
            float x = baseTarget.transform.localPosition.x;
            float z = baseTarget.transform.localPosition.z;
            float y = baseTarget.transform.localPosition.y;
            float scale = Random.Range(0.5f, 1.3f);
            Vector3 newPos = new Vector3(Random.Range(x, x + extends.x), y, Random.Range(z, z + extends.y));
            activeTarget = Instantiate(baseTarget);
            activeTarget.transform.parent = this.transform;
            activeTarget.transform.localPosition = newPos;
            activeTarget.transform.localScale = new Vector3(scale, scale, 1);
            activeTarget.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.B) || viveActionHandler.GetGripDown())
        {
            if (activeThrowable) Destroy(activeThrowable.gameObject);
            CustomThrowable ballToSpawn = Throwables.Find(b => b.type == observer.currentThrowable);
            activeThrowable = Instantiate(ballToSpawn);
            // TODO: set to ballstartposition for proper start
            activeThrowable.transform.position = BallStartPosition;
            activeThrowable.gameObject.SetActive(true);
            Debug.Log("active throwable ", activeThrowable);
            //activeThrowable.transform.position = new Vector3(-0.28f, 0.95f, -0.12f); // for debugging
            // observer.throwNumber++;
        }
    }
}
