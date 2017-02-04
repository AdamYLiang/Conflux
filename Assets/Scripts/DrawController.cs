using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller;

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
        controller = SteamVR_Controller.Input((int)trackedObj.index);

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {

        }
    }
}
