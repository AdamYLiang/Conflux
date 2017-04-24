using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHexLightsController : MonoBehaviour {

    public bool[] lightsOn;

	// Use this for initialization
	void Start () {
        lightsOn = new bool[transform.childCount];
	}
	
	// Update is called once per frame
	void Update () {
        CheckAllBools();
	}

    void CheckAllBools()
    {
        for(int i = 0; i < lightsOn.Length; i++)
        {
            if (lightsOn[i])
            {
                transform.GetChild(i).GetComponent<HexLightController>().isOn = true;
            }
            else
            {
                transform.GetChild(i).GetComponent<HexLightController>().isOn = false;
            }
        }
    }

    public void TurnOn(int index)
    {
        lightsOn[index] = true;
    }

    public void TurnOff(int index)
    {
        lightsOn[index] = false;
    }
}
