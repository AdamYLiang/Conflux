using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverScript : MonoBehaviour {

    private bool complete = false;
    private int indexOnList = -1;
	// Use this for initialization
	void Start () {

        indexOnList = transform.root.GetComponent<PuzzleManager>().receiverCompletion.Count;
        transform.root.GetComponent<PuzzleManager>().receiverCompletion.Add(false);

	}
	
	// Update is called once per frame
	void Update () {

        complete = transform.parent.GetComponent<ConnectedInfo>().complete;

        if (complete)
        {
            transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = true;
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = false;
            GetComponent<Renderer>().material.color = Color.white;
        }
       
	}
}
