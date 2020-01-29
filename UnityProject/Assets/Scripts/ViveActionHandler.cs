using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class ViveActionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean grabAction; 
    public SteamVR_Action_Boolean gripAction;
    public bool GetTeleportDown()
    {
        //if (handType == SteamVR_Input_Sources.RightHand) return false;
        return teleportAction.GetStateDown(handType);
    }

    public bool GetGrab()
    {
        return grabAction.GetState(handType);
    }
    public bool GetGrabDown()
    {
        return grabAction.GetStateDown(handType);
    }

    public bool GetGripDown()
    {
        // To prevent the user from accidentaly spawning stuff.
        //if (handType == SteamVR_Input_Sources.RightHand) return false;
        return gripAction.GetStateDown(handType);
    }

}
