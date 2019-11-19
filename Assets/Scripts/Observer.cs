using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using Valve.VR;

[Serializable]
public class ThrowableData {
    public ThrowableVariants type;
    public int throwNumber;
    // todo add angle
    public int Points;
    public float DistanceToTarget;
    public Vector3 ReleasePos;
    public Vector3 ReleaseForce;
    public float ReleaseAngle;

}
public enum ThrowableVariants
{
    Baseball,
    DiscPointFive,
    DiscOne,
    DiscTwo,
    DiscFive
}

public enum SceneType
{
    VR_Scene,
    MR_Scene
}

// used to handle saving any input not from controllers, saving files, etc..
public class Observer : MonoBehaviour
{
    private List<ThrowableData> ballDataStorage;
    public ThrowableData currentThrowData;
    public ThrowableVariants currentThrowable;
    private ObjectSpawner spawner;
    public SceneType SceneType;

    private int enumCount = 0;
    private int currentThrowNumber = 0;
    [NonSerialized]
    public CustomBlackboard blackboard;

    // Start is called before the first frame update
    void Start()
    {
        this.blackboard = FindObjectOfType<CustomBlackboard>();
        this.spawner = FindObjectOfType<ObjectSpawner>();
        blackboard.PlayerInfo.text += MainMenu.participantID.ToString();
        ballDataStorage = new List<ThrowableData>();
        enumCount = Enum.GetNames(typeof(ThrowableVariants)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            writeDataToFile();
            MainMenu.participantID = -1;
            currentThrowNumber = 0;
            SceneManager.LoadScene("menu");
        }
        if(currentThrowNumber > 4)
        {
            currentThrowNumber = 0;

            int.TryParse(blackboard.ThrowPoints[0][5].text, out int sumOne);
            int.TryParse(blackboard.ThrowPoints[1][5].text, out int sumTwo);
            int.TryParse(blackboard.ThrowPoints[2][5].text, out int sumThree);
            int.TryParse(blackboard.ThrowPoints[3][5].text, out int sumFour);
            int.TryParse(blackboard.ThrowPoints[4][5].text, out int sumFive);
            blackboard.TotalPoints.text = "Total Points: " + (sumOne + sumTwo + sumThree + sumFour + sumFive).ToString();
            if ((int)currentThrowable < enumCount) currentThrowable++; else currentThrowable = 0;
            Debug.Log("active variant " + currentThrowable);
        }
    }

    // handle throwable when target is missed
    public void HandleThrowableHit(CustomThrowable ball)
    {
        blackboard.ThrowPoints[(int)currentThrowable][currentThrowNumber].text = "0";
        int currentTotalVariant = 0;
        if (!int.TryParse(blackboard.ThrowPoints[(int)currentThrowable][5].text, out currentTotalVariant))
            blackboard.ThrowPoints[(int)currentThrowable][5].text = "0";
        SaveThrowDataToStorage(ball, 0);
    }
    // handle throwable when target is hit
    public void HandleThrowableHit(CustomThrowable ball, int points)
    {
        blackboard.ThrowPoints[(int)currentThrowable][currentThrowNumber].text = points.ToString();
        int currentTotalVariant = currentThrowNumber == 0 ? 0 : int.Parse(blackboard.ThrowPoints[(int)currentThrowable][5].text);
        blackboard.ThrowPoints[(int)currentThrowable][5].text = (currentTotalVariant + points).ToString();
        SaveThrowDataToStorage(ball, points);
    }

    void SaveThrowDataToStorage(CustomThrowable ball, int points) {
        int dataIndex = ballDataStorage.FindIndex(bd => bd.type == currentThrowable && bd.throwNumber == currentThrowNumber);
        ballDataStorage[dataIndex].Points = points;
        ballDataStorage[dataIndex].DistanceToTarget = Vector3.Distance(ball.transform.position, spawner.activeTarget.transform.position);
        currentThrowNumber++;
    }

    public void AddCurrentThrowData(Vector3 releasePos, float yAngle, Vector3 appliedForce) {
        if ((int)currentThrowable > 4)
            return;
        currentThrowData = new ThrowableData();
        currentThrowData.ReleasePos = releasePos;
        currentThrowData.ReleaseAngle = yAngle;
        currentThrowData.ReleaseForce = appliedForce;
        currentThrowData.throwNumber = currentThrowNumber;
        currentThrowData.type = currentThrowable;
        // There could be several throws due to failed attempts, therefore invalid data should be removed
        ballDataStorage.Remove(ballDataStorage.Find(bd => bd.type == currentThrowable && bd.throwNumber == currentThrowNumber));
        ballDataStorage.Add(currentThrowData);
    }

    void writeDataToFile() {
        //string pathBase = "Assets/Resources/ParticipantsData/";
        string pathBase = "ParticipantsData/";
        string path = pathBase + MainMenu.participantID.ToString() + ".json";

        if (!Directory.Exists(pathBase))
        {
            Directory.CreateDirectory(pathBase);
        }

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("{\"ParticipantID\": " + MainMenu.participantID.ToString() + ",");
        writer.WriteLine("\"Scene\": \"" + SceneManager.GetActiveScene().name + "\",");
        writer.WriteLine("\"ThrowDataEntries\": [");
        int index = 0;
        string commaString = ",";
        foreach (ThrowableData dataEntry in ballDataStorage)
        {
            if (index == ballDataStorage.Count - 1) commaString = "";
            writer.WriteLine(JsonUtility.ToJson(dataEntry) + commaString);
            index++;
        }
        writer.WriteLine("]\n}");
        writer.Close();
    }
    
}