using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverScript : MonoBehaviour {

    private PuzzleManager manager;
    private ConnectedInfo info;
    private bool complete = false;
    private int indexOnList = -1;
	// Use this for initialization
	void Start () {

        indexOnList = transform.root.GetComponent<PuzzleManager>().receiverCompletion.Count;
        transform.root.GetComponent<PuzzleManager>().receiverCompletion.Add(false);
        info = transform.parent.GetComponent<ConnectedInfo>();
        manager = transform.root.GetComponent<PuzzleManager>();

	}
	
	// Update is called once per frame
	void Update () {

        complete = info.complete;

        if (complete)
        {
            transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = true;
            GetComponent<Renderer>().material.color = info.receivedRGBColor;
        }
        else
        {
            transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = false;
            GetComponent<Renderer>().material.color = info.incompleteRGBColor;
        }
       
	}
}
