using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaster : MonoBehaviour {

    public List<GameObject> door;

    public int openDoor = 0;
	
    protected int doorIndex = 0;

    protected bool allClosed = false;

    public bool locked; //Locked by default, if it is not locked then the doors will behave normally and turn
    public GameObject puzzleUnlocker;

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
        
    }

    public void closeAllDoor()
    {
        allClosed = true;
    }

	public void openNextDoor()
    {
        //doorIndex++;
        //if(doorIndex == door.Count)
        //{
        //    doorIndex = 0;
        //}
        openDoor = doorIndex;
        allClosed = false;
    }
}
