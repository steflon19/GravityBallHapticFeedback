using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlacer : MonoBehaviour
{
    public Vector2 extends;
    // init pos 13, 10.2, 239
    public GameObject targetBase;
    private GameObject targetActive;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(targetActive) Object.Destroy(targetActive);
            float x = targetBase.transform.localPosition.x;
            float z = targetBase.transform.localPosition.z;
            float y = targetBase.transform.localPosition.y;
            float scale = Random.Range(0.5f, 1.3f);
            Vector3 newPos = new Vector3(Random.Range(x, x + extends.x), y, Random.Range(z, z + extends.y));
            targetActive = GameObject.Instantiate(targetBase);
            targetActive.transform.parent = this.transform;
            targetActive.transform.localPosition = newPos;
            targetActive.transform.localScale = new Vector3(scale, scale, 1);
            targetActive.SetActive(true);
        }
    }
}
