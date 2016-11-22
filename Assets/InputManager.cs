using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Start () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();

       
     

       
    }
	
	// Update is called once per frame
	void Update () {
        //Set the index
        var device = SteamVR_Controller.Input((int)trackedObj.index);


        //Hair trigger is light touch on trigger
        if (device.GetHairTrigger())
        {
            Debug.Log("firing");
        }
        //Axis0 is the circlepad

        //Axis1 is the trigger
        //Axis2 is ???!??!?!
        //Axis3 is ?!?!?!?/
        //Axis4 is ?!?!?!?/
        //Axis5 is ?!?!?!??!

        //Check for touchpad press: device.GetPress(SteamVR_Controller.ButtonMask.Touchpad
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("Whatever this is");
        }
        
        //How to read the touchpad axis
    }
}
