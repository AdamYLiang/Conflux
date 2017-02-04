using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterScript : MonoBehaviour {

    LineRenderer lr;

    public enum LaserColor { Red, Blue, Yellow, Pink, Magenta, Green, Grey, Cyan, Brown, Purple, Orange};

    public LaserColor laserColor = LaserColor.Blue;
    private Color laserPigment;
    float verticalAdjust = 0.5f;

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

	// Use this for initialization
	void Start () {
        manager = transform.root.GetComponent<PuzzleManager>();
        lr = transform.FindChild("LaserRenderer").GetComponent<LineRenderer>();
        lr.GetComponent<LineRenderer>().startWidth *= puzzleScale;
        lr.GetComponent<LineRenderer>().endWidth *= puzzleScale;

        //Find our coordinate by checking our name and converting it to a Vector3
        char[] letters = transform.name.ToCharArray();
        string x = "" + letters[1];
        string y = "" + letters[4];
        string z = "" + letters[7];
        //Debug.Log(x + ", " + y + ", " + z);
        laserOriginCoordinate = new Vector3(int.Parse(x), int.Parse(y), int.Parse(z));
        //Debug.Log(laserOriginCoordinate);

        //Initialize the emitter.
      
        puzzleScale = transform.root.lossyScale.x;
        SetLaserPigment(manager.GetLaserPigment(laserColor)/4);
    
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
            transform.FindChild("StandinModel").GetComponent<Renderer>().material.color = laserPigment;
        }
        else
        {
            laserPigment = color/4;
            lr.material.color = laserPigment;
            transform.FindChild("StandinModel").GetComponent<Renderer>().material.color = laserPigment;
        }
     
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
        //SetLaserPigment(manager.GetLaserPigment(laserColor));

    }

    //Call this method to end drawing.
    public void EndDraw()
    {
        linePositions.Remove(simulatedController);
        drawing = false;
        //SetLaserPigment(manager.GetLaserPigment(laserColor) / 4);
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
            obj.GetComponent<ConnectedInfo>().received = false;
        }
        connectedObjects.Clear();
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
            //If it is the type of component we need to keep track of, add it to the list.
            if(node.transform.parent.tag == "Tracked")
            {
                connectedObjects.Add(node.transform.parent.gameObject);
            }

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

        //Case 2: Node is not directly next to previous node.
        //Previous node. Not the last node since that one is the cursor.
        Vector3 lastPosition = linePositions[linePositions.Count - 2].transform.position;
        Vector3 difference = lastPosition - node.transform.position;
        float distance = difference.magnitude;
        //distance = Mathf.Round(distance);

        if (distance < 0.95 * puzzleScale || distance > 1.05 * puzzleScale)
        {
            //Vertex node
            if(distance > 0.45 * puzzleScale && distance < 0.55 * puzzleScale)
            {
                return true;
            }
           Debug.Log(distance + " " + difference);
           return false;
        }
        return true;
    }

}
