using UnityEngine;

public class PickupScript : MonoBehaviour
{
    private SteamVR_TrackedController _controller;
    private PrimitiveType _currentPrimitiveType = PrimitiveType.Sphere;
    private GameObject pickupObject;
    private bool holdingObject = false; 

    private void OnEnable()
    {
        _controller = GetComponent<SteamVR_TrackedController>();
        _controller.TriggerClicked += HandleTriggerClicked;
        _controller.PadClicked += HandlePadClicked;
    }

    private void OnDisable()
    {
        _controller.TriggerClicked -= HandleTriggerClicked;
        _controller.PadClicked -= HandlePadClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log("Attempting to pick up or drop");
        if(pickupObject != null)
        {
            PickUpTheObject();
        }

        if (holdingObject)
        {
           // Debug.Log("dropping");
            pickupObject.transform.parent = null;
            pickupObject.GetComponent<Rigidbody>().useGravity = true;
            pickupObject.GetComponent<Rigidbody>().isKinematic = false;
            holdingObject = false;
        }

    }

    public void PickUpTheObject()
    {
            //Debug.Log("picking up");
            holdingObject = true;
            pickupObject.transform.parent = this.transform;
            pickupObject.GetComponent<Rigidbody>().useGravity = false;
            pickupObject.GetComponent<Rigidbody>().isKinematic = true;

    }

    public void OnTriggerEnter(Collider activator)
    {
        //Debug.Log(activator.name + " " + activator.tag);

        if (activator.tag == "pickupable")
        {
            pickupObject = activator.gameObject;
        }
    }

    public void OnTriggerExit(Collider activator)
    {
        pickupObject = null;
    }

 
    private void HandlePadClicked(object sender, ClickedEventArgs e)
    {
    }
}