using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WallSpawner : MonoBehaviour {

    public GameObject wallTile;
    public List<GameObject> Tiles;

    public float height, length;

	// Use this for initialization
	void Start () {


        Initialize();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize()
    {
        //Change the base wall's scale
        Transform baseWall = transform.FindChild("BaseWall");

        baseWall.localScale = new Vector3(length, height, 1);
        baseWall.localPosition = new Vector3(length / 2 - 0.5f, height / 2 - 0.5f, 0.5f);

        //Find the transform to place them in
        Transform playTiles = transform.FindChild("PlayTiles");

        //Spawn objects in a grid.
        Vector3 location = Vector3.zero;

        for(int i = 0; i < height; i++)
        {
            location.y = i;

            for(int j = 0; j < length; j++)
            {
                location.x = j;
                Transform temp = Instantiate(Tiles[0], playTiles).transform;
                temp.localPosition = location;
                temp.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            }
        }
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(WallSpawner))]
public class WallSpawnerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WallSpawner script = (WallSpawner)target;

       


    }
}
#endif



