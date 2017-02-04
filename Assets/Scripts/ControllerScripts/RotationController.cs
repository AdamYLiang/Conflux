using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller;
    public bool isTouchingPuzzle; //Checks to see you are touching a puzzle based on collider
    public GameObject currentPuzzle; // Registers current touched puzzle

    // Use this for initialization
    void Start () {
        isTouchingPuzzle = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {

        controller = SteamVR_Controller.Input((int)trackedObj.index);

        //If you press the button
        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //And you are touching a button, set it to play and deactivate the box collider
            if (isTouchingPuzzle)
            {
                currentPuzzle.GetComponent<PuzzleManager>().play = true;
                currentPuzzle.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            //Debug.Log("Trigger Pressed");
            controller.TriggerHapticPulse(700);
        }

        //If button is released
        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //And you are touching a puzzle, set play to false and turn on the collider, reset values
            if (isTouchingPuzzle)
            {
                currentPuzzle.GetComponent<PuzzleManager>().play = false;
                currentPuzzle.transform.gameObject.GetComponent<BoxCollider>().enabled = true;
                isTouchingPuzzle = false;
                currentPuzzle = null;
            }
        }

    }

}
