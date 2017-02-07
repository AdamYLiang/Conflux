using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlockRotation : MonoBehaviour {

    public float fixRotation = 0;

    public void Apply()
    {
        transform.parent.GetComponent<FixRotation>().rotationFix = fixRotation;
    }
}
