using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour {

    //Steam VR stuff
    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller;

    public bool isTouchingPuzzle; //Checks to see you are touching a puzzle based on collider
    public GameObject currentPuzzle; // Registers current touched puzzle
    public GameObject lastPuzzle; //Temporarily saves the last puzzle, used for continued flick rotation
    public bool shouldRotate; //Boolean to check if player can rotate the object
    public bool continueRotate; //Boolean to check if object should continue rotation after deselected

    //Used to calculate a vector between the initial grab point and the controller 
    private Vector3 rotationStartPosition; 
    private Vector3 updatedPosition;


    public int rotationSpeed; //Speed of rotation, defaulted to 10 
    public float rotDecayTime = 3f; //Timer for rotation
    public float percentage; //Percent to decay for timer

    //Rotation values before rotate
    private float tempRotX, tempRotY, tempRotZ;
    float rotX;
    float rotZ;
    float rotY;

    //Storage of rotation values to keep after a flick rotation
    float steadyRotateX;
    float steadyRotateZ;
    float steadyRotateY;

    //Some lock code : Calvin So
    protected bool lockRotation = false;

    void Start () {
        isTouchingPuzzle = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        shouldRotate = false;
        continueRotate = false;
        rotationSpeed = 10;
        steadyRotateX = 0;
        steadyRotateY = 0;
        steadyRotateZ = 0;
    }

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
                lastPuzzle = null;
                continueRotate = false;
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
                lastPuzzle = currentPuzzle;
                percentage = 1f;
                tempRotX = rotX;
                tempRotY = rotY;
                tempRotZ = rotZ;
                currentPuzzle.transform.gameObject.GetComponent<BoxCollider>().enabled = true;
                isTouchingPuzzle = false;
                shouldRotate = false;
                Debug.Log("Stopped");
                currentPuzzle.GetComponent<PuzzleManager>().play = false;
                currentPuzzle = null;
            }
        }

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            lockRotation = true;
        }

        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            lockRotation = false;
        }

        //If the player is grabbing a puzzle they can rotate it based on controller movement
        if (shouldRotate && !lockRotation)
        {
            updatedPosition = transform.position;
            Vector3 directionToUpdate = (updatedPosition - rotationStartPosition);

            //Call on method to account for the correct face to rotate in the proper direction
            doRotation(directionToUpdate);

            //Debug.Log("Direction is: " + directionToUpdate + " the angles are " + currentPuzzle.transform.eulerAngles);

            //currentPuzzle.transform.Rotate(directionToUpdate, Space.World);
        }

        //If the player has let go of the object AND it should continue rotating after flick
        if (continueRotate && !shouldRotate)
        {
            rotX = tempRotX * percentage;
            rotY = tempRotY * percentage;
            rotZ = tempRotZ * percentage;
            percentage = Mathf.Clamp01(percentage - Time.deltaTime / rotDecayTime);
            
            ContinueRotate(lastPuzzle, rotX, rotY, rotZ);
        }

    }


    //Helper function to convert values and then rotate cube
    void doRotation(Vector3 directionValue)
    {
        //Sets a deadzone between -0.1 and 0.1
        //Sets optimal rotate between -0.6 and 0.6
        //Anything more breaks the cube and makes it spin
        /*
        if (directionValue.z <= 0.1 && directionValue.z >= -0.1)
        {
            rotationSpeed = 15;
            directionValue.z = 0;
        }

        //Some ad-hoc code by Calvin.
        //if (directionValue.y <= 0.1 && directionValue.y >= -0.1)
        //{
        //    rotationSpeed = 15;
        //    directionValue.y = 0;
        //}

        if (directionValue.x <= 0.1 && directionValue.x >= -0.1)
        {
            rotationSpeed = 15;
            directionValue.x = 0;
        }*/

        //Actual rotation, does not take into account the y axis since two axis rotation seems good
        rotZ = directionValue.z * rotationSpeed * Mathf.Deg2Rad;
        //rotY = directionValue.y * rotationSpeed * Mathf.Deg2Rad;
        rotX = directionValue.x * rotationSpeed * Mathf.Deg2Rad;

        //If the players flicks hard, save the value and continue rotating the cube once released 
        //Stop the rotate once grabbed again

        //Debug.Log(directionValue.x + " " + directionValue.z);
        //If the pull direction exceeds these bounds, keep the spin of a flick
        if(directionValue.x > 0.4f || directionValue.x < -0.4f)
        {
            steadyRotateX = rotX;
            continueRotate = true;
        }

        if(directionValue.y > 0.4f || directionValue.y < -0.4f)
        {
            steadyRotateY = rotY;
            continueRotate = true;
        }

        if(directionValue.z > 0.4f || directionValue.z < -0.4f)
        {
            steadyRotateZ = rotZ;
            continueRotate = true;
        }

        //Takes the cube and rotates it along the WORLDSPACE axis based on the degree calculated above
        currentPuzzle.transform.RotateAround(Vector3.right, rotZ);
        //currentPuzzle.transform.RotateAround(Vector3.forward, rotY);
        currentPuzzle.transform.RotateAround(Vector3.forward, -rotX);

    }

    //Helper method that keeps the puzzle rotate after flick
    void ContinueRotate(GameObject puzzle, float steadyRotX, float steadyRotY, float steadyRotZ)
    {   
        GameObject temp = puzzle;
        //Debug.Log(steadyRotZ + " HERES MORE " + -steadyRotX);
        //This is used to decay the values so the rotation stops
        temp.transform.RotateAround(Vector3.right, steadyRotZ);
        //temp.transform.RotateAround(Vector3.forward, steadyRotY);
        temp.transform.RotateAround(Vector3.forward, -steadyRotX);
    }

}
