using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderController : MonoBehaviour {

    //Camera object that we are attached to.
    Camera cam;

    //Shader to apply to the camera
    public Shader shader;

	// Use this for initialization
	void Start () {
        //Retrieves camera component
        cam = transform.GetComponent<Camera>();


	}
	
	// Update is called once per frame
	void Update () {

        cam.SetReplacementShader(shader, "StandardShader");

        
    }
}
