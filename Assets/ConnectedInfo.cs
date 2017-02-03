using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedInfo : MonoBehaviour {

    public bool received = false, complete = false;
    public bool specificColor = false;
    public EmitterScript.LaserColor laserFilter = EmitterScript.LaserColor.Blue;
    public EmitterScript.LaserColor receivedLaserColor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //If we've received a laser
        if (received)
        {
            //If we only receive a certain color
            if (specificColor)
            {
                //If the received color is the same as the filter
                if (receivedLaserColor == laserFilter)
                {
                    complete = true;
                }
            }
            else
            {
                complete = true;
            }
        }
        else
        {
            complete = false;
        }
    }
}
