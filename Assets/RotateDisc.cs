using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDisc : MonoBehaviour {

    //Handle to follow as it rotates;
    public GameObject handle;

    //Object to map rotations to.
    public GameObject mappedObject;
    
    //Offset of rotation.
    Vector3 rotationOffset;

    //The valid rotations we want to allow.
    public Vector3[] validRotations;

    //The target rotation.
    Vector3 targetRotation;

    //Snappiness of follow. 
    public float snap = 0.5f;

    //Smoothdamp velocity
    float velocity = 0.0f;

	// Use this for initialization
	void Start () {
        rotationOffset = mappedObject.transform.eulerAngles;
        Debug.Log(rotationOffset);
        AutoCorrect();
	}
	
	// Update is called once per frame
	void Update () {

        //Vector3 directionToHandle = handle.transform.position - transform.position;

        //Calculate the angle to the handle
        //float angleToHandle = Vector3.Angle( , directionToHandle);


        //GetComponent<Rigidbody>().MoveRotation()

        AutoCorrect();

        //Calculate the smoothdamp y angle
        float yAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetRotation.y,
                ref velocity,
                snap
            );

        //Debug.Log(yAngle);

        //Snap the mapped object to the rotation.
        transform.eulerAngles = new Vector3(
            0,
            yAngle,
            0);

        mappedObject.transform.eulerAngles = transform.eulerAngles + rotationOffset;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AutoCorrect();
        }

	}

    //Auto corrects the rotation to 90 degree snaps.
    public void AutoCorrect()
    {
        Vector3 rotation = transform.eulerAngles;
        Vector3 correction = new Vector3(0, 0, 0);
        float distance = float.MaxValue;

        for(int i = 0; i < validRotations.Length; i++)
        {
            if(Vector3.Distance(rotation, validRotations[i]) < distance)
            {
                distance = Vector3.Distance(rotation, validRotations[i]);
                if(distance - 360 > -45f)
                {
                    distance = Mathf.Abs(distance - 360f);
                }
                //Debug.Log("Closest: " + validRotations[i] + " Distance: " + distance);
               
                correction = validRotations[i];
            }
        }

        targetRotation = correction;
    }
}
