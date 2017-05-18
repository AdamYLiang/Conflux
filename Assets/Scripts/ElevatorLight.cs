using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLight : MonoBehaviour {

	public bool lightStatus = false; //true is on, false is off
	public float lightSpeed = 2f;

	void Start () {
		lightStatus = false;
	}

	//Universal toggle, is outdated
	public void ToggleEleLight(){
		StartCoroutine(LightControl(lightStatus));
		lightStatus =! lightStatus;
	}

	//Toggles on
	public void ToggleEleLightOn(){
		StartCoroutine(LightControl(false));
	}

	//Toggles off light
	public void ToggleEleLightOff(){
		StartCoroutine(LightControl(true));
	}

	IEnumerator LightControl(bool lightOn){
		if(!lightOn){
			for(float i = 0; i <= 7; i+=0.1f){
				//gameObject.GetComponent<Light>().intensity += i;
				gameObject.GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, 5f, lightSpeed * Time.deltaTime);
				yield return null;
			}
		}

		else if (lightOn){
			for(float i = 0; i <= 7; i +=0.1f){
				//gameObject.GetComponent<Light>().intensity -= i;
				gameObject.GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, 0f, lightSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}
}
