using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomBlackboard : MonoBehaviour
{
    // Just a data class basically.
    private Text[] BaseballPoints;
    private Text[] DiscPointFivePoints;
    private Text[] DiscOnePoints;
    private Text[] DiscTwoPoints;
    private Text[] DiscFivePoints;
    public GameObject[] PointsRows;
    public List<Text[]> ThrowPoints;
    public Text TotalPoints;
    public Text PlayerInfo;
    public Text LastThrowAngle;
    public Text LastThrowForce;
    public Text dummy;

    private void Start()
    {
        BaseballPoints = new Text[7];
        DiscPointFivePoints = new Text[7];
        DiscOnePoints = new Text[7];
        DiscTwoPoints = new Text[7];
        DiscFivePoints = new Text[7];
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
        // saveguard??
        BaseballPoints[6] = dummy;
        DiscPointFivePoints[6] = dummy;
        DiscOnePoints[6] = dummy;
        DiscTwoPoints[6] = dummy;
        DiscFivePoints[6] = dummy;
        ThrowPoints.Add(BaseballPoints);
        ThrowPoints.Add(DiscPointFivePoints);
        ThrowPoints.Add(DiscOnePoints);
        ThrowPoints.Add(DiscTwoPoints);
        ThrowPoints.Add(DiscFivePoints);
        // DUMMY SAVEGUARD???
        ThrowPoints.Add(DiscFivePoints);
    }

}
