using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValueScript : MonoBehaviour
{
    Text ballMassText;
    // Start is called before the first frame update
    void Start()
    {
        ballMassText = GetComponent<Text>();
    }

    public void textUpdate (float value)
    {
        ballMassText.text = Mathf.RoundToInt(value) + " kg";
    }
}
