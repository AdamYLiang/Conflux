﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StartButton : MonoBehaviour {

    //The image we are manipulating
    public Image indicator;

    //Wait timer variables
    public float waitTime = 3f;
    protected float waitTimer;

    //Our HMD reference
    protected GameObject hmd;

    //Bool for being on the button
    protected bool entered = false;

    //Bool for having stood on the button long enough
    public bool standConfirmed = false;

    //Bool to begin the color change
    protected bool changeToConfirm;

    //Color references
    public Color confirmedColor;
    protected Color originalColor;

    //Color change timer variables
    public float colorChangeTime = 2f;
    protected float colorChangeStep = 0f;

    //Fire only once
    public bool fireOnce = true;
    protected bool fired = false;

    //Event variable
    public UnityEvent OnStandInside = new UnityEvent();
    public UnityEvent OnFinishRotation = new UnityEvent();

    void Start()
    {
        waitTimer = 0;
        originalColor = indicator.color;
    }
	
	// Update is called once per frame
	void Update () {

        if (!standConfirmed)
        {
            //When entered, count down to zero.
            if (entered)
            {
                waitTimer = Mathf.Clamp01(waitTimer + (Time.deltaTime / waitTime));
            }
            else
            {
                waitTimer = Mathf.Clamp01(waitTimer - (Time.deltaTime / waitTime));
            }

            indicator.fillAmount = waitTimer;
        }

        //Set the bools once we've completed
        if(waitTimer == 1)
        {
            changeToConfirm = true;
            standConfirmed = true;
            if (!fired)
            {
                OnStandInside.Invoke();
                fired = true;
            }
        }

        //Lerp the color to our confirm color
        if (changeToConfirm)
        {
            colorChangeStep = Mathf.Clamp01(colorChangeStep + Time.deltaTime / colorChangeTime); 
            indicator.color = Color.Lerp(originalColor, confirmedColor, colorChangeStep);
        }
	}

    //On enter
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name == "Camera (head)")
        {
            entered = true;
        }
    }

    //On exit
    void OnTriggerExit(Collider col)
    {
        if (col.transform.name == "Camera (head)")
        {
            entered = false;
        }
        //If we aren't firing once reset the correct bools.
        if (!fireOnce)
        {
            fired = false;
            waitTimer = 0f;
            changeToConfirm = false;
            standConfirmed = false;
            indicator.color = originalColor;
        }
    }

    public IEnumerator RotateObj(GameObject obj, float rotation)
    {
        bool finishedRotating = false;
        float totalRotation = 0f;
        while (!finishedRotating)
        {
            obj.transform.RotateAround(transform.position, Vector3.up, 1f);
            totalRotation += 1f;
            if(totalRotation == rotation)
            {
                finishedRotating = true;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        OnFinishRotation.Invoke();
    }

    //Rotate object around us
    public void RotateObjectAround(GameObject obj)
    {
        StartCoroutine(RotateObj(obj, 180f));
    }

}