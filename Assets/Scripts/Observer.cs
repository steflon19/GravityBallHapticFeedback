using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using Valve.VR;

public struct BallThrowData {
    BallVariants type;
    // todo add angle
    Vector3 appliedForce;
    float DistanceToTarget;
    Vector3 playerPos;

}

// used to handle saving any input not from controllers, saving files, etc..
public class Observer : MonoBehaviour
{
    private List<BallThrowData> dataStorage;
    public GameObject SteamVRObject;
    public BallVariants activeBallVariant;

    private int throwNumber = 0;
    [NonSerialized]
    public CustomBlackboard blackboard;

    // Start is called before the first frame update
    void Start()
    {
        SteamVRObject.SetActive(true);
        this.blackboard = FindObjectOfType<CustomBlackboard>();
        blackboard.PlayerInfo.text += MainMenu.participantID.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // TODO: write to file?


            MainMenu.participantID = -1;
            XRSettings.enabled = false;
            SteamVRObject.SetActive(false);
            throwNumber = 0;
            SceneManager.LoadScene("menu");
        }
        if(throwNumber > 4)
        {
            throwNumber = 0;
            if ((int)activeBallVariant < 2) activeBallVariant++; else activeBallVariant = 0;
            Debug.Log("active variant " + activeBallVariant);
        }
    }

    public void HandleThrowable(CustomTarget target, CustomThrowable ball, Vector3 playerPos) {
        throwNumber++;

    }
    // TODO: Find necessary parameters 
    void SaveThrowDataToStorage(CustomTarget target, CustomThrowable ball) {


    }

    // TODO: actually write data in here....
    void writeDataToFile() {
        string path = "Assets/Resources/ParticipantsData/" + MainMenu.participantID.ToString() + ".csv";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("ParticipantID," + MainMenu.participantID.ToString());

        writer.WriteLine("some data," + (MainMenu.participantID / 2).ToString());
        writer.WriteLine("some more data," + (MainMenu.participantID * 2).ToString());
        writer.Close();
    }
}

public enum BallVariants {
    Baseball,
    Golfball,
    Kettleball
}