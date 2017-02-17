using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFromController : MonoBehaviour {

    public GameObject emitter;
    public SteamVR_Controller.Device mainController;
    public SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Start () {
        trackedObj = transform.parent.GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        mainController = SteamVR_Controller.Input((int)trackedObj.index);
        if (mainController.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            if (emitter != null)
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
                if (mainController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && col.gameObject.transform.root.GetComponent<PuzzleManager>().play)
                {
                    transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
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
            //If we are currently drawing
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
                            emitter.GetComponent<EmitterScript>().RemoveLineNode(col.gameObject);
                        }   
                    }
                }//New node, attempt to add it, ONLY if it isn't already connected.
                else if (col.GetComponent<ConnectionNOde>() != null)
                {
                    if (!col.GetComponent<ConnectionNOde>().connected)
                    {
                        Vector3 position = col.transform.parent.position;
                        emitter.GetComponent<EmitterScript>().AddLineNode(col.gameObject);
                    }
                }//New node and edge connection
                else if (col.transform.parent.name == "EdgeConnections")
                {
                    Vector3 position = col.transform.position;
                    emitter.GetComponent<EmitterScript>().AddLineNode(col.gameObject);
                }
                else if (col.transform.parent.name.Contains("Receiver"))
                {
                    if (!col.transform.parent.GetComponent<ConnectionNOde>().connected)
                    {
                        Vector3 position = col.transform.parent.position;
                        emitter.GetComponent<EmitterScript>().AddLineNode(col.gameObject);
                        Debug.Log("Receiver isn't connected... Let's connect it to our emitter.");

                        ConnectedInfo info = col.transform.parent.GetComponent<ConnectedInfo>();
                        info.receivedLaserColor = emitter.GetComponent<EmitterScript>().laserColor;
                        info.received = true;
                        emitter.GetComponent<EmitterScript>().connected = true;

                        //Disconnect us from this emitter after connecting.
                        emitter.GetComponent<EmitterScript>().EndDraw();
                        emitter = null;
                    }
                    
                }

            }
        }

    }
}
