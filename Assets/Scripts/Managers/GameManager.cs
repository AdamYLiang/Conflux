using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Keep track of the controllers
    public GameObject controller1, controller2;
    public GameObject hmd;

    public SteamVR_Controller.Device rightController, leftController;
    public SteamVR_TrackedObject trackedObjRight = null, trackedObjLeft = null;

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
            controller1 = GameObject.Find("Controller (left)");
        }
        else
        {
            if(trackedObjLeft == null)
            {
                trackedObjLeft = controller1.transform.GetComponent<SteamVR_TrackedObject>();
            }
            else
            {
                leftController = SteamVR_Controller.Input((int)trackedObjLeft.index);
            }
        }

        if(controller2 == null)
        {
            controller2 = GameObject.Find("Controller (right)");
        }
        else
        {
            if (trackedObjRight == null)
            {
                trackedObjRight = controller2.transform.GetComponent<SteamVR_TrackedObject>();
            }
            else
            {
                rightController = SteamVR_Controller.Input((int)trackedObjRight.index);
            }
        }

        if(hmd == null)
        {
            hmd = GameObject.Find("Camera (eye)");
        }
	}

    //helper for checking nulls on controller
}
