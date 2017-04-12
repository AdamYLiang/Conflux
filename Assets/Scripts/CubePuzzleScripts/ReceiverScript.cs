using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverScript : MonoBehaviour {

    private PuzzleManager manager;
    private ConnectedInfo info;
    private bool complete = false;
    private int indexOnList = -1;

    Vector3 scale = new Vector3(0.6f, 0.15f, 0.6f);

	// Use this for initialization
	void Start () {
        transform.parent.localScale = scale;
        //indexOnList = transform.root.GetComponent<PuzzleManager>().receiverCompletion.Count;
		indexOnList = transform.parent.parent.parent.GetComponent<PuzzleManager>().receiverCompletion.Count;
        //transform.root.GetComponent<PuzzleManager>().receiverCompletion.Add(false);
		transform.parent.parent.parent.GetComponent<PuzzleManager>().receiverCompletion.Add(false);
        info = transform.parent.GetComponent<ConnectedInfo>();
        manager = transform.root.GetComponent<PuzzleManager>();

	}
	
	// Update is called once per frame
	void Update () {

        complete = info.complete;

        if (complete)
        {
            //transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = true;
			transform.parent.parent.parent.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = true;
            transform.FindChild("Crystal_Emitter:pCylinder3").GetComponent<Renderer>().material.color = info.receivedRGBColor;
        }
        else
        {
            //transform.root.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = false;
			transform.parent.parent.parent.GetComponent<PuzzleManager>().receiverCompletion[indexOnList] = false;
            transform.FindChild("Crystal_Emitter:pCylinder3").GetComponent<Renderer>().material.color = info.incompleteRGBColor;
        }
       
	}
}
