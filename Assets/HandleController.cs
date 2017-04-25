using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleController : MonoBehaviour {

    public GameObject originPoint;

    public bool follow = true;

	// Use this for initialization
	void Update () {

        if (Input.GetKey(KeyCode.A))
        {
            follow = !follow;
        }

        if (follow)
        {
            Reset();
        }
	}
	
	public void Reset()
    {
        transform.position = originPoint.transform.position;
    }
}
