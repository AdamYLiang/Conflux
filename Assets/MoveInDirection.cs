using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour {

    public Vector3 direction;
    public bool move = false;
	
	// Update is called once per frame
	void Update () {

        if (move)
        {
            transform.position += direction * Time.deltaTime;
        }
        

	}
}
