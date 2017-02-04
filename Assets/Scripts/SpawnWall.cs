using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall : MonoBehaviour {

    public bool north = false, east = false, south = false, west = false;
    bool northSpawned, eastSpawned, southSpawned, westSpawned;

    public GameObject wall;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

        if (north && !northSpawned)
        {
            Instantiate(wall, transform.position, Quaternion.identity);
            northSpawned = true;
        }




        //South is the default.
        if (!south)
        {
            
        }



	}
}
