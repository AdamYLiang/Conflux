using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUpdater : MonoBehaviour {

    //Reference to manager
    public PuzzleManager puzzleManager;

    //A reference to the main puzzle editor
    public CubeSpawner puzzleEditor;

    //A copy of the list of possible tiles.
    List<GameObject> tiles;

    //The tile type index on the list.
    public int tileType;

    //Coordinate on the cube.
    public Vector3 cubeCoordinate;

    //Our original type, If changed must replace.
    private GameObject originalTileType;
    
    //Number of walls we want on this tile.
    public FaceInfo.WallNumber wallNumber = FaceInfo.WallNumber.Zero;

	// Use this for initialization
	void Start () {
        puzzleManager = transform.root.GetComponent<PuzzleManager>();
        if (puzzleManager.editor)
        {
            puzzleEditor = transform.root.GetComponent<CubeSpawner>();
            tiles = puzzleEditor.Tiles;
            originalTileType = tiles[tileType];
        }
       
	}
	
	// Update is called once per frame
	void Update () {
		
        if(puzzleManager.editor)
        {
            //If we have changed the tile type
            if (tiles[tileType] != originalTileType)
            {
                Replace();
            }
        }
	}

    //Replace ourselves with the correct tile type.
    public void Replace()
    {
        GameObject temp = Instantiate(tiles[tileType], transform.position, transform.rotation, transform.parent);
        temp.GetComponent<TileUpdater>().CopyValues(this);
        temp.GetComponent<FixRotation>().face = GetComponent<FixRotation>().face;
        temp.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(gameObject);
    }

    //Copy the values onto the new object when we spawn it.
    public void CopyValues(TileUpdater tile)
    {
        wallNumber = tile.wallNumber;
        cubeCoordinate = tile.cubeCoordinate;
    }

}
