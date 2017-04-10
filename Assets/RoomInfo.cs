using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour {


    public List<GameObject> doors;
    private List<DoorInfo> doorInfo;
	public bool doorsOpen = false;

	// Use this for initialization
	void Start () {
		
        for(int i = 0; i < doors.Count - 1; i++)
        {
            DoorInfo temp = new DoorInfo(doors[i]);
            //doorInfo.Add(temp);
        }
	}

	//BUGS: Door doesnt rotate when going in and out of the same door.

	// Update is called once per frame
	void Update () {

		//If the room is active and the doors are not open
		//Run a loop through all the doors and set them open, then say its open so it stops the loop
		if(this.gameObject.activeSelf && !doorsOpen){
			for(int i = 0; i < doors.Count; i++){
				doors[i].SetActive(true);
			}
			doorsOpen = true;
		}
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
