using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        controller = SteamVR_Controller.Input((int)trackedObj.index);
        if(controller.GetAxis().x != 0 || controller.GetAxis().y != 0)
        {
            Debug.Log("X Axis is " + controller.GetAxis().x + " and Y axis is " + controller.GetAxis().y);
        }

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Trigger Pressed");
            controller.TriggerHapticPulse(700);
        }

    }
}