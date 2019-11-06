using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlutoScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Pluto");
        Physics.gravity = new Vector3(0.0f, -0.62f, 0.0f);
    }

}
