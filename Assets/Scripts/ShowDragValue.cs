using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDragValue : MonoBehaviour
{
    Text ballDragText;
    // Start is called before the first frame update
    void Start()
    {
        ballDragText = GetComponent<Text>();
    }

    public void textUpdate(float value)
    {
        ballDragText.text = Mathf.RoundToInt(value) + " N";
    }
}
