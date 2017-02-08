using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedInfo : MonoBehaviour {

    public bool received = false, complete = false;
    public bool specificColor = true;
    public Color receivedRGBColor, incompleteRGBColor;
    public EmitterScript.LaserColor laserFilter = EmitterScript.LaserColor.Blue;
    public EmitterScript.LaserColor receivedLaserColor = EmitterScript.LaserColor.None;

    private PuzzleManager manager;
    // Use this for initialization
    void Start () {
        manager = transform.root.GetComponent<PuzzleManager>();
        if(laserFilter != EmitterScript.LaserColor.None)
        {
            incompleteRGBColor = manager.GetLaserPigment(laserFilter);
            incompleteRGBColor *= 1 / 3f;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (manager.editor)
        {
            incompleteRGBColor = manager.GetLaserPigment(laserFilter);
            incompleteRGBColor *= 1 / 3f;
        }

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
