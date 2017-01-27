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

        //Dont need any of this go here instead: http://unity3d.college/2016/11/16/steamvr-controller-input/
        //Sets up most of the triggers and then you just hook it up based on your own class 
        //Hair trigger is light touch on trigger
        if (device.GetHairTrigger())
        {
            Debug.Log("firing from the input manager ");
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
