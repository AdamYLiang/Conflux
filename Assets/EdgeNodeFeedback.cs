using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeNodeFeedback : MonoBehaviour {

    public float detectRange = 5, maxSize;

    public GameObject controller1, controller2;
    public float growTime = 1f;
    public bool react = true;

    protected GameObject gameManager;

    protected Vector3 originalScale;


    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager");

        controller1 = gameManager.GetComponent<GameManager>().controller1;
        controller2 = gameManager.GetComponent<GameManager>().controller2;

        originalScale = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        controller1 = gameManager.GetComponent<GameManager>().controller1;
        controller2 = gameManager.GetComponent<GameManager>().controller2;

        if (controller1 != null && controller2 != null && react)
        {
            //Calculate the distance from controller to node
            float distance = (controller1.transform.position - transform.position).magnitude;
            float distance2 = (controller2.transform.position - transform.position).magnitude;

            //Pick the smallest distance to use as our measurement. i.e. we want the closest controller to affect our size
            float chosen = 0;
            if (distance > distance2)
            {
                chosen = distance2;
            }
            else // Instead of checking for less than, just an else is fine. If it is equal it won't matter which one we pick anyway.
            {
                chosen = distance;
            }

            //Default 1 will keep it the normal size. We want hte min to be 1, and the max to be maxSize.
            //We want it to scale bigger the closer it gets to 0. Our max range of default scaling should be detectRange
            float scaling = 1;
            //We only operate if our detectRange is greater than chosen. If we are equal, no math required.
            if (detectRange > chosen)
            {
                //Render it
                transform.GetComponent<Renderer>().enabled = true;

                //DetectRange - chosen will be greater the smaller chosen is, i.e. closer to the node
                //Thus, with a detect range of 5, and a chosen distance of 3, our scale modifier will be 0.4f.
                //At a range of 5 with chosen distance of 1, our scale modifeier will be 0.8f;
                //Note: Issues because we are dividing decimals, which actually makes it bigger.
                scaling = ((detectRange - chosen) / detectRange) * maxSize + 0.2f;
                scaling = Mathf.Min(scaling, 2f);
                if (scaling < 0f)
                {
                    scaling = 0;
                }
            }
            else
            {
                scaling = 0.2f;
            }
            transform.localScale = originalScale * scaling;
        }
        else if(!react)
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        react = false;
        
        transform.GetChild(0).localScale = originalScale * 2.5f;
    }
}
