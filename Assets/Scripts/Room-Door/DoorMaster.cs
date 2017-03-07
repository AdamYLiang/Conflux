using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaster : MonoBehaviour {

    public List<GameObject> door;

    protected int roomIndex = 0;

    protected bool allClosed = false;

    public bool locked; //Locked by default, if it is not locked then the doors will behave normally and turn
    public GameObject puzzleUnlocker; //The puzzle that unlocks said door

    //Assumes 1 door can only go between 2 rooms, assign both doors and then set them inactive when rotating
    //Assign each manually in inspector 
    public GameObject Room1; //starting room
    public GameObject Room2; //ending room
    protected bool usedRoom = false; //When set to true, will flip which rooms to set active and inactive

    void Update()
    {

        if (!locked)
        {
          
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

    public void openNextDoor()
    {

        if (usedRoom)
        {
            Room1.SetActive(true);
            usedRoom = false;
        }

        else
        {
            Room2.SetActive(true);
            usedRoom = true;
        }
        allClosed = false;
    }
}
