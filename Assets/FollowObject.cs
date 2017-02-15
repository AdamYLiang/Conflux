using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    public GameObject targetObj;
    public bool follow = false;

    Vector3 positionDifference;

	// Use this for initialization
	void Start () {

        positionDifference = transform.position - targetObj.transform.position;

	}
	
	// Update is called once per frame
	void Update () {

        if (follow)
        {
            transform.position = targetObj.transform.position + positionDifference;
        }

	}
}
