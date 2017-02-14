using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUpdater : MonoBehaviour {

    public CubeSpawner puzzleEditor;

    List<GameObject> tiles;

    public int tileType;
    private GameObject originalTileType;
    
    public FaceInfo.WallNumber wallNumber = FaceInfo.WallNumber.Zero;

	// Use this for initialization
	void Start () {
        puzzleEditor = transform.root.GetComponent<CubeSpawner>();
        tiles = puzzleEditor.Tiles;
        originalTileType = tiles[tileType];
	}
	
	// Update is called once per frame
	void Update () {
		
        //If we have changed the tile type
        if(tiles[tileType] != originalTileType)
        {
            Replace();
        }

	}

    //Replace ourselves with the correct tile type.
    public void Replace()
    {
        GameObject temp = Instantiate(tiles[tileType], transform.position, transform.rotation, transform.parent);

        Destroy(gameObject);
    }

    //Copy the values onto the new object when we spawn it.
    public void CopyValues(TileUpdater tile)
    {
        tiles = tile.tiles;
        wallNumber = tile.wallNumber;
    }

}
