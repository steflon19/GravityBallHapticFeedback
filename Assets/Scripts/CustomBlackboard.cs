using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomBlackboard : MonoBehaviour
{
    // Just a data class basically.
    private Text[] BaseballPoints = new Text[6];
    private Text[] DiscPointFivePoints = new Text[6];
    private Text[] DiscOnePoints = new Text[6];
    private Text[] DiscTwoPoints = new Text[6];
    private Text[] DiscFivePoints = new Text[6];
    public GameObject[] PointsRows;
    public List<Text[]> ThrowPoints;
    public Text TotalPoints;
    public Text PlayerInfo;

    private void Start()
    {
        // TODO: if further change, refactor to GetGameObjectByTag("Throw1") etc..
        ThrowPoints = new List<Text[]>();
        for (int i = 0; i < PointsRows.Length; i++)
        {
            Text[] children = PointsRows[i].transform.GetComponentsInChildren<Text>();
            BaseballPoints[i] = children[0];
            DiscPointFivePoints[i] = children[1];
            DiscOnePoints[i] = children[2];
            DiscTwoPoints[i] = children[3];
            DiscFivePoints[i] = children[4];
        }
        ThrowPoints.Add(BaseballPoints);
        ThrowPoints.Add(DiscPointFivePoints);
        ThrowPoints.Add(DiscOnePoints);
        ThrowPoints.Add(DiscTwoPoints);
        ThrowPoints.Add(DiscFivePoints);
    }

}
