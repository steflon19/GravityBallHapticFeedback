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
public class BallThrowData {
    public BallVariants type;
    public int throwNumber;
    // todo add angle
    public int Points;
    public float DistanceToTarget;
    public Vector3 ReleasePos;
    public Vector3 ReleaseForce;
    public float ReleaseAngle;

}
public enum BallVariants
{
    Baseball,
    Golfball,
    Kettleball
}

// used to handle saving any input not from controllers, saving files, etc..
public class Observer : MonoBehaviour
{
    private List<BallThrowData> ballDataStorage;
    public BallThrowData currentThrowData;
    public BallVariants currentBallVariant;
    private ObjectSpawner spawner;

    private int currentThrowNumber = 0;
    [NonSerialized]
    public CustomBlackboard blackboard;

    // Start is called before the first frame update
    void Start()
    {
        this.blackboard = FindObjectOfType<CustomBlackboard>();
        this.spawner = FindObjectOfType<ObjectSpawner>();
        blackboard.PlayerInfo.text += MainMenu.participantID.ToString();
        ballDataStorage = new List<BallThrowData>();
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

            int sumOne, sumTwo, sumThree = 0;
            int.TryParse(blackboard.Ballpoints[0][5].text, out sumOne);
            int.TryParse(blackboard.Ballpoints[1][5].text, out sumTwo);
            int.TryParse(blackboard.Ballpoints[2][5].text, out sumThree);
            blackboard.TotalPoints.text = "Total Points: " + (sumOne + sumTwo + sumThree).ToString();
            if ((int)currentBallVariant < 2) currentBallVariant++; else currentBallVariant = 0;
            Debug.Log("active variant " + currentBallVariant);
        }
    }

    // handle throwable when target is missed
    public void HandleThrowableHit(CustomThrowable ball)
    {
        GameObject activeTarget = spawner.activeTarget;
        // TODO: calculate distance to save to data storage.
        blackboard.Ballpoints[(int)currentBallVariant][currentThrowNumber].text = "0";
        int currentTotalVariant = 0;
        if (!int.TryParse(blackboard.Ballpoints[(int)currentBallVariant][5].text, out currentTotalVariant))
            blackboard.Ballpoints[(int)currentBallVariant][5].text = "0";
        SaveThrowDataToStorage(ball, 0);
    }
    // handle throwable when target is hit
    public void HandleThrowableHit(CustomThrowable ball, int points)
    {
        blackboard.Ballpoints[(int)currentBallVariant][currentThrowNumber].text = points.ToString();
        int currentTotalVariant = currentThrowNumber == 0 ? 0 : int.Parse(blackboard.Ballpoints[(int)currentBallVariant][5].text);
        blackboard.Ballpoints[(int)currentBallVariant][5].text = (currentTotalVariant + points).ToString();
        SaveThrowDataToStorage(ball, points);
    }

    void SaveThrowDataToStorage(CustomThrowable ball, int points) {
        int dataIndex = ballDataStorage.FindIndex(bd => bd.type == currentBallVariant && bd.throwNumber == currentThrowNumber);
        ballDataStorage[dataIndex].Points = points;
        ballDataStorage[dataIndex].DistanceToTarget = Vector3.Distance(ball.transform.position, spawner.activeTarget.transform.position);
        currentThrowNumber++;
    }

    public void AddCurrentThrowData(Vector3 releasePos, float yAngle, Vector3 appliedForce) {
        currentThrowData = new BallThrowData();
        currentThrowData.ReleasePos = releasePos;
        currentThrowData.ReleaseAngle = yAngle;
        currentThrowData.ReleaseForce = appliedForce;
        currentThrowData.throwNumber = currentThrowNumber;
        currentThrowData.type = currentBallVariant;
        // There could be severa throws due to failed attempts, therefore invalid data should be removed
        ballDataStorage.Remove(ballDataStorage.Find(bd => bd.type == currentBallVariant && bd.throwNumber == currentThrowNumber));
        ballDataStorage.Add(currentThrowData);
    }

    void writeDataToFile() {
        string path = "Assets/Resources/ParticipantsData/" + MainMenu.participantID.ToString() + ".json";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("{\"ParticipantID\": " + MainMenu.participantID.ToString() + ",");
        writer.WriteLine("\"Scene\": \"" + SceneManager.GetActiveScene().name + "\",");
        writer.WriteLine("\"ThrowDataEntries\": [");
        int index = 0;
        string commaString = ",";
        foreach (BallThrowData dataEntry in ballDataStorage)
        {
            if (index == ballDataStorage.Count - 1) commaString = "";
            writer.WriteLine(JsonUtility.ToJson(dataEntry) + commaString);
        }
        writer.WriteLine("]\n}");
        writer.Close();
    }
    
}