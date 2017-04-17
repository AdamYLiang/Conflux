using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaster : MonoBehaviour {

    public List<GameObject> door;

    protected int roomIndex = 0;

    protected bool allClosed = false;

    public bool locked; //Locked by default, if it is not locked then the doors will behave normally and turn
    private bool lockedChanged= false;
	public bool finishedRotating = false;
    public GameObject puzzleUnlocker; //The puzzle that unlocks said door
	public bool isPlayerIn = false; //Shows if player is inside

    //Assumes 1 door can only go between 2 rooms, assign both doors and then set them inactive when rotating
    //Assign each manually in inspector 
    public GameObject Room1; //starting room
    public GameObject Room2; //ending room
    protected bool usedRoom = false; //When set to true, will flip which rooms to set active and inactive

    public bool[] lights;

	void Start(){

		//Only does 1st door in list
		if(!locked){
			door[0].GetComponent<AirlockAnimationController>().OpenDoor();
		}

	}

    void Update()
    {

        if (locked && lockedChanged)
        {
            door[0].GetComponent<AirlockAnimationController>().CloseDoorIgnoreEvent();
            lockedChanged = false;
        }
        else if (!locked && lockedChanged)
        {
            door[0].GetComponent<AirlockAnimationController>().OpenDoorIgnoreEvent();
            lockedChanged = false;
        }

		//If both adjacent rooms are inactive, then set this door inactive, also resets the forloop so that it can turn on doors
		//Only activates if the player is not inside
		if(!Room1.activeSelf && !Room2.activeSelf && !isPlayerIn){
			Room1.GetComponent<RoomInfo>().doorsOpen = false;
			Room2.GetComponent<RoomInfo>().doorsOpen = false;
			RoomTurnOff();
			//Invoke("RoomTurnOff", 15f);
			//this.gameObject.SetActive(false);
		}

        /*
        //If the puzzle to unlock the door exists
        if (puzzleUnlocker != null)
        {
            //If the puzzle is finished then unlock the door 
            if (puzzleUnlocker.GetComponent<PuzzleManager>().finished)
            {
                locked = false;
            }
        }*/
        
    }

	void RoomTurnOff(){
		if(!Room1.activeSelf && !Room2.activeSelf){
			this.gameObject.SetActive(false);
		}
	}

    public void SetRoomsInactive()
    {
        Room1.SetActive(false);
        Room2.SetActive(false);
        if(roomIndex == 0)
        {
            roomIndex = 1;
        }
        else
        {
            roomIndex = 0;
        }
    }

    //Sets the right room active
    public void SetRoomActive()
    {
        if(roomIndex == 0)
        {
            Room1.SetActive(true);
        }
        else
        {
            Room2.SetActive(true);
        }
    }

    public void SetLock(bool state)
    {
        locked = state;
        lockedChanged = true;
    }

    public void openNextDoor()
    {

        if (usedRoom)
        {
            Room1.SetActive(true);
			Room1.GetComponent<RoomInfo>().doorsOpen = false;
            usedRoom = false;
        }

        else
        {
            Room2.SetActive(true);
			Room2.GetComponent<RoomInfo>().doorsOpen = false;
            usedRoom = true;
        }
        allClosed = false;
    }
}
