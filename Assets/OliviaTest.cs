using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliviaTest : MonoBehaviour {

    public Camera playerCam;
    public float zoomSpeed = 5.0f;
    public float max = 20.0f;
    public float min = 10.0f;
    public float zoomSensitivity = 10.0f;

    //The targetZoom is where we want the zoom to go to.
    private float targetZoom;

    //The current zoom is what our zoom is currently. We'll use this to set the field of view.
    private float currentZoom;

    //Original zoom is where we were when Lerp begins.
    private float originalZoom;

    //The step we are on in the lerp function.
    private float step = 0f;

    private bool isAxisInUse;

    //This bool will check if we are zooming in, or out.
    private bool zoomIn;

	// Use this for initialization
	void Start () {
        currentZoom = playerCam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
        //When we get the trigger down
		if(Input.GetKeyDown(KeyCode.A))
        {
            if(isAxisInUse == false)
            {
                //If we are zooming in
                if (zoomIn)
                {   
                    //Set the target zoom to min.
                    targetZoom = min;

                    //This formula takes the zoom distance we normally travel Abs(max - min).
                    //Then it subtracts the distance we are travelling this time Abs(min - currentZoom)
                    //Then divides it by the normal distance travelled, to give us the correct step in the lerp function.
                    //The other one is the same but for the second step compares it to max.
                    step = (Mathf.Abs(max - min) - Mathf.Abs(min - currentZoom)) / Mathf.Abs(max - min);
                   
                }//If we are zooming out
                else
                {   //Set the target zoom to max.
                    targetZoom = max;
                    step = (Mathf.Abs(max - min) - Mathf.Abs(max - currentZoom)) / Mathf.Abs(max - min);
                }
                isAxisInUse = true;

                //Autocorrect step if it is 1 to 0
                if (step == 1)
                {
                    step = 0;
                }

                //Record what zoom we started out on, so we can Lerp properly.
                originalZoom = currentZoom;

                //This just switches which direction we're going in.
                zoomIn = !zoomIn;
            }
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            isAxisInUse = false;
        }

      
        //If we aren't yet at our target zoom.
        if(currentZoom != targetZoom)
        {
            //Increment the step by our zoom speed.
            step += (1 / zoomSpeed) * Time.deltaTime;
            //Lerp the current zoom using the function.
            currentZoom = Mathf.Lerp(originalZoom, targetZoom, step);
        }

        //We can just clamp the values and assign it to playerCam.fieldOfView right after. 
        currentZoom = Mathf.Clamp(currentZoom, min, max);
        playerCam.fieldOfView = currentZoom;

	}

}
