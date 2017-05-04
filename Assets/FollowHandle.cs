using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowHandle : MonoBehaviour {

    //The handle that will influence us.
    public GameObject handle;

    public enum Axis_t
    {
        XAxis,
        YAxis,
        ZAxis
    };

    //Axis to rotate around.
    public Axis_t axisOfRotation = Axis_t.YAxis;



    // Update is called once per frame
    void Update () {
        
        
	}
}
