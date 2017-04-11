using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexLightController : MonoBehaviour {

    public bool isOn = false;
    protected bool prevOn;

    Color originalEmissionColor;

    public float lightRange = 1;

    protected Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
        prevOn = isOn;

        originalEmissionColor = mat.GetColor("_EmissionColor");

	}
	
	// Update is called once per frame
	void Update () {

        if (isOn)
        {
            if(!prevOn)
            {
                prevOn = true;
                StartCoroutine(TurnOn());
            }
        }
        else
        {
            if (prevOn)
            {
                prevOn = false;
                StartCoroutine(TurnOff());
            }
        }
       
	}

    public IEnumerator TurnOn()
    {
        float step = 0f;
       
        while(mat.GetColor("_EmissionColor").grayscale < originalEmissionColor.grayscale * lightRange)
        {
            float factor = Mathf.Lerp(0, lightRange, step);
            mat.SetColor("_EmissionColor", originalEmissionColor * factor);
            //mat.SetFloat("EmissionScaleUI", Mathf.Lerp(0, lightRange, step));
            step += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator TurnOff()
    {
        float step = 0f;

        while(mat.GetColor("_EmissionColor").grayscale > 0)
        {
            float factor = Mathf.Lerp(lightRange, 0, step);
            mat.SetColor("_EmissionColor", originalEmissionColor * factor);
            step += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
