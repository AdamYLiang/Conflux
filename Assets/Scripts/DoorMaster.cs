using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaster : MonoBehaviour {

    public List<GameObject> door;

    public int openDoor = 0;
	
    protected int doorIndex = 0;

    protected bool allClosed = false;

    void Start()
    {
        doorIndex = openDoor;
    }

    void Update()
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
            for(int i = 0; i < door.Count; i++)
            {
                door[i].SetActive(true);
            }
        }
        
    }

    public void FlipDoor(float timeTaken)
    {
        StartCoroutine(FlipDoorCoroutine(timeTaken));
    }

    public IEnumerator FlipDoorCoroutine(float timeTaken)
    {
        float timeDuration = timeTaken;
        float step = 0f;
        Quaternion originalRotation = transform.rotation;
        Quaternion flip = Quaternion.Euler(0, 180, 0);
        Quaternion flippedRotation = new Quaternion(flip.x + originalRotation.x, flip.y + originalRotation.y,
            flip.z + originalRotation.z, flip.w + originalRotation.w);

        while (step < 1)
        {
            transform.rotation = Quaternion.Lerp(originalRotation, flippedRotation, step);
            step += Time.deltaTime / timeTaken;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void closeAllDoor()
    {
        allClosed = true;
    }

	public void openNextDoor()
    {
        doorIndex++;
        if(doorIndex == door.Count)
        {
            doorIndex = 0;
        }
        openDoor = doorIndex;
        allClosed = false;
    }
}
