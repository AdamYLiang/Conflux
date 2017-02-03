using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {


    public List<bool> receiverCompletion;
    public bool play = false;
    public bool finished = false;
    public bool editor = false;
    public bool hidden = false;
    public Color uncompleteColor, completeColor;
    public float colorChangeSpeed = 3f;

    private bool allActive = true;
    private Light glow;
    private bool changing = false, color1 = false, color2 = true;
    private float colorChangeTimer = 0f; 
    private Transform playTiles;
    private Color  targetColor;
    private MeshRenderer targetRenderer;

    void Start()
    {
        glow = transform.FindChild("Glow").GetComponent<Light>();
        glow.color = uncompleteColor;
        playTiles = transform.GetChild(0);
        for (int i = 0; i < playTiles.childCount; i++)
        {
            if (playTiles.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                targetRenderer = playTiles.GetChild(i).GetComponent<MeshRenderer>();
                i = playTiles.childCount;
            }
        }
        SetAllColor(uncompleteColor);
    }

	// Update is called once per frame
	void Update () {

        if (hidden)
        {
            HideCube();
        }
        else
        {
            ShowCube();

            if (play)
            {
                finished = CheckAllReceiver();
            }
            else if(!play && !editor)
            {
                 transform.Rotate(new Vector3(Random.Range(10, 20), Random.Range(10, 20), Random.Range(10, 20)) * Time.deltaTime);
            }

            if (finished && targetRenderer.material.color == uncompleteColor && !changing)
            {
                targetColor = completeColor;
                colorChangeTimer = 0;
                changing = true;
            }

            if (!finished && targetRenderer.material.color == completeColor && !changing)
            {
                targetColor = uncompleteColor;
                colorChangeTimer = 0;
                changing = true;
            }

            if (changing)
            {
                float step = Time.deltaTime / colorChangeSpeed;
                Color originalColor;
                if (targetColor == completeColor)
                {
                    originalColor = uncompleteColor;
                }
                else
                {
                    originalColor = completeColor;
                }
                colorChangeTimer += step;
                SetAllColor(Color.Lerp(originalColor, targetColor, colorChangeTimer));
                glow.color = Color.Lerp(originalColor, targetColor, colorChangeTimer);
                if (colorChangeTimer >= 1.0f)
                {
                    changing = false;
                }
            }

            if (!play && !editor)
            {
                transform.Rotate(new Vector3(Random.Range(10, 20), Random.Range(10, 20), Random.Range(10, 20)) * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(transform.eulerAngles + (new Vector3(20, 5, 0) * Time.deltaTime));
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                play = !play;
            }
        }
	}

    //Hides the cube so that performance can be increased.
    public void HideCube()
    {
        if (allActive)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.gameObject.SetActive(false);
            }
        }
        allActive = false;
      
    }

    //Show the cube
    public void ShowCube()
    {
        if (!allActive)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.gameObject.SetActive(true);
            }
        }
        allActive = true;
    
    }

    //Sets the colors properly
    public void SetAllColor(Color targetColor)
    {
        for (int i = 0; i < playTiles.childCount; i++)
        {
            if (playTiles.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                targetRenderer = playTiles.GetChild(i).GetComponent<MeshRenderer>();
                targetRenderer.material.color = targetColor;
            }
        }
    }

    bool CheckAllReceiver()
    {
        bool completion = true;
        for(int i = 0; i < receiverCompletion.Count; i++)
        {
            if(receiverCompletion[i] == false)
            {
                completion = false;
            }

        }
        return completion;
    }

    public void EmptyList()
    {
        receiverCompletion = new List<bool>();
    }


}
