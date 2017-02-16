using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller;
    public bool isTouchingPuzzle; //Checks to see you are touching a puzzle based on collider
    public GameObject currentPuzzle; // Registers current touched puzzle
    public bool shouldRotate;
    private Vector3 rotationStartPosition;
    private Vector3 updatedPosition;
    public Vector3 facePosition;
    float rotX;
    float rotY;
    float rotZ;

    // Use this for initialization
    void Start () {
        isTouchingPuzzle = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        shouldRotate = false;
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
                rotationStartPosition = transform.position;
                shouldRotate = true;
            }
            //Debug.Log("Trigger Pressed");
            controller.TriggerHapticPulse(700);
        }

        //If button is released
        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //Set play to false and turn on the collider, reset values
            if (currentPuzzle != null)
            {
                currentPuzzle.transform.gameObject.GetComponent<BoxCollider>().enabled = true;
                isTouchingPuzzle = false;
                shouldRotate = false;
                Debug.Log("Stopped");
                currentPuzzle.GetComponent<PuzzleManager>().play = false;
                currentPuzzle = null;
            }
        }

        if (shouldRotate)
        {
            updatedPosition = transform.position;
            Vector3 directionToUpdate = (updatedPosition - rotationStartPosition);

            //Call on method to account for the correct face to rotate in the proper direction
            doRotation(directionToUpdate);

            //Debug.Log("Direction is: " + directionToUpdate + " the angles are " + currentPuzzle.transform.eulerAngles);

            //currentPuzzle.transform.Rotate(directionToUpdate, Space.World);
        }

    }


    //Helper function to convert values and then rotate cube
    void doRotation(Vector3 directionValue)
    {
        //Clamps the value to 1f if it exceeds it, in order to simulate Input.GetAxis, needs to account for -1 as well ??
        //if(directionValue.x > 1f)
        //{
        //    rotX = 1f * 3f * Mathf.Deg2Rad;
        //}
        //else rotX = directionValue.x * 3f * Mathf.Deg2Rad;

        //if (directionValue.y > 1f)
        //{
        //    rotY = 1f * 3f * Mathf.Deg2Rad;
        //}
        //else rotY = directionValue.y * 3f * Mathf.Deg2Rad;

        //if (directionValue.z > 1f)
        //{
        //    rotZ = 1f * 3f * Mathf.Deg2Rad;
        //}
        //else rotZ = directionValue.z * 3f * Mathf.Deg2Rad;
        Debug.Log(rotX);
        rotX = directionValue.z * 30f * Mathf.Deg2Rad;
        rotY = directionValue.y * 30f * Mathf.Deg2Rad;
        rotZ = directionValue.x * 30f * Mathf.Deg2Rad;

        //currentPuzzle.transform.RotateAround(currentPuzzle.transform.position, Vector3.right, -rotY * Time.deltaTime);

        //currentPuzzle.transform.RotateAround()

        //Takes the cube and rotates it along the WORLDSPACE axis based on the degree calculated above
        currentPuzzle.transform.RotateAround(Vector3.right, rotX);
        //currentPuzzle.transform.RotateAround(Vector3.forward, rotY);
        currentPuzzle.transform.RotateAround(Vector3.forward, -rotZ);

    }

}
