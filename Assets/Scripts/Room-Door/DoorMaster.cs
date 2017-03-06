using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaster : MonoBehaviour {

    public List<GameObject> door;

    public int openDoor = 0;
	
    protected int doorIndex = 0;

    protected bool allClosed = false;

    public bool locked; //Locked by default, if it is not locked then the doors will behave normally and turn
    public GameObject puzzleUnlocker; //The puzzle that unlocks said door

    //Assumes 1 door can only go between 2 rooms, assign both doors and then set them inactive when rotating
    //Assign each manually in inspector 
    public GameObject Room1; //starting room
    public GameObject Room2; //ending room
    protected bool usedRoom = false; //When set to true, will flip which rooms to set active and inactive

    void Start()
    {
        doorIndex = openDoor;
    }

    void Update()
    {

        if (!locked)
        {

            if (!allClosed)
            {
                for (int i = 0; i < door.Count; i++)
                {
                    if (i != doorIndex)
                    {
                        door[i].SetActive(true);
                    }
                    else
                    {
                        door[i].SetActive(false);
                    }
                }
            }
            else //All doors should be closed
            {
                for (int i = 0; i < door.Count; i++)
                {
                    door[i].SetActive(true);
                }
            }
        }

        //If the puzzle to unlock the door exists
        if (puzzleUnlocker != null)
        {
            //If the puzzle is finished then unlock the door 
            if (puzzleUnlocker.GetComponent<PuzzleManager>().finished)
            {
                locked = false;
            }
        }
        
    }

    public void closeAllDoor()
    {
        Room1.SetActive(false);
        Room2.SetActive(false);
        allClosed = true;
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
        //doorIndex++;
        //if(doorIndex == door.Count)
        //{
        //    doorIndex = 0;
        //}
        openDoor = doorIndex;
        allClosed = false;
    }
}
