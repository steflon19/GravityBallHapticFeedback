using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject baseTarget;
    public AnimationCurve targetScaleByDistanceCurve;
    [System.NonSerialized]
    public GameObject activeTarget;
    [System.NonSerialized]
    public bool throwableGrabReady;

    public List<CustomThrowable> Throwables;
    [System.NonSerialized]
    public CustomThrowable activeThrowable;

    private Vector3 BallStartPosition;

    private Observer observer;
    private ViveActionHandler viveActionHandler;
    private float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        BallStartPosition = new Vector3(1.8f, 0.9f, 1.95f);
        observer = FindObjectOfType<Observer>();
        viveActionHandler = FindObjectOfType<ViveActionHandler>();
        activeTarget = Instantiate(baseTarget);
        throwableGrabReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))// || viveActionHandler.GetTeleportDown())
        {
            SpawnTarget();
        }

        if (Input.GetKeyDown(KeyCode.B) || (observer.ActiveSceneType == SceneType.MR_Scene && viveActionHandler.GetGrabDown() && (activeThrowable && activeThrowable.collisionHandled))
            || (viveActionHandler.GetGripDown() && SceneType.VR_Scene == observer.ActiveSceneType))// || viveActionHandler.GetGripDown())
        {
            if (activeThrowable && activeThrowable.gameObject) Destroy(activeThrowable.gameObject);
            CustomThrowable ballToSpawn = Throwables.Find(b => b.type == observer.currentThrowable);
            activeThrowable = Instantiate(ballToSpawn);
            if (observer.ActiveSceneType == SceneType.VR_Scene)
            {
                // TODO: set to ballstartposition for proper start
                activeThrowable.transform.position = BallStartPosition;
                activeThrowable.gameObject.SetActive(true);
                Debug.Log("active throwable ", activeThrowable);
                //activeThrowable.transform.position = new Vector3(-0.28f, 0.95f, -0.12f); // for debugging
            }
            else
            {
                throwableGrabReady = true;
                Debug.Log("Throwable ready MR " + activeThrowable.gameObject.name);
            }

            SpawnTarget();
        }
    }

    void SpawnTarget()
    {

        if (activeTarget) Destroy(activeTarget);
        float x = baseTarget.transform.localPosition.x;
        float z = baseTarget.transform.localPosition.z;
        float y = baseTarget.transform.localPosition.y;
        Vector3 newPos = new Vector3(x + observer.GetCurrentThrowNumber(), y, Random.Range(z, z + 6));
        //float distanceToTarget = Vector3.Distance(BallStartPosition, newPos);
        //float scaleMin = targetScaleByDistanceCurve.Evaluate(distanceToTarget/maxDistance);
        //Debug.Log("dist " + distanceToTarget + " and scaleMin " + scaleMin);
        //float scale = Random.Range(scaleMin, 1.3f);
        activeTarget = Instantiate(baseTarget);
        activeTarget.transform.parent = this.transform;
        activeTarget.transform.localPosition = newPos;
        activeTarget.transform.localScale = new Vector3(1, 1, 1);
        activeTarget.SetActive(true);
    }
}
