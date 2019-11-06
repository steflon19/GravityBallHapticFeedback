using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

// used to handle saving any input not from controllers, saving files, etc..
public class Observer : MonoBehaviour
{
    private List<List<int>> dataStorage;
    public static int throwNumber = 0;
    public static BallVariants activeBallVariant = BallVariants.Baseball;
    public GameObject SteamVRObject;
    public Text playerIdDisplay;
    // Start is called before the first frame update
    void Start()
    {
        SteamVRObject.SetActive(true);
        InitializeDataStorage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // TODO: write to file?



            MainMenuPassID.participantID = -1;
            XRSettings.enabled = false;
            SteamVRObject.SetActive(false);
            Observer.throwNumber = 0;
            SceneManager.LoadScene("menu");
        }

        if (Input.GetKeyDown(KeyCode.B)) {

        }
    }

    void InitializeDataStorage() {

    }

    void SaveThrowDataToStorage(CustomTarget target, CustomThrowable ball) {
        int currentThrow = Observer.throwNumber;

    }

    void writeDataToFile() {
        string path = "Assets/Resources/ParticipantsData/" + MainMenuPassID.participantID.ToString() + ".csv";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("ParticipantID," + MainMenuPassID.participantID.ToString());

        writer.WriteLine("some data," + (MainMenuPassID.participantID / 2).ToString());
        writer.WriteLine("some more data," + (MainMenuPassID.participantID * 2).ToString());
        writer.Close();
    }
}

public enum BallVariants {
    Baseball,
    Kettleball,
    Golfball
}