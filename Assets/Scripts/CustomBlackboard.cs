using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomBlackboard : MonoBehaviour
{
    // Just a data class basically.
    public Text[] BaseballPoints;
    public Text[] GolfballPoints;
    public Text[] KettlebellPoints;
    public List<Text[]> Ballpoints;
    public Text TotalPoints;
    public Text PlayerInfo;

    private void Start()
    {
        Ballpoints = new List<Text[]>();
        Ballpoints.Add(BaseballPoints);
        Ballpoints.Add(GolfballPoints);
        Ballpoints.Add(KettlebellPoints);
    }

}
