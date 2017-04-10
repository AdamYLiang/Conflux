using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Note: Modified and updated version of:
//          http://ericeastwood.com/blog/20/texture-tiling-based-on-object-sizescale-in-unity

//Only run in edit mode.
[ExecuteInEditMode]
public class Tiler : MonoBehaviour {

    //Get and store texture from renderer
    public Material mat;
    private Texture texture;

    //Our desired texture control. Change this to affect ______
    public float textureToMeshZ = 2f;

    //Just saves whether or not we have changed the corresponding public variable
    Vector3 prevScale = Vector3.one;

    //Just saves whether or not we have changed the public variable
    float prevTextureToMeshZ = -1f;

    //Square tiling
    public bool isSquare = false;

	// Use this for initialization
	void Start () {

		Debug.Log("Tiler is on, please turn off");

        mat = GetComponent<Renderer>().material;

        texture = mat.mainTexture;

        //Save our scale to the current scale
        if (isSquare)
        {
            //Only use one value to maintain a "cube" ratio
            prevScale = new Vector3(gameObject.transform.lossyScale.x, gameObject.transform.lossyScale.z, gameObject.transform.lossyScale.z);
            
        }
        else
        {
            prevScale = gameObject.transform.lossyScale;
        }

        //Set to public variables
        prevTextureToMeshZ = textureToMeshZ;

        UpdateTiling();
	}
	
	// Update is called once per frame
	void Update () {
		
        //If either our scale has changed or 
        if(gameObject.transform.lossyScale != prevScale || 
            !Mathf.Approximately(textureToMeshZ, prevTextureToMeshZ))
        {
            UpdateTiling();
        }

        //Save our scale to the current scale
        if (isSquare)
        {
            //Only use one value to maintain a "cube" ratio
            prevScale = new Vector3(gameObject.transform.lossyScale.x, gameObject.transform.lossyScale.z, gameObject.transform.lossyScale.z);
        }
        else
        {
            prevScale = gameObject.transform.lossyScale;
        }
        prevTextureToMeshZ = textureToMeshZ;
	}

    [ContextMenu("UpdateTiling")]
    void UpdateTiling()
    {
        //Standard plane is 10 x 10
        float planeSizeX = 10f;
        float planeSizeZ = 10f;

        //Calculate the texture-to-mesh width based on the ratio given by user-set texture-to-mesh height.
        float textureToMeshX = ((float)texture.width / texture.height) * textureToMeshZ;

        //Set the values.
        gameObject.GetComponent<Renderer>().material.mainTextureScale =
            new Vector2(planeSizeX * gameObject.transform.lossyScale.x / textureToMeshZ,   
                planeSizeZ * gameObject.transform.lossyScale.z / textureToMeshZ);
    }
}
