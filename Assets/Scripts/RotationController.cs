using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller;
    public bool isTouchingPuzzle;
    public GameObject currentPuzzle;

    // Use this for initialization
    void Start () {
        isTouchingPuzzle = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {

        controller = SteamVR_Controller.Input((int)trackedObj.index);

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (isTouchingPuzzle)
            {
                currentPuzzle.GetComponent<PuzzleManager>().play = true;
                currentPuzzle.transform.FindChild("CubePuzzle").gameObject.SetActive(false);
            }
            //Debug.Log("Trigger Pressed");
            controller.TriggerHapticPulse(700);
        }

        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (isTouchingPuzzle)
            {
                currentPuzzle.GetComponent<PuzzleManager>().play = false;
                currentPuzzle.transform.FindChild("CubePuzzle").gameObject.SetActive(true);
                isTouchingPuzzle = false;
                currentPuzzle = null;
            }
        }

    }

}
