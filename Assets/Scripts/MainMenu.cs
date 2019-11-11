using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button btnVR;
    public Button btnMR;
    public InputField inputNumber;
    private int participantNumber;
    public static int participantID = -1;

    public void onClickVR()
    {
        if (int.TryParse(inputNumber.text, out participantNumber))
        {
            MainMenu.participantID = participantNumber;
            SceneManager.LoadScene("vr_scene");
        }
    }
    public void onClickMR()
    {
        if (int.TryParse(inputNumber.text, out participantNumber))
        {
            MainMenu.participantID = participantNumber;
            SceneManager.LoadScene("mr_scene");
        }
    }
}
