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
                emitter.GetComponent<EmitterScript>().simulatedController = null;
                emitter.GetComponent<EmitterScript>().drawing = false;
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
                if (Input.GetKeyDown(KeyCode.X))
                {
                    emitter = col.gameObject.transform.parent.gameObject;
                    emitter.GetComponent<EmitterScript>().simulatedController = gameObject;
                    emitter.GetComponent<EmitterScript>().drawing = true;
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
                Vector3 position = col.transform.parent.position;
                emitter.GetComponent<EmitterScript>().AddLineNode(col.gameObject);
            }
        }
    }
}
