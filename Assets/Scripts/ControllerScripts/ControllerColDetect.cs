using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerColDetect : MonoBehaviour
{

    //If you collide with a puzzle set it to true etc
    void OnTriggerStay(Collider activator)
    {
        //Debug.Log(activator.name);

        if (activator.tag == "CubePuzzle")
        {
            // Debug.Log("ITS PUZZLE TIME BOYZ");
            transform.parent.GetComponent<RotationController>().isTouchingPuzzle = true;
            transform.parent.GetComponent<RotationController>().currentPuzzle = activator.transform.root.gameObject;
        }

    }
}
