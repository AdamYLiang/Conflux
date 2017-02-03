using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CubeSpawner : MonoBehaviour {

    public List<GameObject> Tiles;

    public GameObject wallTile;

    [SerializeField]
    public FaceInfo northFace = new FaceInfo(), 
        eastFace = new FaceInfo(), southFace = new FaceInfo(), 
        westFace = new FaceInfo(), bottomFace = new FaceInfo(), 
        topFace = new FaceInfo();

    [System.NonSerialized]
    public FaceInfo[] faceBlocks;

    public FixRotation.Face selectedFace;

	// Use this for initialization
	void Start () {
        faceBlocks = new FaceInfo[6];
        faceBlocks[0] = northFace; faceBlocks[1] = eastFace; faceBlocks[2] = southFace; faceBlocks[3] = westFace;
        faceBlocks[4] = bottomFace; faceBlocks[5] = topFace;
        retrieveCube(new Vector3(0, 0, 0));
        PlaceCubes();
	}
	
	// Update is called once per frame
	void Update () {

        //Using space bar to reload the cube.
        if (Input.GetKeyDown(KeyCode.Space))
        {      
            RefreshCube();
        }
	}

    // Flip the coordinate, i.e. 0 -> 4, 1 -> 3.
    public int FlipCoordinate(int x)
    {
        return 4 - x;
    }

    //Translates the coordinate to all the array indexes and faces associated with it. (i.e. 0,0,0, has three faces, and needs three coordinates)
    public Vector3[] TranslateEdgeCoordinateToIndexes(Vector3 coordinate)
    {
        Vector3[] coordinates;

        bool xEdge = false, yEdge = false, zEdge = false;
        //Booleans for each direction
        bool n, e, s, w, t, b;

        //Detect if it is a corner or an edge.
        xEdge = (coordinate.x == 0 || coordinate.x == 4);
        yEdge = (coordinate.y == 0 || coordinate.y == 4);
        zEdge = (coordinate.z == 0 || coordinate.z == 4);

        e = coordinate.x == 0;
        w = coordinate.x == 4;
        b = coordinate.y == 0;
        t = coordinate.y == 4;
        n = coordinate.z == 0;
        s = coordinate.z == 4;

        //Count the bools to know if corner or edge
        int edgeAxis = 0;
        if (xEdge)
        {
            edgeAxis++;
        }
        if (yEdge)
        {
            edgeAxis++;
        }
        if (zEdge)
        {
            edgeAxis++;
        }

        //Array size is dependent on array axis.
        coordinates = new Vector3[edgeAxis];

        int index = 0;
        //Can't have opposite sides so else if. Index increased everytime to be the appropriate one.
        if (n)
        {
            coordinates[index] = new Vector3(0, 0, 0);
            index++;
        }
        else if (s)
        {
            coordinates[index] = new Vector3(2, 0, 0);
            index++;
        }

        if (e)
        {
            coordinates[index] = new Vector3(1, 0, 0);
            index++;
        }
        else if (w)
        {
            coordinates[index] = new Vector3(3, 0, 0);
            index++;
        }

        if (t)
        {
            coordinates[index] = new Vector3(5, 0, 0);
            index++;
        }
        else if (b)
        {
            coordinates[index] = new Vector3(4, 0, 0);
            index++;
        }
        
        for(int i = 0; i < edgeAxis; i++)
        {
            coordinates[i] = TranslateEdgeCoordinateToArrayIndex((int)coordinates[i].x, coordinate);
        }
        return coordinates;
    }

    //Translates the specific edgeCoordinate ot the specific array index for that one coordinate. Is used by TranslateEdgeCoordinateToIndexes().
    public Vector3 TranslateEdgeCoordinateToArrayIndex(int face, Vector3 coordinate)
    {
        Vector3 returnVector = -Vector3.one;
        returnVector.x = face;

        //NOTE: returnVector x is the face, y is the x coordinate, and z is the y coordinate.

        switch (face)
        {
            //North face
            case 0:
                returnVector.y = coordinate.x;
                returnVector.z = coordinate.y;
                break;
            //East face
            case 1:
                returnVector.y = FlipCoordinate((int)coordinate.z);
                returnVector.z = coordinate.y;
                break;
            //South face
            case 2:
                returnVector.y = FlipCoordinate((int)coordinate.x);
                returnVector.z = coordinate.y;
                break;
            //West face
            case 3:
                returnVector.y = coordinate.z;
                returnVector.z = coordinate.y;
                break;
            //Bottom face
            case 4:
                returnVector.y = coordinate.x; //FlipCoordinate((int)coordinate.x);
                returnVector.z = FlipCoordinate((int)coordinate.z);
                break;
            //Top face
            case 5:
                returnVector.y = coordinate.x;
                returnVector.z = coordinate.z;
                break;
            default:
                Debug.Log("Something bad happened");
                break;


        }
        return returnVector;
    }

    //Take a coordinate and figure out which index it is in the array. Face coordinates only.
    // 0: North, 1: East, 2: South, 3: West, 4: Bottom, 5: Top
    //Returns (-1,-1,-1) if it is a corner piece.
    public Vector3 TranslateFaceCoordinateToArrayIndex(Vector3 coordinate)
    {
        //This Vector will return as: x = arrayIndex, y = x value, z = y value.
        Vector3 returnVector = Vector3.zero;

        //Figure out the face. This is the X in the return vector.
        
        if((coordinate.z == 0 && coordinate.x == 0) || (coordinate.z == 0 && coordinate.y == 0) || (coordinate.x == 0 && coordinate.y == 0)
            || (coordinate.z == 4 && coordinate.x == 4) || (coordinate.z == 4 && coordinate.y == 4) || (coordinate.x == 4 && coordinate.y == 4)
            || (coordinate.z == 0 && coordinate.x == 4) || (coordinate.z == 0 && coordinate.y == 4) || (coordinate.x == 0 && coordinate.y == 4)
            || (coordinate.z == 4 && coordinate.x == 0) || (coordinate.z == 4 && coordinate.y == 0) || (coordinate.x == 4 && coordinate.y == 0))
        {
            returnVector = -Vector3.one;
        }//If the z is 0, this is the north face.
        else if(coordinate.z == 0)
        {
            returnVector.x = 0;
        } // If the x is 0, this is the east face.
        else if (coordinate.x == 0)
        {
            returnVector.x = 1;
        } // If the z is 4, this is the south face.
        else if (coordinate.z == 4)
        {
            returnVector.x = 2;
        }// If the x is 4, this is the west face.
        else if (coordinate.x == 4)
        {
            returnVector.x = 3;
        }  // If the y is 0, this is the bottom face.
        else if (coordinate.y == 0)
        {
            returnVector.x = 4;
        } // If the y is 4, this is the top face.
        else if (coordinate.y == 4)
        {
            returnVector.x = 5;
        }

        //If we are the north face, then we can take it as coordinate x and y with no change.
        if(returnVector.x == 0)
        {
            returnVector.y = coordinate.x;
            returnVector.z = coordinate.y;
        } // If we are the east face, then we must use the flipped z value as our x, and the y value as y.
        else if (returnVector.x == 1)
        {
            returnVector.y = FlipCoordinate((int)coordinate.z);
            returnVector.z = coordinate.y;
        } // If we are the south face, then we need to flip our x values, and keep the y value.
        else if (returnVector.x == 2)
        {
            returnVector.y = FlipCoordinate((int)coordinate.x);
            returnVector.z = coordinate.y;
        }// If we are the west face, then we must use the z value as our x, and the y value as y.
        else if (returnVector.x == 3)
        {
            returnVector.y = coordinate.z;
            returnVector.z = coordinate.y;
        }  // If we are the bottom face, then we flip the x for our x values, and flip the z for our y values.
        else if (returnVector.x == 4)
        {
            returnVector.y = (int)coordinate.x;
            returnVector.z = FlipCoordinate((int)coordinate.z);
        } // If we are the top face, then we use x as x and z as y.
        else if (returnVector.x == 5)
        {
            returnVector.y = coordinate.x;
            returnVector.z = coordinate.z;
        }
        return returnVector;
    }

    //Loads tiles on every face
    public void PlaceCubes()
    {
        //Iterate through every coordinate in our cube.
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    //The coordinate of this run through the for loop.
                    Vector3 coordinate = new Vector3(i, j, k);

                    //Use the helper function to get the gameobject of that coordinate.
                    GameObject cube = retrieveCube(coordinate);
                    
                    //As long as we got a proper cube...
                    if(cube != null)
                    {
                        //Get the array indexes for the coordinate using hte helper function, set it to the array.
                        Vector3 info = TranslateFaceCoordinateToArrayIndex(coordinate);
                        Vector3[] coordinates = { info };

                        //If we got a negative coordinate, it is an edge/corner coordinate and must be handled accordingly.
                        if (info.Equals(-Vector3.one))
                        {
                            coordinates = TranslateEdgeCoordinateToIndexes(coordinate); 
                        }
                        //Will only need to do once if not an edge coordinate.
                        for (int m = 0; m < coordinates.Length; m++)
                        {
                            //This is the coordinate of the block. If it is an edge we have more than one face to worry about thus the for loop.
                            info = coordinates[m];

                            //Get the tile number we need to spawn.
                            int value = faceBlocks[(int)info.x].faceBlocks[(int)info.z].row[(int)info.y];
                            //If the value isn't a valid prefab, don't do anything and tell the user.
                            if (value >= 0 && value < Tiles.Count)
                            {
                                //Get the proper rotation depending on the face using the function.
                                Quaternion rotation = Quaternion.Euler(CalculateRotation((int)info.x));

                                //Get the proper position using the function.
                                Vector3 position = CalculateRelativePosition((int)info.x);

                                //Spawn the object and then use the proper modifications in the function.
                                GameObject temp = Instantiate(Tiles[value], cube.transform.position + position, rotation);
                                temp.transform.name = "(" + i + ", " + j + ", " + k + ") " + temp.transform.name;
                                ModifyObject(temp, info, 0, rotation);
                            }
                            else
                            {
                                Debug.Log("Attempted to spawn a tile not in the prefab list.");
                            }
                            
                            //Find out how many walls we need to spawn here, then call the necessary function.
                            FaceInfo.WallNumber blocks = faceBlocks[(int)info.x].faceBlocks[(int)info.z].blocked[(int)info.y];
                            SpawnWallsOnCoordinate(blocks, info, cube);
                        }
                    }  
                }
            }
        }
    }

    //Standard spawn walls function
    public void SpawnWallsOnCoordinate(FaceInfo.WallNumber blocks, Vector3 info, GameObject cube)
    {
        if (blocks > 0)
        {
            //Debug.Log("Number of walls:" + blocks);
            //Rotation fix is to differentiate the different blocks.
            float rotationFix = 0f;
            rotationFix = RectifyRotationFix(info);
            for (int q = 0; q < (int)blocks; q++)
            {
                Quaternion rotation = Quaternion.Euler(CalculateRotation((int)info.x));
                Vector3 position = CalculateRelativePosition((int)info.x);
                GameObject temp = Instantiate(wallTile, cube.transform.position + position, rotation);
                ModifyObject(temp, info, rotationFix, rotation);
                temp.transform.GetChild(0).GetComponent<WallBlockRotation>().enabled = false;
                rotationFix += 90f;
            }

        }
    }

    //Standard spawn modifications
    public void ModifyObject(GameObject temp, Vector3 info, float rotationFix, Quaternion rotation)
    {
        temp.transform.parent = transform.FindChild("PlayTiles");
        temp.GetComponent<FixRotation>().face = (FixRotation.Face)info.x;
        temp.GetComponent<FixRotation>().rotationFix = rotationFix;
        temp.GetComponent<FixRotation>().UseRotation(rotation.eulerAngles);
        temp.GetComponent<FixRotation>().originalRotation = rotation.eulerAngles;
    }

    //Returns a float to use as the rotation fix depending on whether or not it is an edge/vertex coordinate.
    public float RectifyRotationFix(Vector3 info)
    {
        float rotationFix = 0f;
        if (info.z == 4 && info.y == 0)
        {
            rotationFix = 90f;
        }
        else if (info.z == 0 && info.y == 4)
        {
            rotationFix = -90f;
        }
        else if (info.z == 0)
        {
            rotationFix = 0f;
        }
        else if (info.z == 4)
        {
            rotationFix = 180f;
        }
        else if (info.y == 0)
        {
            rotationFix = 90f;
        }
        else if (info.y == 4)
        {
            rotationFix = -90f;
        }
        return rotationFix;
    }

    //Gives the correct rotation for the tile depending on the face.
    public Vector3 CalculateRotation(int face)
    {
        switch (face)
        {
            //North face
            case 0:
                return new Vector3(0, 0, 90);
            //East face
            case 1:
                return new Vector3(0, 90, 90);
            //South face
            case 2:
                return new Vector3(180, 0, -90);
            //West face
            case 3:
                return new Vector3(-180, 90, -90);
            //Bottom face
            case 4:
                return new Vector3(0, 0, 180);
            //Top face
            case 5:
                return new Vector3(0, 0, 0);
        }

        return Vector3.zero;
    }

    //Returns the Vector3 that needs to be applied to the cube's transform to place it correctly.
    public Vector3 CalculateRelativePosition(int face)
    {
        //Switch statements for the face we are on
        float space = 0.5f;
        switch (face)
        {
            //North face
            case 0:
                return new Vector3(-space, 0, 0);
            //East face
            case 1:
                return new Vector3(0, 0, space);
            //South face
            case 2:
                return new Vector3(space, 0, 0);
            //West face
            case 3:
                return new Vector3(0, 0, -space);
            //Bottom face
            case 4:
                return new Vector3(0, -space, 0);
            //Top face
            case 5:
                return new Vector3(0, space, 0);
            default:
                Debug.Log("Something bad happened.");
                break;
        }


        return Vector3.zero;
    }

    //Removes and places the cubes.
    public void RefreshCube()
    {
        GetComponent<PuzzleManager>().EmptyList();
        RemoveTiles();
        PlaceCubes();
    }

    //Destroys all the tiles.
    public void RemoveTiles()
    {
        Transform tiles = transform.FindChild("PlayTiles");
        //Debug.Log(tiles.childCount);
        for(int i = 0; i < tiles.childCount; i++)
        {
            Destroy(tiles.GetChild(i).gameObject);
        }
    }

    //Removes the tiles on this face
    public void RemoveTilesOnFace(FixRotation.Face face)
    {
        Transform tiles = transform.FindChild("PlayTiles");
        for(int i = 0; i < tiles.childCount; i++)
        {
            if(tiles.GetChild(i).GetComponent<FixRotation>().face == face)
            {
                Destroy(tiles.GetChild(i).gameObject);
                
            }
        }
    }

    //Retrieve the cube using the coordinates.
    public GameObject retrieveCube(Vector3 coordinate)
    {
        int x = (int)coordinate.x; int y = (int) coordinate.y; int z = (int)coordinate.z;
        string coordinateString = "Tile (" + x + "," + y + "," + z + ")";
        //Debug.Log(coordinateString);
        Transform returnObject = transform.FindChild("Cubes").FindChild(coordinateString);
        //Debug.Log(returnObject);
        if(returnObject == null)
        {
            //Debug.Log("Something is missing in the scene.");
            //Null means such a coordinate doesnt exist on the surface of the cube. i.e. inner coordinates.
            return null;
        }
        else
        {
            //Debug.Log(returnObject);
            return returnObject.gameObject;
        }
        
    }

    //Reset the whole array, as in full cube.
    public void ResetArray()
    {
        ResetFaceBlocks(FixRotation.Face.North);
        ResetFaceBlocks(FixRotation.Face.East);
        ResetFaceBlocks(FixRotation.Face.West);
        ResetFaceBlocks(FixRotation.Face.South);
        ResetFaceBlocks(FixRotation.Face.Bottom);
        ResetFaceBlocks(FixRotation.Face.Top);
    }

    //Select Correct FaceBlocks variable
    public FaceInfo GetFaceBlocks(FixRotation.Face face)
    {
        FaceInfo faceBlocks;
        switch ((int)face)
        {
            case 0:
                faceBlocks = northFace;
                break;
            case 1:
                faceBlocks = eastFace;
                break;
            case 2:
                faceBlocks = southFace;
                break;
            case 3:
                faceBlocks = westFace;
                break;
            case 4:
                faceBlocks = bottomFace;
                break;
            case 5:
                faceBlocks = topFace;
                break;
            default:
                Debug.Log("Something bad happened in ResetFaceBlocks");
                faceBlocks = null;
                break;
        }
        return faceBlocks;
    }

    //Reset a specific array, as in face.
    public void ResetFaceBlocks(FixRotation.Face face)
    {
        FaceInfo faceBlocks = GetFaceBlocks(face);
        if(faceBlocks != null)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    faceBlocks.faceBlocks[i].row[j] = 0;
                    faceBlocks.faceBlocks[i].blocked[j] = FaceInfo.WallNumber.Zero;
                }
            }
        }
    }

    //Make walls on specific array, as in face.
    public void SpawnWalls(FixRotation.Face face)
    {
        FaceInfo faceBlocks = GetFaceBlocks(face);

        if(faceBlocks != null)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if((i == 0 || i == 4) && (j == 0 || j == 4))
                    {
                        faceBlocks.faceBlocks[i].blocked[j] = FaceInfo.WallNumber.Two;
                    }
                    else if ( i == 0 || i == 4 || j == 0 || j == 4)
                    {
                        faceBlocks.faceBlocks[i].blocked[j] = FaceInfo.WallNumber.One;
                    }         
                }
            }
        }
    }

    //Make walls on whole cube.
    public void SpawnWallsAllFaces()
    {
        SpawnWalls(FixRotation.Face.North);
        SpawnWalls(FixRotation.Face.East);
        SpawnWalls(FixRotation.Face.South);
        SpawnWalls(FixRotation.Face.West);
        SpawnWalls(FixRotation.Face.Bottom);
        SpawnWalls(FixRotation.Face.Top);

    }

    //Cover all faces in rubber.
    public void SpawnRubberAllFaces()
    {
        SpawnRubberOnFace(FixRotation.Face.North);
        SpawnRubberOnFace(FixRotation.Face.East);
        SpawnRubberOnFace(FixRotation.Face.West);
        SpawnRubberOnFace(FixRotation.Face.South);
        SpawnRubberOnFace(FixRotation.Face.Top);
        SpawnRubberOnFace(FixRotation.Face.Bottom);
    }

    //Cover a face in rubber.
    public void SpawnRubberOnFace(FixRotation.Face face)
    {
        FaceInfo faceBlocks = GetFaceBlocks(face);

        if (faceBlocks != null)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    faceBlocks.faceBlocks[i].row[j] = 3;
                }
            }
        }
    }

    //Save the object as a prefab, as well as any modifications to make it game ready.
    public void SaveAsPrefab(Transform transform)
    {
        //Workaround, make an empty prefab, replace it with our gameObject, then spawn it again, modify that, and replace it. 
        //We do this to avoid changing the one we are editing at the moment.
        Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Puzzles/tempPuzzle.prefab");
        PrefabUtility.ReplacePrefab(transform.gameObject, prefab);
        PrefabUtility.InstantiatePrefab(prefab);

        //Can't cast the instantiation so we can just find it again after spawning.
        GameObject spawnedPrefab = GameObject.Find("tempPuzzle");

        //Destroy the CubeSpawner script, since it's for editting, then destroy the placeholder cubes underlying the puzzle since they are now unnecessary.
        DestroyImmediate(spawnedPrefab.GetComponent<CubeSpawner>());
        DestroyImmediate(spawnedPrefab.transform.FindChild("Cubes").gameObject);
        EmitterScript[] emitters = spawnedPrefab.transform.GetComponentsInChildren<EmitterScript>();
        for(int i = 0; i < emitters.Length; i++)
        {
            emitters[i].linePositions.Clear();
        }
        spawnedPrefab.GetComponent<PuzzleManager>().receiverCompletion.Clear();
        FixRotation[] rotationFixers = spawnedPrefab.transform.GetComponentsInChildren<FixRotation>();
        for(int i = 0; i < rotationFixers.Length; i++)
        {
            DestroyImmediate(rotationFixers[i]);
        }

        //Replace the prefab with the modified one. Destroy the one that exists in the scene.
        PrefabUtility.ReplacePrefab(spawnedPrefab, prefab);
        DestroyImmediate(spawnedPrefab);
    }
}



[System.Serializable]
public class FaceInfo{

    //Face blocks described as a five-by-five grid.
    [System.Serializable]
    public enum WallNumber { Zero, One, Two, Three, Four};

    [System.Serializable]
    public class rows
    {
        public int[] row;
        public WallNumber[] blocked;
    }

    public rows[] faceBlocks;

    public FaceInfo()
    {
        faceBlocks = new rows[5];

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CubeSpawner))]
public class CubeSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CubeSpawner script = (CubeSpawner)target;

        if (GUILayout.Button("Make Walls on Selected Face"))
        {
            script.SpawnWalls(script.selectedFace);
        }

        if (GUILayout.Button("Make Walls on all faces."))
        {
            script.SpawnWallsAllFaces();
        }

        if (GUILayout.Button("Make Rubber on Selected Face"))
        {
            script.SpawnRubberOnFace(script.selectedFace);
        }

        if (GUILayout.Button("Make Rubber on all faces."))
        {
            script.SpawnRubberAllFaces();
        }
 
        if (GUILayout.Button("Reset Selected Face"))
        {
            script.ResetFaceBlocks(script.selectedFace);
        }

        if (GUILayout.Button("Reset Whole Cube"))
        {
            script.ResetArray();
        }

        if (GUILayout.Button("Export puzzle"))
        {
            script.SaveAsPrefab(script.transform);
        }


    }
}
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(FaceInfo))]
public class FaceInfoEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Write the prefix label
        //EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PrefixLabel(position, label);

        //Create a temp rect we will use to iterate throughout the data to determine its location
        Rect newRect = position;
        newRect.y += 18f;

        //Find the entire Multidimensional array
        SerializedProperty faceBlocks = property.FindPropertyRelative("faceBlocks");

        //Loop through array, always size five
        for (int i = 4; i >= 0; i--)
        {
            //Find the individual row. Set the width to the width/5
            SerializedProperty row = faceBlocks.GetArrayElementAtIndex(i).FindPropertyRelative("row");
            SerializedProperty blocked = faceBlocks.GetArrayElementAtIndex(i).FindPropertyRelative("blocked");

            //Divide by the number of slots we want. Doubled it for empty spaces between
            newRect.width = position.width / 10;

            //Set the size to five if it isnt already.
            if (row.arraySize != 5)
            {
                row.arraySize = 5;
            }

            if(blocked.arraySize != 5)
            {
                blocked.arraySize = 5;
            }

            //Iterate through the whole row
            for (int j = 0; j < 5; j++)
            {
                //Retrieve an individual element, and display it, then move slightly to the right and repeat. Multipled by two because we want gaps.
                //SerializedProperty value = ;
                newRect.height = 18f;
                EditorGUI.PropertyField(newRect, row.GetArrayElementAtIndex(j), GUIContent.none);
                newRect.x += newRect.width;
                EditorGUI.PropertyField(newRect, blocked.GetArrayElementAtIndex(j), GUIContent.none);
                newRect.x += newRect.width;
            }

            //Move vertically down for the next row, and reset the horizontal.
            newRect.x = position.x;
            newRect.y += 18f;
        }
       
        //EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18f * 7f;
    }
}
#endif
