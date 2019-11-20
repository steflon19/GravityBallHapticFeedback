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
    // This couldv been done cleaner but hey..quick and dirty..
    BaseballTwo
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
    [NonSerialized]
    public ObjectSpawner spawner;
    public SceneType ActiveSceneType;

    private int enumCount = 0;
    private int currentThrowNumber = 0;
    private bool handledLastThrowOfType = false;
    [NonSerialized]
    public CustomBlackboard blackboard;

    // Start is called before the first frame update
    void Start()
    {
        ResetDataStuff();
    }

    private void OnLevelWasLoaded(int level)
    {
        ResetDataStuff();
    }

    void ResetDataStuff() {
        blackboard = FindObjectOfType<CustomBlackboard>();
        spawner = FindObjectOfType<ObjectSpawner>();
        blackboard.PlayerInfo.text = "PlayerID: " + MainMenu.participantID.ToString();
        ballDataStorage = new List<ThrowableData>();
        enumCount = Enum.GetNames(typeof(ThrowableVariants)).Length;
        currentThrowNumber = 0;
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
        //if(handledLastThrowOfType)
        if(currentThrowNumber > 4)
        {
            currentThrowNumber = 0;

            int.TryParse(blackboard.ThrowPoints[0][5].text, out int sumOne);
            int.TryParse(blackboard.ThrowPoints[1][5].text, out int sumTwo);
            int.TryParse(blackboard.ThrowPoints[2][5].text, out int sumThree);
            int.TryParse(blackboard.ThrowPoints[3][5].text, out int sumFour);
            int.TryParse(blackboard.ThrowPoints[4][5].text, out int sumFive);
            blackboard.TotalPoints.text = "Total Points: " + (sumOne + sumTwo + sumThree + sumFour + sumFive).ToString();
            currentThrowable++;
        }
        if ((int)currentThrowable >= enumCount)
        {
            currentThrowable = 0;
            Debug.Log("NEXT active variant RESET " + currentThrowable);
            Debug.LogWarning("Test DONE, should stop.");
        }

    }

    // handle throwable when target is missed
    public void HandleThrowableHit(CustomThrowable ball)
    {
        int tmpThrowNum = currentThrowNumber;
        ThrowableVariants tmpThrowVar = currentThrowable;
        if (tmpThrowNum > 4)
        {
            Debug.LogError("INVALID ACCESS AddCurrentThrowData " + currentThrowNumber);
            tmpThrowNum -= 1;
        } else if ((int)tmpThrowVar > 4)
        {
            Debug.LogError("INVALID ACCESS AddCurrentThrowData " + currentThrowable);
            tmpThrowVar -= 1;
        }
        blackboard.ThrowPoints[(int)tmpThrowVar][tmpThrowNum].text = "0";
        int currentTotalVariant = 0;
        if (!int.TryParse(blackboard.ThrowPoints[(int)tmpThrowVar][5].text, out currentTotalVariant))
            blackboard.ThrowPoints[(int)tmpThrowVar][5].text = "0";
        SaveThrowDataToStorage(ball, 0);
    }
    // handle throwable when target is hit
    public void HandleThrowableHit(CustomThrowable ball, int points)
    {
        int tmpThrowNum = currentThrowNumber;
        ThrowableVariants tmpThrowVar = currentThrowable;
        if (tmpThrowNum > 4)
        {
            Debug.LogError("INVALID ACCESS HandleThrowableHit " + currentThrowNumber);
            tmpThrowNum -= 1;
        }
        else if ((int)tmpThrowVar > 4)
        {
            Debug.LogError("INVALID ACCESS HandleThrowableHit " + currentThrowable);
            tmpThrowVar -= 1;
        }
        blackboard.ThrowPoints[(int)tmpThrowVar][tmpThrowNum].text = points.ToString();
        int currentTotalVariant = tmpThrowNum == 0 ? 0 : int.Parse(blackboard.ThrowPoints[(int)tmpThrowVar][5].text);
        blackboard.ThrowPoints[(int)tmpThrowVar][5].text = (currentTotalVariant + points).ToString();
        SaveThrowDataToStorage(ball, points);
    }

    void SaveThrowDataToStorage(CustomThrowable ball, int points)
    {
        int dataIndex = ballDataStorage.FindIndex(bd => bd.type == currentThrowable && bd.throwNumber == currentThrowNumber);
        if (dataIndex < 0) {
            Debug.LogError("invalid data saving, something went wrong before?? " + currentThrowable + " - " + currentThrowNumber);
            currentThrowNumber++;
            return;
        }
        ballDataStorage[dataIndex].Points = points;
        float dist = Vector3.Distance(ball.transform.position, spawner.activeTarget.transform.position);
        ballDataStorage[dataIndex].DistanceToTarget = (float)Math.Round(dist, 4);
        writeDataToFileRAW(currentThrowData);

        Debug.Log("increasing throwable num");
        currentThrowNumber++;
        //currentThrowNumber = currentThrowNumber < 5 ? currentThrowNumber + 1 : 0;
        //if (currentThrowNumber == 5)
        //    handledLastThrowOfType = true;
    }

    public void AddCurrentThrowData(Vector3 releasePos, float yAngle, Vector3 appliedForce)
    {
        int tmpThrowNum = currentThrowNumber;
        ThrowableVariants tmpThrowVar = currentThrowable;
        if (tmpThrowNum > 4)
        {
            Debug.LogError("INVALID ACCESS HandleThrowableHit " + currentThrowNumber);
            tmpThrowNum -= 1;
        }
        else if ((int)tmpThrowVar > 4)
        {
            Debug.LogError("INVALID ACCESS HandleThrowableHit " + currentThrowable);
            tmpThrowVar -= 1;
        }
        currentThrowData = new ThrowableData();
        currentThrowData.ReleasePos = releasePos;
        currentThrowData.ReleaseAngle = yAngle;
        currentThrowData.ReleaseForce = appliedForce;
        currentThrowData.throwNumber = tmpThrowNum;
        currentThrowData.type = currentThrowable;
        // There could be several releases due to failed attempts, therefore invalid data should be removed
        // only do this in case this was an actual proper throw? to prevent deleting proper data i guess
        if(tmpThrowNum == currentThrowNumber)
            ballDataStorage.Remove(ballDataStorage.Find(bd => bd.type == currentThrowable && bd.throwNumber == tmpThrowNum));
        ballDataStorage.Add(currentThrowData);
    }

    void writeDataToFileRAW(ThrowableData data)
    {
        string pathBase = "ParticipantsData/";
        string path = pathBase + MainMenu.participantID.ToString() + "_RAW.txt";
        if (!Directory.Exists(pathBase))
        {
            Directory.CreateDirectory(pathBase);
        }
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(JsonUtility.ToJson(data));
        writer.Close();
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

    public int GetCurrentThrowNumber() {
        return currentThrowNumber;
    }

    public void WriteThrowToBlackboard(float forceMag, float yAngle)
    {
        blackboard.LastThrowAngle.text = "Last Throw Angle: " + Math.Round(yAngle).ToString() + "°";
        blackboard.LastThrowForce.text = "Last Throw Force: " + Math.Round(forceMag, 2).ToString();
    }

}