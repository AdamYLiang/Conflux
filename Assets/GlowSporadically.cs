using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowSporadically : MonoBehaviour {


    float intervalLength;
    float emission = 1;
    bool isForward = true;
    public Color baseColor = Color.gray;

    //Time it takes to go from bright to dark in seconds.
    float transitionTime = 1f;

	// Use this for initialization
	void Start () {
        transitionTime = Random.Range(0.1f, 5f);
	}
	
	// Update is called once per frame
	void Update () {

        emission = Mathf.PingPong(Time.time/transitionTime, 4) + 1;

        GetComponent<Renderer>().material.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));

	}
}
