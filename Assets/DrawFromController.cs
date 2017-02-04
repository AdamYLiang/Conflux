using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFromController : MonoBehaviour {

    public GameObject emitter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(emitter != null)
            {
                emitter.GetComponent<EmitterScript>().EndDraw();
                emitter = null;
            }
        }
	}

    void OnTriggerStay(Collider col)
    {
        //Found a connection node.
        if (col.name == "ConnectionNode")
        {
            //Found a connection node thats part of an emitter.
            if(col.gameObject.transform.parent.name.Contains("Emitter"))
            {
                //Press a key and set our new draw origin to here.
                if (Input.GetKeyDown(KeyCode.X) && col.gameObject.transform.root.GetComponent<PuzzleManager>().play)
                {
                    emitter = col.gameObject.transform.parent.gameObject;
                    emitter.GetComponent<EmitterScript>().StartDraw(gameObject);
                }
              
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //Found a connection node.
        if(col.gameObject.name.Contains("ConnectionNode"))
        {
            if(emitter != null)
            {
                //If we have already connected to this node before...
                if (emitter.GetComponent<EmitterScript>().linePositions.Contains(col.gameObject))
                {   //If this was the last node we connected to... (Note: Excluding the controller node which is at the end)
                    if(emitter.GetComponent<EmitterScript>().linePositions.IndexOf(col.gameObject) == 
                        (emitter.GetComponent<EmitterScript>().linePositions.Count - 2))
                    {
                        //If this isn't the original node...
                        if(emitter.GetComponent<EmitterScript>().linePositions.IndexOf(col.gameObject) != 0)
                        {
                            //Remove it from the list.
                            emitter.GetComponent<EmitterScript>().linePositions.Remove(col.gameObject);
                        }   
                    }
                }//New node, attempt to add it.
                else
                {
                    Vector3 position = col.transform.parent.position;
                    emitter.GetComponent<EmitterScript>().AddLineNode(col.gameObject);

                }

                //If this is a receiver, we need to end the drawing after connecting.
                if (col.gameObject.transform.parent.name.Contains("Receiver"))
                {
                    emitter.GetComponent<EmitterScript>().EndDraw();
                    emitter = null;
                }
            }
        }

    }
}
