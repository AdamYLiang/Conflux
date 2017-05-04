using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SocketLogic : MonoBehaviour {

	public UnityEvent socketed = new UnityEvent();
	public float timer;
	public bool shouldOpen;

	// Use this for initialization
	void Start () {
		timer = 2f;
		shouldOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Currently set to a delay of 2 seconds, if the puzzle is inside it then it will close door and deactivate script
	void OnTriggerStay(Collider col){
		if(col.gameObject.tag == "CubePuzzle"){
			timer -= Time.deltaTime;
			if(timer <= 0){
				socketed.Invoke();
				this.enabled = false;
			}
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "CubePuzzle"){
			timer = 2f;
		}
	}
}
