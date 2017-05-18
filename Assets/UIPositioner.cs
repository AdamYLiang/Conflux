using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositioner : MonoBehaviour {

    //The UI Objects.
    public GameObject[] UIObjects;

    public GameObject debug;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceIndicator(debug.transform);
        }
    }

    //Function to place indicator object at objects position and rotation.
    public void PlaceIndicator(Transform obj)
    {
        PlaceAndStart(0, obj.position, obj.eulerAngles);
    }

    //Places and starts the UI Object 
    public void PlaceAndStart(int index, Vector3 position, Vector3 rotation)
    {
        UIObjects[index].transform.position = position;
        UIObjects[index].transform.eulerAngles = rotation;
        UIObjects[index].GetComponent<UIController>().TurnOn();
    }
}
