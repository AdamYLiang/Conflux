using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour {


    public List<GameObject> doors;
    private List<DoorInfo> doorInfo;

	// Use this for initialization
	void Start () {
		
        for(int i = 0; i < doors.Count - 1; i++)
        {
            DoorInfo temp = new DoorInfo(doors[i]);
            doorInfo.Add(temp);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public class DoorInfo
    {
        bool open = false;
        GameObject door = null;

        public DoorInfo(GameObject obj)
        {
            door = obj;
        }
    }


}
