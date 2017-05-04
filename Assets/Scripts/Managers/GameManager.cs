using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour {

    //Keep track of the controllers
    public GameObject controller1, controller2;
    public GameObject hmd;

    //public SteamVR_Controller.Device rightController, leftController;
    //public SteamVR_TrackedObject trackedObjRight = null, trackedObjLeft = null;
    public Hand hand1, hand2;


    // Use this for initialization
    void Start () {
		
	}
	
	//Assigns based on it 
    //Change name of controller 1 and 2
    //Controller 2 is no sphere
    //Grows cube

	void Update () {
		if(controller1 == null)
        {
            controller1 = GameObject.Find("Hand1");
        }
        else
        {
            if(hand1 == null)
            {
                hand1 = controller1.GetComponent<Hand>();
            }
        }

        if(controller2 == null)
        {
            controller2 = GameObject.Find("Hand2");
        }
        else
        {
            if(hand2 == null)
            {
                hand2 = controller2.GetComponent<Hand>();
            }
        }

        if(hmd == null)
        {
            hmd = GameObject.Find("Camera (eye)");
        }
	}

    //helper for checking nulls on controller
}
