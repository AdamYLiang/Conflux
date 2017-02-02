using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverScript : MonoBehaviour {

    public bool received = false, complete = false;
    public bool specificColor = false;
    public EmitterScript.LaserColor laserFilter = EmitterScript.LaserColor.Blue;
    public EmitterScript.LaserColor receivedLaserColor;

    private int indexOnList = -1;
	// Use this for initialization
	void Start () {

        indexOnList = transform.root.GetComponent<PuzzleManager>().receiverCompletion.Count;
        transform.root.GetComponent<PuzzleManager>().receiverCompletion.Add(false);

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
                if(receivedLaserColor == laserFilter)
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



        if (complete)
        {
            transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = true;
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = false;
            GetComponent<Renderer>().material.color = Color.white;
        }
       
	}
}
