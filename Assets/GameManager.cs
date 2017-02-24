using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Keep track of the controllers
    public GameObject controller1, controller2;
    public GameObject hmd;

    public SteamVR_Controller.Device rightController, leftController;
    public SteamVR_TrackedObject trackedObjRight, trackedObjLeft;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(controller1 == null)
        {
            controller1 = GameObject.Find("Controller (left)");
            if(controller1 != null)
            {
                trackedObjLeft = controller1.transform.parent.GetComponent<SteamVR_TrackedObject>();
            }
        }
        else
        {
            leftController = SteamVR_Controller.Input((int)trackedObjLeft.index);
        }

        if(controller2 == null)
        {
            controller2 = GameObject.Find("Controller (right)");
            if (controller1 != null)
            {
                trackedObjRight = controller1.transform.parent.GetComponent<SteamVR_TrackedObject>();
            }
        }
        else
        {
            rightController = SteamVR_Controller.Input((int)trackedObjRight.index);
        }

        if(hmd == null)
        {
            hmd = GameObject.Find("Camera (eye)");
        }
	}
}
