using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour {

    public float rotationFix = 0;
    public enum Face {North, East, South, West, Bottom, Top};
    public Vector3 originalRotation;
    public Face face;

    private float timer = 0f, waitTime = 1f;
    private bool changed = false;


    void Start()
    {
        //originalRotation = transform.eulerAngles;
    }

	
	// Update is called once per frame
	void Update () {

            if (transform.root.GetComponent<PuzzleManager>().editor)
            {
                UseRotation(originalRotation);
            }
    }

    public void UseRotation(Vector3 originalRotation)
    {
        Vector3 eulerRotation = originalRotation;
        eulerRotation += ConvertRotation(rotationFix);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }

    //Takes the rotation value and converts it to the right axis.
    Vector3 ConvertRotation(float rotation)
    {
        //Opposite faces use the same axis, but are flipped.
        if(face == Face.North || face == Face.East || face == Face.South || face == Face.West)
        {
            return new Vector3(-rotation, 0, 0);
        }
        else if (face == Face.Top)
        {
            return new Vector3(0, rotation, 0);
        }
        else if (face == Face.Bottom)
        {
            return new Vector3(0, -rotation, 0);
        }
        return Vector3.zero;
    }
}
