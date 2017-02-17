using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to access unity's VR functions, add the namespace
using UnityEngine.VR;

public class MouseLook : MonoBehaviour
{

    public float mouseX, mouseY;

    private bool VRMode;

    // Use this for initialization
    void Start()
    {
        VRMode = VRDevice.isPresent;
        if (VRMode)
        {
            Debug.Log("VR Device: " + VRDevice.model);
        }
        else
        {
            //Camera.main.transform.Translate(0, 2f, 0);
            transform.root.Translate(0, 2f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!VRMode)
        {
            //Do emergency input code if we're not using VR
            var cam = gameObject.transform;

            //Rotate camera based on mouse delta speed
            cam.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);

            //Unroll the camera by setting z to zero no matter what
            cam.localEulerAngles = new Vector3(cam.localEulerAngles.x, cam.localEulerAngles.y, 0);
        }
    }


}
