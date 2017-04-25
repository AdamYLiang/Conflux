using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerColDetect : MonoBehaviour
{
    public GameObject selected = null;
    private GameObject highlighted = null;

    public SteamVR_Controller.Device mainController;
    public SteamVR_TrackedObject trackedObj;

    public Material oldMat = null;

    void Start()
    {
        trackedObj = transform.parent.GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        mainController = SteamVR_Controller.Input((int)trackedObj.index);
        if (mainController.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            if(GetComponent<SpringJoint>() != null)
            {
                Destroy(GetComponent<SpringJoint>());
            }
        }


        /*
       //If the selected obj is not the one that is highlighted, we have a new selected.
       if(selected != highlighted)
       {
           //If the highlighted wasn't null, we need to reset the old highlighted obj.
           if(highlighted != null)
           {
               RemoveHighlight(highlighted.transform);
           }

           //If the selected obj isn't null, we need to apply the highlight.
           if(selected != null)
           {
               ApplyHighlight(selected.transform);
           }

           //Regardless of what happened the highlighted object should now be the selected.
           highlighted = selected;
       }*/
    }

    //Applies the shader to the transform.
    public void ApplyHighlight(Transform trans)
    {
        oldMat = trans.GetComponent<MeshRenderer>().material;
        Material mat = new Material(Shader.Find("Custom/HighlightShader"));
        mat.CopyPropertiesFromMaterial(oldMat);
        trans.GetComponent<MeshRenderer>().material = mat;
    }

    //Resets the shader of the given transform if possible.
    public bool RemoveHighlight(Transform trans)
    {
        if (oldMat != null)
        {
            trans.GetComponent<MeshRenderer>().material = oldMat;
            oldMat = null;
            return true;
        }
        return false;

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Interactable"))
        {
            selected = col.gameObject;
        }
    }

    //If you collide with a puzzle set it to true etc
    void OnTriggerStay(Collider col)
    {
        //Debug.Log(activator.name);

        if (col.tag == "CubePuzzle")
        {
            // Debug.Log("ITS PUZZLE TIME BOYZ");
            //transform.parent.GetComponent<RotationController>().isTouchingPuzzle = true;
            //transform.parent.GetComponent<RotationController>().currentPuzzle = col.transform.root.gameObject;
        }
        //Found interactable object
        else if (col.name == "Interactable")
        {
            //If we press down pick up the interactable object.
            if (mainController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) || Input.GetKeyDown(KeyCode.A))
            {
                SpringJoint joint = gameObject.AddComponent<SpringJoint>();
                col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                joint.connectedBody = col.gameObject.GetComponent<Rigidbody>();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Interactable"))
        {
            selected = null;
            RemoveHighlight(col.transform);
        }
    }
}
