using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionNOde : MonoBehaviour {

    public bool connected = false;
    public Vector3 coordinate = -Vector3.one;

    public enum NodeType { Cube, Other };
    public NodeType type;

    ConnectedInfo info;

    void Start()
    {
        if (GetComponent<ConnectedInfo>() != null)
        {
            info = GetComponent<ConnectedInfo>();
        }
    }

    public void RetrieveCoordinate()
    {
        //If this returns a negative vector then something went wrong.
        Vector3 returnVector = -Vector3.one;
        string name = "";

        //It will always be the name of its parent.
        name = gameObject.name;
        if (name.Contains("Connection"))
        {
            name = transform.parent.name;
        }

        //Extract the coordinate from the name.
        //Skip 0 since it is '('
        int counter = 1;
        string x = "", y = "", z = "";

        //While we don't reach ','
        while (name[counter] != ',')
        {
            //Debug.Log(name[counter]);
            x += name[counter];
            //Exit if we reach the end.
            if (counter == name.Length)
            {
                break;
            }
            counter++;
        }

        //Increment by one to pass over the comma
        counter++;
        while (name[counter] != ',')
        {
            y += name[counter];
            if (counter == name.Length)
            {
                break;
            }
            counter++;
        }

        //Increment by one to pass over the next comma
        counter++;

        //For the last one we check for ')'
        while (name[counter] != ')')
        {
            z += name[counter];
            if (counter == name.Length)
            {
                break;
            }
            counter++;
        }

        int xCoord = int.Parse(x);
        int yCoord = int.Parse(y);
        int zCoord = int.Parse(z);
        returnVector = new Vector3(xCoord, yCoord, zCoord);
        coordinate = returnVector;
    }
}
