using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlockRotation : MonoBehaviour {

    public float fixRotation = 0;
    public bool enabled = false;
	
	// Update is called once per frame
	void Update () {

        if(fixRotation != 0)
        {
            enabled = true;
        }

        if (enabled)
        {
            transform.parent.GetComponent<FixRotation>().rotationFix = fixRotation;
        }
       
	}
}
