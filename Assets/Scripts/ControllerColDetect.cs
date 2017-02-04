using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerColDetect : MonoBehaviour {

    void OnTriggerStay(Collider activator)
    {
        if(activator.tag == "CubePuzzle")
        {
           // Debug.Log("ITS PUZZLE TIME BOYZ");
            transform.parent.GetComponent<RotationController>().isTouchingPuzzle = true;
            transform.parent.GetComponent<RotationController>().currentPuzzle = activator.transform.root.gameObject;
        }
    }

    //void OnTriggerExit(Collider activator)
    //{
    //    if(activator.tag == "CubePuzzle")
    //    {
    //        Debug.Log("Left the puzzle how dare you");
    //        transform.parent.GetComponent<RotationController>().isTouchingPuzzle = false;
    //        activator.transform.root.gameObject.GetComponent<PuzzleManager>().play = false;
    //        transform.parent.GetComponent<RotationController>().currentPuzzle = null;
    //    }
    //}
}
