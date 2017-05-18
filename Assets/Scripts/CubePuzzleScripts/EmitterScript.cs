using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmitterScript : MonoBehaviour {

    LineRenderer lr;

    public enum LaserColor { Red, Blue, Yellow, Pink, Magenta, Green, Grey, Cyan, Brown, Purple, Orange, None};

    public LaserColor laserColor = LaserColor.Blue;
    private Color laserPigment;
    float verticalAdjust = 0.5f;

    public GameObject drawNode;
    public GameObject simulatedController;
    public GameObject emitter;
    GameObject receiver = null;

    //All the positions we need to add to the grid
    public List<GameObject> linePositions = new List<GameObject>();
    //A conversion of the list above to the coordinates on the cube
    public List<GameObject> connectedObjects = new List<GameObject>();
    //The size of one step in any direction on the grid
    public float unitSize = 2.5f, cornerDifference = 0.75f, heightDifference = 0.5f;

    private Vector3 laserOriginCoordinate;

    public LayerMask checkLayerMask;
    public bool drawing = false, connected = false;

    private float heightFactor = 0.33f, puzzleScale = 1.0f;
    private PuzzleManager manager;

    Vector3 scale = new Vector3(1f, .25f, 1f);

    //The indicator prefab
    public GameObject indicator;

    //The corresponding receiver it should be going to.
    public GameObject endReceiver;

    //Event that is calle when we begin drawing.
    //public UnityEvent OnStartDraw;

    // Use this for initialization
    void Start () {
        //manager = transform.root.GetComponent<PuzzleManager>();
		manager = transform.parent.parent.GetComponent<PuzzleManager>(); //have to account for new hierarchy
        puzzleScale = manager.gameObject.transform.lossyScale.x;
        lr = transform.FindChild("LaserRenderer").GetComponent<LineRenderer>();
        lr.GetComponent<LineRenderer>().startWidth *= puzzleScale;
        lr.GetComponent<LineRenderer>().endWidth *= puzzleScale;
        transform.localScale = scale;
        laserOriginCoordinate = GetComponent<TileUpdater>().cubeCoordinate;

        //Initialize the emitter.
      
        //puzzleScale = transform.root.lossyScale.x;
        SetLaserPigment(manager.GetLaserPigment(laserColor)/2);
    
	}
	
	// Update is called once per frame
	void Update () {

        if (manager.editor)
        {
            SetLaserPigment(manager.GetLaserPigment(laserColor));
        }

        if(drawing)
        {
            FollowController();
        }

        if(linePositions.Count > 0)
        {
            UpdateList();
            CheckCompletions();
        }
       
      
    }


    //Colors: Red, Blue, Yellow, Pink,  Magenta, Green, Grey, Cyan, Brown, Purple, Orange
    void SetLaserPigment(Color color)
    {
        if (drawing || connected)
        {
            laserPigment = color;
            lr.material.color = laserPigment;
        }
        else
        {
            laserPigment = color / 2f;
            lr.material.color = laserPigment;
        }
        transform.FindChild("Emitter").GetChild(0).GetComponent<Renderer>().material.color = laserPigment;
        //transform.FindChild("Emitter").GetChild(1).GetComponent<Renderer>().material.color = laserPigment;

    }

    //Call this method to start drawing.
    public void StartDraw(GameObject controller)
    {
        simulatedController = controller;
        drawing = true;
        DisconnectEmitter();
        linePositions.Clear();
        linePositions.Add((transform.FindChild("ConnectionNode").gameObject));
        linePositions.Add(new GameObject());
        SetLaserPigment(manager.GetLaserPigment(laserColor));
        Instantiate(indicator, endReceiver.transform.position, endReceiver.transform.rotation);

    }

    //Call this method to end drawing.
    public void EndDraw()
    {
        linePositions.Remove(simulatedController);
        drawing = false;
        simulatedController.GetComponent<DrawFromController>().emitter = null;
        SetLaserPigment(manager.GetLaserPigment(laserColor));
    }

    //Check for completions
    public void CheckCompletions()
    {
        foreach(GameObject obj in connectedObjects)
        {
            //Debug.Log(node.transform.parent.name);
            if (obj.transform.GetComponent<ConnectedInfo>() != null)
            {
                //Debug.Log("Went through");
                connected = true;
                obj.transform.GetComponent<ConnectedInfo>().receivedLaserColor = laserColor;            
                obj.transform.GetComponent<ConnectedInfo>().receivedRGBColor = laserPigment;
                obj.transform.GetComponent<ConnectedInfo>().received = true;

            }
           
        }
    }
    
    //Update the list in the lineRenderer's positions to match.
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
                lrPositions[i] += node.transform.up * heightFactor * puzzleScale;

                //If this is the last real drawn node, move the draw node to there.
                if (i == linePositions.Count - 2)
                {
                    drawNode.transform.localPosition = lrPositions[i];
                }
            }
            // This should only happen when we're not drawing.
            if (i == linePositions.Count - 1 && !drawing)
            {
                lrPositions[i] += node.transform.up * heightFactor * puzzleScale;
            }

           

            lrPositions[i] = emitter.transform.InverseTransformPoint(lrPositions[i]);
           
            i++;
        }

        emitter.GetComponent<LineRenderer>().numPositions = lrPositions.Length;
        emitter.GetComponent<LineRenderer>().SetPositions(lrPositions);
    }

    //Disable the connection on all the objects in the list and clear it.
    public void DisconnectEmitter()
    {
        foreach (GameObject obj in connectedObjects)
        {
            if(obj.transform.parent.name.Contains("Receiver") || obj.transform.parent.name.Contains("Point"))
            {
                obj.transform.parent.GetComponent<ConnectedInfo>().received = false;
                //obj.transform.GetComponent<NodeFeedback>().react = true;
            }
            setConnection(obj, false);
        }
        connectedObjects.Clear();
    }

    //Follow the controller.
    public void FollowController()
    {
        linePositions[linePositions.Count - 1] = simulatedController;
    }

    //Remove a node
    public void RemoveLineNode(GameObject node)
    {
        connectedObjects.Remove(node);

        linePositions.Remove(node);

        setConnection(node, false);
    }

    //Changed to boolean for haptics: Adam Liang
    //Returns true if it connects, in order to help if it should pulse
    //Add a position to the line
    public bool AddLineNode(GameObject node)
    {
        //Only add it if it is a valid position.
        if (CheckViableNode(node))
        {
            //Add the obejct to the list.
            connectedObjects.Add(node);

            //Add a Vector. It doesn't matter what value it is since the top level of the list is always connectede to the controller.
            linePositions.Add(node);

            //Set the vector before as the target position.
            linePositions[linePositions.Count - 2] = node;
            setConnection(node, true);
            return true;
        }
        else return false;
    }

    //Set the connection node to bool
    public void setConnection(GameObject obj, bool connection)
    {
        if(obj.transform.parent.name == "EdgeConnections")
        {
            obj.transform.GetComponent<ConnectionNOde>().connected = connection;
            //obj.transform.GetComponent<EdgeNodeFeedback>().react = !connection;
        }
        else if (obj.transform.parent.name.Contains("Receiver") || obj.transform.parent.name.Contains("Emitter"))
        {
            obj.transform.parent.GetComponent<ConnectionNOde>().connected = connection;
            //No feedback to modify on receiver or emitter.
        }
        else
        { 
            obj.transform.parent.GetComponent<ConnectionNOde>().connected = connection;
            obj.transform.GetComponent<NodeFeedback>().react = !connection;
        }
         
    }

    //Check if connection node is connected
    public bool isConnected(GameObject obj)
    {
        if (obj.transform.parent.GetComponent<ConnectionNOde>().connected)
        {
            return true;
        }
        return false;
    }

    //Check if the position is invalid.
    //Case 0: Node has already been used.
    //Case 1: Something is between our last node and this one.
    //Case 2: Node is not directly next to the previous node.
    public bool CheckViableNode(GameObject node)
    {
        //Case 0: Already used by someone else
        if(node.transform.parent.GetComponent<ConnectionNOde>() != null)
        {
            if (node.transform.parent.GetComponent<ConnectionNOde>().connected)
            {
                return false;
            }
        }
        


        //Case 1: Node has already been used.
        for (int i = 0; i < linePositions.Count; i++)
        {
            if (node == linePositions[i])
            {
                return false;
            }
        }

        //Case 2: A wall/rubber is between us and the next node.
        GameObject lastObject = linePositions[linePositions.Count - 2];
        //Debug.Log(lastObject);
        Vector3 targetDirection = (lastObject.transform.position + lastObject.transform.up * heightFactor * puzzleScale) - (node.transform.position + node.transform.up * heightFactor * puzzleScale);
        Ray targetRay = new Ray(node.transform.position + node.transform.up * heightFactor * puzzleScale, targetDirection);
        //Debug.DrawRay(node.transform.position + node.transform.up * heightFactor * puzzleScale, targetDirection, Color.red, 10f);
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

        //Case 3: Node is not directly next to previous node.
        //The last position that we are comparing to.
        GameObject lastNode = linePositions[linePositions.Count - 2];
        if(lastNode != null)
        {
            //If we aren't compraing to an edgeconnection
            if (node.transform.parent.name != "EdgeConnections" && lastNode.transform.parent.name != "EdgeConnections")
            {
                //Compare as coordinates on a grid.
                //Check if it hsa retrieved its coordinate. If not, get it not.
                //Hack fix. If this node's parent is the emitter or receiver, we need to go to the parent.
                if (lastNode.transform.parent.name.Contains("Emitter") || lastNode.transform.parent.name.Contains("Receiver"))
                {
                    lastNode = lastNode.transform.parent.gameObject;
                }
                if (lastNode.GetComponent<ConnectionNOde>().coordinate == -Vector3.one)
                {
                    lastNode.GetComponent<ConnectionNOde>().RetrieveCoordinate();
                }
                Vector3 lastCoordinate = lastNode.GetComponent<ConnectionNOde>().coordinate;
                //Same for the other node.
                if (node.transform.parent.name.Contains("Emitter") || node.transform.parent.name.Contains("Receiver"))
                {
                    node = node.transform.parent.gameObject;
                }
                if (node.GetComponent<ConnectionNOde>().coordinate == -Vector3.one)
                {
                    node.GetComponent<ConnectionNOde>().RetrieveCoordinate();
                }
                Vector3 newCoordinate = node.GetComponent<ConnectionNOde>().coordinate;

                if (ViableConnection(lastCoordinate, newCoordinate))
                {
                    return true;
                }
               
            }//If one of them is an edge connection, check for a distance.
            else
            {
                Vector3 lastPosition = lastNode.transform.position;
                Vector3 newPosition = node.transform.position;

                if (Vector3.Distance(lastPosition, newPosition) < puzzleScale)
                {
                    return true;
                }
            }
        }
        return false;

        //Old case 2:
        //Previous node. Not the last node since that one is the cursor.
        /*
        Vector3 lastPosition = linePositions[linePositions.Count - 2].transform.position;
        Vector3 difference = lastPosition - node.transform.position;
        float distance = difference.magnitude;
        //distance = Mathf.Round(distance);
        
        if (distance < 0.95 * puzzleScale || distance > 1.05 * puzzleScale)
        {
            //If the node is an edge connection 
            if(node.transform.parent.name != connectedObjects[connectedObjects.Count - 2].transform.name && distance < puzzleScale)
            //if(distance > 0.45 * puzzleScale && distance < 0.55 * puzzleScale)
            {
                return true;
            }
           Debug.Log(distance + " " + difference);
           return false;
        }
        return true;*/

    }

    //Helper function to compare two coordinates to see if they can be connected
    public bool ViableConnection(Vector3 coord1, Vector3 coord2)
    {
        //Only return true if only one of the coordinates changes AND it only changes by one.
        Vector3 difference = coord1 - coord2;
        bool xOne = false, yOne = false, zOne = false;

        if(difference.x == 0 && difference.y == 0)
        {
            if(Mathf.Abs(difference.z) == 1)
            {
                xOne = true;
            }
        }
        else if(difference.x == 0 && difference.z == 0)
        {
            if(Mathf.Abs(difference.y) == 1)
            {
                yOne = true;
            }
        }
        else if(difference.y == 0 && difference.z == 0)
        {
            if(Mathf.Abs(difference.x) == 1)
            {
                zOne = true;
            }
        }
        
        if(xOne ^ yOne ^ zOne)
        {
            return true;
        }
        return false;
    }

}
