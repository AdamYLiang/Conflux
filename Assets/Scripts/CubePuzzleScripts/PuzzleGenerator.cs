using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PuzzleGenerator : MonoBehaviour {

    [SerializeField]
    public PuzzleEditor puzzleEditor = new PuzzleEditor();

    //Variables to change for generation
    public int emitterNumber, pointNumber, numberOfFaces;
	
    //Checks the numbers are valid
    bool CheckInputs()
    {
        if(emitterNumber < 0 || emitterNumber > 8)
        {
            Debug.Log("Invalid emitter number");
            return false;
        }
        else if( numberOfFaces < 0 || numberOfFaces > 6)
        {
            Debug.Log("Invalid number of faces.  1 - 6.");
            return false;
        }
        return true;
    }

    //Inits the puzzle to be completely empty.
    void InitPuzzle()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                puzzleEditor.northFace.faceBlocks[i].row[j] = 0;
                puzzleEditor.eastFace.faceBlocks[i].row[j] = 0;
                puzzleEditor.southFace.faceBlocks[i].row[j] = 0;
                puzzleEditor.westFace.faceBlocks[i].row[j] = 0;
                puzzleEditor.bottomFace.faceBlocks[i].row[j] = 0;
                puzzleEditor.topFace.faceBlocks[i].row[j] = 0;
            }
        }
    }

    //Simply returns a random face.
    FixRotation.Face ReturnRandomFace()
    {
        int i = Random.Range(0, 6);
        switch (i)
        {
            case 0: return FixRotation.Face.North;
            case 1: return FixRotation.Face.East;
            case 2: return FixRotation.Face.South;
            case 3: return FixRotation.Face.West;
            case 4: return FixRotation.Face.Bottom;
            case 5: return FixRotation.Face.Top;
            default: Debug.Log("Error, shouldn't get incorrect face.");
                return FixRotation.Face.All;
        }
    }

    //Checks to see if the given array already contains face.
    bool DoesNotContainFace (FixRotation.Face[] array, FixRotation.Face face)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i] == face)
            {
                return false;
            }
        }
        return true;
    }

    //Correctly selects random faces, always making sure they are adjacent to give space for the puzzle.
    FixRotation.Face[] ReturnPuzzleFaces(int numberOffaces)
    {
        //Pick faces for the puzzle. Ensure we don't pick the same one more than once.
        FixRotation.Face[] validFaces = new FixRotation.Face[numberOfFaces];

        //This is just all the faces.
        if (numberOfFaces == 6)
        {
            validFaces[0] = FixRotation.Face.North;
            validFaces[1] = FixRotation.Face.East;
            validFaces[2] = FixRotation.Face.South;
            validFaces[3] = FixRotation.Face.West;
            validFaces[4] = FixRotation.Face.Bottom;
            validFaces[5] = FixRotation.Face.Top;
        } //Impossible to pick three faces that don't intersect at some point.
        else if (numberOfFaces >= 3)
        {
            for (int i = 0; i < numberOfFaces; i++)
            {
                FixRotation.Face temp = ReturnRandomFace();
                if (DoesNotContainFace(validFaces, temp))
                {
                    validFaces[i] = temp;
                }

            }
        } // Must pick two faces right next to each other. 
        else if (numberOfFaces == 2)
        {
            validFaces[0] = ReturnRandomFace();
            //Incrementing by one for the cardinal directions is fine.
            if((int)validFaces[0] < 4)
            {
                validFaces[1] = validFaces[1]++;
            }
            else //For the top and bottom pick one of the four cardinal directions.
            {
                validFaces[1] = (FixRotation.Face)Random.Range(0, 3);
            }
           
        }
        else
        {
            validFaces[0] = ReturnRandomFace();
        }

        return validFaces;
    }

    //Randomly selects from the given array of faces.
    FixRotation.Face ReturnRandomValidFace(FixRotation.Face[] validFaces)
    {
        if (validFaces.Length > 1)
        {
            return validFaces[Random.Range(0, validFaces.Length)];
        }
        else return validFaces[0];
    }

    //Returns a random position on a face
    Vector2 ReturnRandomPosition()
    {
        return new Vector2(Random.Range(0, 5), Random.Range(0, 5));
    }

    //Select correct face
    FaceInfo ReturnCorrectFaceInfo(FixRotation.Face face)
    {
        int i = (int)face;
        switch (i)
        {
            case 0: return puzzleEditor.northFace;
            case 1: return puzzleEditor.eastFace;
            case 2: return puzzleEditor.southFace;
            case 3: return puzzleEditor.westFace;
            case 4: return puzzleEditor.bottomFace;
            case 5: return puzzleEditor.topFace;
            default: Debug.Log("Error, non valid face info.");
                return null;
        }
    }

    //Randomly place the emtiters
    void PlaceEmittersAndReceivers(LinePuzzleInfo[] linePuzzle)
    {
        //Generate a random position for the emitters and receivers.
        for (int i = 0; i < emitterNumber; i++)
        {
            //While the random position is invalid, keep generating a new place.
            bool emitterValid = false, receiverValid = false;
            while (!emitterValid)
            {
                //Pick random two numbers for its coordinate.
                Vector2 emitterPosition = ReturnRandomPosition();

                //Select the face values we need to modify
                FaceInfo face = ReturnCorrectFaceInfo(linePuzzle[i].emitterFace);

                //If the face coordinates is unoccupied, place the emitter there.
                if (face.faceBlocks[(int)emitterPosition.x].row[(int)emitterPosition.y] == 0)
                {
                    face.faceBlocks[(int)emitterPosition.x].row[(int)emitterPosition.y] = 2;
                    emitterValid = true;
                }
            }

            //While the random position is invalid, keep generating a new place.
            while (!receiverValid)
            {
                //Pick random two numbers for its coordinates.
                Vector3 receiverPosition = ReturnRandomPosition();

                //Select the face values we need to modify.
                FaceInfo face = ReturnCorrectFaceInfo(linePuzzle[i].receiverFace);

                //If the face coordinates is unoccupied place the receiver there.
                if (face.faceBlocks[(int)receiverPosition.x].row[(int)receiverPosition.y] == 0)
                {
                    face.faceBlocks[(int)receiverPosition.x].row[(int)receiverPosition.y] = 3;
                    receiverValid = true;
                }
            }

        }
    }

    //Tells us if the coordinate is a valid one.
    bool ValidCoordinateNumbers(Vector2 coordinate)
    {
        if (coordinate.x < 0 || coordinate.x > 4 || coordinate.y < 0 || coordinate.y > 4)
        {
            return false;
        }
        return true;
    }

    //Tells us if this is a valid face crossover
    bool IsFaceValid(FixRotation.Face face, FixRotation.Face[] validFaces)
    {
        for(int i = 0; i < validFaces.Length; i++)
        {
            if(validFaces[i] == face)
            {
                return true;
            }
        }
        return false;
    }

    //Gives us all the open nodes in the puzzle. in a Vector3, x is x, y is y, z is face.
    List<Vector3> AllOpenNodes(FixRotation.Face[] validFaces)
    {
        List<Vector3> validNodes = new List<Vector3>();

        for(int i = 0; i < validFaces.Length; i++)
        {
            FaceInfo face = ReturnCorrectFaceInfo(validFaces[i]);

            for(int j = 0; j < face.faceBlocks.Length; j++)
            {
                for(int k = 0; k < face.faceBlocks[j].row.Length; k++)
                {
                    //
                    if(face.faceBlocks[j].row[k] == 0)
                    {
                        //Adds the node to the list.
                        validNodes.Add(new Vector3(j, k, (int)validFaces[i]));
                    } 
                }
            }
        }

        return validNodes;

    }

    /*
    //Finds a path from point a to point b, using the coordinates given, then returns coordinates used.
    Vector2[] */

    // Generate a random puzzle
    public void GeneratePuzzle()
    {
        //Check numbers are valid, then proceed.
        if (CheckInputs())
        {
            //Get faces for this puzzle
            FixRotation.Face[] validFaces = ReturnPuzzleFaces(numberOfFaces);

            //Generate a face for each puzzle, the emitter and receiver
            LinePuzzleInfo[] linePuzzle = new LinePuzzleInfo[emitterNumber];
            for (int i = 0; i < emitterNumber; i++)
            {
                linePuzzle[i] = new LinePuzzleInfo();
                linePuzzle[i].emitterFace = ReturnRandomValidFace(validFaces);
                linePuzzle[i].receiverFace = ReturnRandomValidFace(validFaces);
            }

            //Init the puzzle to empty.
            InitPuzzle();

            //Place the emitters and receivers
            PlaceEmittersAndReceivers(linePuzzle);

            //For each emitter, calculate the most obvious path.
            for(int i = 0; i < linePuzzle.Length; i++)
            {
                //Keep finding a path until it is valid.
                bool validPath = false;
            }


        }
    }

    public class LinePuzzleInfo
    {
        public FixRotation.Face emitterFace;
        public FixRotation.Face receiverFace;
        public int numberOfPoints;

        public LinePuzzleInfo()
        {
            
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(PuzzleGenerator))]
    public class PuzzleGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PuzzleGenerator script = (PuzzleGenerator)target;

            if (GUILayout.Button("Generate Puzzle"))
            {
                script.GeneratePuzzle();
            }
        }
    }
    #endif

}
