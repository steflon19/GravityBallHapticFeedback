using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarsScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Mars");
        Physics.gravity = new Vector3(0.0f, -3.711f, 0.0f);
    }

}
