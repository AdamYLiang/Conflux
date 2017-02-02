using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterScript : MonoBehaviour {

    LineRenderer lr;

    public enum LaserColor { Red, Blue };

    public LaserColor laserColor = LaserColor.Blue;
    float verticalAdjust = 0.5f;

    public GameObject simulatedController;
    public GameObject emitter;
    GameObject receiver = null;

    //All the positions we need to add to the grid
    public List<GameObject> linePositions = new List<GameObject>();
    //A conversion of the list above to the coordinates on the cube
    public List<Vector3> linePositionsOnGrid = new List<Vector3>();
    //The size of one step in any direction on the grid
    public float unitSize = 2.5f, cornerDifference = 0.75f, heightDifference = 0.5f;

    private Vector3 laserOriginCoordinate;

    public LayerMask checkLayerMask;
    public bool drawing = false;

	// Use this for initialization
	void Start () {
        lr = transform.FindChild("LaserRenderer").GetComponent<LineRenderer>();

        //Find our coordinate by checking our name and converting it to a Vector3
        char[] letters = transform.name.ToCharArray();
        string x = "" + letters[1];
        string y = "" + letters[4];
        string z = "" + letters[7];
        //Debug.Log(x + ", " + y + ", " + z);
        laserOriginCoordinate = new Vector3(int.Parse(x), int.Parse(y), int.Parse(z));
        //Debug.Log(laserOriginCoordinate);

        //Initialize the emitter.
        linePositions.Add((transform.FindChild("ConnectionNode").gameObject));
        linePositions.Add(new GameObject());
        /*linePositions.Add(new Vector3(heightDifference, 1 * unitSize, 0));
        linePositions.Add(new Vector3(heightDifference, (2 + cornerDifference) * unitSize, 0));
        linePositions.Add(new Vector3(-2 * heightDifference, (2 + cornerDifference) * unitSize, 0));
        linePositions.Add(new Vector3(-2 * heightDifference, (2 + cornerDifference) * unitSize, unitSize));
        //ConvertCoordinateToCubeCoord(new Vector3(0,-unitSize,0));
        //UpdateList();*/
	}
	
	// Update is called once per frame
	void Update () {
        /*
        Ray forwardRay = new Ray(transform.position + (transform.up * verticalAdjust), transform.right);
        RaycastHit rayInfo = new RaycastHit();
        if (Physics.Raycast(forwardRay, out rayInfo, 100f))
        {
            float distance = Vector3.Distance(transform.position, rayInfo.point);
            lr.SetPosition(1, new Vector3(0, -distance * 2.5f, 0));

            if (rayInfo.collider.name == "Receiver")
            {
                rayInfo.collider.GetComponent<ReceiverScript>().receivedLaserColor = laserColor;
                rayInfo.collider.GetComponent<ReceiverScript>().received = true;
                receiver = rayInfo.collider.gameObject;
            }
        }
        else if (receiver != null)
        {
            
            receiver.GetComponent<ReceiverScript>().received = false;
            //receiver = null;
        }*/
        
        if(simulatedController  != null && drawing)
        {
            FollowController();
        }

        if (!drawing)
        {
            linePositions.Remove(simulatedController);
        }

        if(linePositions.Count > 0)
        {
            UpdateList();
            CheckCompletions();
        }
       
      
    }
    /*
    public void UpdateList()
    {
        linePositionsOnGrid.Clear();
        for (int i = 0; i < linePositions.Count; i++)
        {
            linePositionsOnGrid.Add(ConvertCoordinateToCubeCoord(linePositions[i]));
        }

    }
    
    public Vector3 ConvertCoordinateToCubeCoord(Vector3 laserCoord)
    {
        Vector3 returnVector = -Vector3.one;
        Vector3 originPoint = laserOriginCoordinate;
        FixRotation.Face face = GetComponent<FixRotation>().face;
        returnVector.x = laserCoord.x / unitSize;
        returnVector.y = laserCoord.y / unitSize;
        returnVector.z = laserCoord.z / unitSize;

        //Operations according to face
        switch ((int)face)
        {
            //North face. X and Y are inverted
            case 0:
                returnVector.x = -laserCoord.y / unitSize;
                returnVector.y = -laserCoord.z / unitSize;
                returnVector.z = laserCoord.x / unitSize;
                break;
            //East face. Y and Z are inverted, but the Y value is read in as negative.
            case 1:
                returnVector.x = laserCoord.x / unitSize;
                returnVector.y = laserCoord.z / unitSize;
                returnVector.z = laserCoord.y / unitSize;
                break;
            //South face. Inverted of north.
            case 2:
                returnVector.x = laserCoord.y / unitSize;
                returnVector.y = laserCoord.z / unitSize;
                returnVector.z = -laserCoord.x / unitSize;
                break;
            //West face. Inverted of East.
            case 3:
                returnVector.x = -laserCoord.x / unitSize;
                returnVector.y = laserCoord.z / unitSize;
                returnVector.z = -laserCoord.y / unitSize;
                break;
            //Bottom face.
            case 4:
                returnVector.x = -laserCoord.y / unitSize;
                returnVector.y = -laserCoord.x / unitSize;
                returnVector.z = -laserCoord.z / unitSize;
                break;
            //Top face. Inverted of top.
            case 5:
                returnVector.x = -laserCoord.y / unitSize;
                returnVector.y = -laserCoord.x / unitSize;
                returnVector.z = -laserCoord.z / unitSize;

                break;
            default:
                //No change.
                Debug.Log("Conversion Error: Invalid face.");
                break;
        }
        //Add the originPoint to compensate for the tile being at possibly many different parts of the cube.
        returnVector += originPoint;
        returnVector = new Vector3((int)returnVector.x, (int)returnVector.y, (int)returnVector.z);
        //Debug.Log(returnVector);
        return returnVector;
    }
    
    
    //Computes the necessary distances based on the coordinates we are on. Position is the controller's current position.
    public float[] ComputeDistances(FixRotation.Face face, Vector3 position)
    {
        //Distances around the four cardinal directions. Calculate which cardinal direction we are closest to.
        float distanceToForward, distanceToBackward, distanceToRight, distanceToLeft, distanceToOutside, distanceToInside;

        //Checkthe laser's coordinates for the face relative to its origin it is on
        Vector3 laserLatestCoordinate = linePositions[linePositions.Count - 1];
        bool useX = false;        

        //The position we want to compare to. First need the laser position. Transform its local coordinates to the real world ones.
        Vector3 laserLatestPosition = emitter.transform.TransformPoint(laserLatestCoordinate);
        Vector3 targetDirection = Vector3.forward;

        //The directions we need to check.


        //Raycast from the laserLatestPosition to find if we've hit the wall, and change accordingly.
        for (int i = 0; i < 6; i++)
        {
            Ray checkRebound = new Ray(laserLatestPosition, targetDirection);
            RaycastHit checkInfo = new RaycastHit();
            if (Physics.Raycast(checkRebound, out checkInfo, 1000f))
            {
                if (checkInfo.collider.tag == "ReboundWalls")
                {
                    useX = true;
                }
            }
        }

        //Compute the distances in the four possible directions
        distanceToForward = Vector3.Distance(position, laserLatestPosition + transform.forward);
        distanceToBackward = Vector3.Distance(position, laserLatestPosition - transform.forward);
        distanceToRight = Vector3.Distance(position, laserLatestPosition + transform.right);
        distanceToLeft = Vector3.Distance(position, laserLatestPosition - transform.right);
        distanceToOutside = Vector3.Distance(position, laserLatestPosition + transform.up);
        distanceToInside = Vector3.Distance(position, laserLatestPosition - transform.up);

        float[] returnArray = { distanceToForward, distanceToBackward, distanceToRight, distanceToLeft, distanceToOutside, distanceToInside };
        return returnArray;
        

    }
    
    //Return a list that shows which faces in relation to a single face are on the Z-axis
    public FixRotation.Face ReturnOppositeFace (FixRotation.Face face)
    {
        switch ((int)face)
        {
            //North Face
            case 0: return FixRotation.Face.South;
            //East face
            case 1: return FixRotation.Face.West;
            //South face
            case 2: return FixRotation.Face.North;
            //West face
            case 3: return FixRotation.Face.East;
            //Bottom face
            case 4: return FixRotation.Face.Top;
            //Top face
            case 5: return FixRotation.Face.Bottom;
            //Erro
            default: Debug.Log("ReturnOppositeFace had an error. Incorrect input?");
                return FixRotation.Face.Top;
        }
    }

    //Select lowest value, return it's index.
    public int SelectLowest(float[] numbers)
    {
        int returnInt = -1;
        float testFloat = float.MaxValue;
        for(int i= 0; i < numbers.Length; i++)
        {
            if(testFloat > numbers[i])
            {
                returnInt = i;
                testFloat = numbers[i];
            }
        }
        Debug.Log("Lowest is " + returnInt);
        return returnInt;
    }

    //Takes the position of the controller and gives us the cardinal direction we should use.
    public Vector3 ApproximatePosition(FixRotation.Face face, Vector3 position)
    {
        //Return variable. If still this negative then something went wrong.
        Vector3 returnVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        //First check if the distance from the origin is greater than 1 and thus a valid input. Any less should be regarded as no change.
        if(Vector3.Distance(transform.position, position) <= 1)
        {
            return returnVector;
        }

        //Using the helper function find the direction that is closest to the controller.
        //NOTE: index 0 is forward, 1 is backward, 2 is right, 3 is left
        float[] distances = ComputeDistances(face, position);
        int lowestDistance = SelectLowest(distances);

        //Debug.Log(distanceToForward + " " + distanceToBackward + " " + distanceToRight + " " + distanceToLeft);
        Debug.Log(lowestDistance);
        switch (lowestDistance)
        {
            case 0: return Vector3.forward;
            case 1: return Vector3.back;
            case 2: return Vector3.right;
            case 3: return Vector3.left;
            default: return returnVector;
        }

    }*/

    //Check for completions
    public void CheckCompletions()
    {
        foreach(GameObject node in linePositions)
        {
            if(node.transform.parent != null)
            {
                //Debug.Log(node.transform.parent.name);
                if (node.transform.parent.name.Contains("Receiver"))
                {
                    //Debug.Log("Went through");
                    node.transform.parent.FindChild("Receiver").GetComponent<ReceiverScript>().receivedLaserColor = laserColor;
                    node.transform.parent.FindChild("Receiver").GetComponent<ReceiverScript>().received = true;
                }
            }
           
        }
    }
    
    //Update the list
    public void UpdateList()
    {
        Vector3[] lrPositions;
        int i = 0;
        lrPositions = new Vector3[linePositions.Count];
        foreach(GameObject node in linePositions)
        {
            lrPositions[i] = node.transform.position;
          
            if(i != linePositions.Count - 1)
            {
                lrPositions[i] += node.transform.up * 0.33f;
            }
            lrPositions[i] = emitter.transform.InverseTransformPoint(lrPositions[i]);

            i++;
        }

        emitter.GetComponent<LineRenderer>().numPositions = lrPositions.Length;
        emitter.GetComponent<LineRenderer>().SetPositions(lrPositions);
    }

    //Follow the controller.
    public void FollowController()
    {
        linePositions[linePositions.Count - 1] = simulatedController;
    }

    //Add a position to the line
    public void AddLineNode(GameObject node)
    {
        //Only add it if it is a valid position.
        if (CheckViableNode(node))
        {
            //Add a Vector. It doesn't matter what value it is since the top level of the list is always connectede to the controller.
            linePositions.Add(node);
            //Set the vector before as the target position.
            linePositions[linePositions.Count - 2] = node;
        }
    }

    //Check if the position is invalid.
    //Case 0: Node has already been used.
    //Case 1: Something is between our last node and this one.
    //Case 2: Node is not directly next to the previous node.
    public bool CheckViableNode(GameObject node)
    {

        //Case 0: Node has already been used.
        for (int i = 0; i < linePositions.Count; i++)
        {
            if (node == linePositions[i])
            {
                return false;
            }
        }

        //Case 1: A wall/rubber is between us and the next node.
        GameObject lastObject = linePositions[linePositions.Count - 2];
        Debug.Log(lastObject);
        Vector3 targetDirection = (lastObject.transform.position + lastObject.transform.up * 0.33f) - (node.transform.position + node.transform.up * 0.33f);
        Ray targetRay = new Ray(node.transform.position + node.transform.up * 0.33f, targetDirection);
        Debug.DrawRay(node.transform.position + node.transform.up * 0.33f, targetDirection, Color.red, 10f);
        RaycastHit hitInfo = new RaycastHit();
        if(Physics.Raycast(targetRay, out hitInfo, (lastObject.transform.position - node.transform.position).magnitude ,checkLayerMask))
        {
            //Debug.Log(hitInfo.collider.name);
            if(hitInfo.collider.name.Contains("Wall") || hitInfo.collider.name.Contains("Rubber"))
            {
                //Debug.Log("Detected a wall/rubber between." + hitInfo.collider.name);
                return false;
            }
        }

        //Case 2: Node is not directly next to previous node.
        //Previous node. Not the last node since that one is the cursor.
        Vector3 lastPosition = linePositions[linePositions.Count - 2].transform.position;
        Vector3 difference = lastPosition - node.transform.position;
        float distance = difference.magnitude;
        //distance = Mathf.Round(distance);

        if (distance < 0.95 || distance > 1.05)
        {
            //Vertex node
            if(distance > 0.45 && distance < 0.55)
            {
                return true;
            }
           Debug.Log(distance + " " + difference);
           return false;
        }
        return true;
    }

}
