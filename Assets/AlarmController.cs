using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmController : MonoBehaviour {

    //Time it takes to fade from one color to another
    public float fadeTime = 1f;

    //Time it takes for the alarm to play itself again
    public float alarmInterval = 60f;
    //Time we spend playing the alarm
    public float timePlaying = 10f;

    //The color when we are not on
    public Color offColor;
    //The color when we are on
    public Color onColor;
    //The material we will be manipulating
    protected Material ourMaterial;

    //The audiosource that we will be playing
    public AudioSource audio;
    //Whether or not we should be playing the audio
    protected bool playing;

    //Timer to count in between each play of the alarm.
    protected float alarmTimer = 0f;
    //Timer to count how long the alarm has been playing.
    protected float playTimer = 0f;
    //Timer to count our lerp steps
    protected float colorStep = 0;
    //Bool to see which direction we are fading. False means lighting up, true means dimming down.
    protected bool fading = false;

    public bool noGlow = true;
    //The light that we are manipulating
    public Light alarmLight;
    public float maxLightRange = 10f;
    //protected float lightRangeStep = 0; unnecessary because we can just use the color step.


	// Use this for initialization
	void Start () {
        ourMaterial = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

        //If we aren't playing currently
        if (!playing)
        {
            AlarmColorFade();
            alarmTimer = Mathf.Clamp01(alarmTimer + Time.deltaTime / alarmInterval);
            if (alarmTimer == 1)
            {
                playing = true;
                playTimer = 0;
                audio.Play();
            }
        }
        else if (playing) // If we are playing currently
        {
            AlarmColorAnimate();
            playTimer = Mathf.Clamp01(playTimer + Time.deltaTime / timePlaying);
            if(playTimer == 1)
            {
                playing = false;
                alarmTimer = 0;
            }
        }
        if (!noGlow)
        {
            alarmLight.range = maxLightRange * colorStep;
        }


    }

    //Helper function to fade and maintain faded alarm
    void AlarmColorFade()
    {
        ourMaterial.color = Color.Lerp(offColor, onColor, colorStep);
        colorStep = Mathf.Clamp01(colorStep - Time.deltaTime / fadeTime);
 
     
    }

    //Helper function to "animate" the alarm
    void AlarmColorAnimate()
    {
        //If we are lighting up
        if (!fading)
        {
            ourMaterial.color = Color.Lerp(offColor, onColor, colorStep);
            colorStep = Mathf.Clamp01(colorStep + Time.deltaTime / fadeTime);
            if(colorStep >= 1)
            {
                fading = true;
            }
        }
        else //If we are dimming down
        {
            ourMaterial.color = Color.Lerp(offColor, onColor, colorStep);
            colorStep = Mathf.Clamp01(colorStep - Time.deltaTime / fadeTime);
            if (colorStep <= 0)
            {
                fading = false;
            }
        }
    }
}
