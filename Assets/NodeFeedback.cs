﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeFeedback : MonoBehaviour {

    public float detectRange = 5, maxSize;

    public GameObject controller1, controller2;
    public float growTime = 1f;

    public Color closeColor;
    protected Color originalColor;

    protected bool onColor = false;
    protected Vector3 originalScale;

    protected float colorStep = 0f;
    protected Material ourMat;

	// Use this for initialization
	void Start ()
    {
        controller1 = GameObject.Find("Controller (left)");
        controller2 = GameObject.Find("Controller (right)");
        originalScale = transform.GetChild(0).localScale;
        ourMat = transform.GetChild(0).GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(controller1 == null)
        {
            controller1 = GameObject.Find("Controller (left)");
        }

        if(controller2 == null)
        {
            controller2 = GameObject.Find("Controller (right)");
        }

        if(controller1 != null && controller2 != null)
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

            if (chosen < detectRange)
            {
                //Debug.Log(chosen);
            }

            //Default 1 will keep it the normal size. We want hte min to be 1, and the max to be maxSize.
            //We want it to scale bigger the closer it gets to 0. Our max range of default scaling should be detectRange
            float scaling = 1;
            //We only operate if our detectRange is greater than chosen. If we are equal, no math required.
            if (detectRange > chosen)
            {
                //DetectRange - chosen will be greater the smaller chosen is, i.e. closer to the node
                //Thus, with a detect range of 5, and a chosen distance of 3, our scale modifier will be 0.4f.
                //At a range of 5 with chosen distance of 1, our scale modifeier will be 0.8f;
                scaling =  (1 - (chosen/detectRange)) * (maxSize) + 1;
                //Debug.Log(scaling);
                onColor = true;
            }
            else
            {
                onColor = false;
            }

            if (onColor)
            {
                colorStep = Mathf.Clamp01(colorStep + Time.deltaTime * 2);
            }
            else
            {
                colorStep = Mathf.Clamp01(colorStep - Time.deltaTime * 2);
            }

            ourMat.color = Color.Lerp(originalColor, closeColor, colorStep);
            transform.GetChild(0).localScale = originalScale * scaling;
        }
        else
        {

            controller1 = GameObject.Find("Controller1");
            controller2 = GameObject.Find("Controller2");
        }
    }
}