using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour {

    //The puzzle we are representing in the game space
    public GameObject puzzle;

    //The factor/multiplier we apply to the original scale to get our target scale.
    public float miniatureFactor = 0.1f, playFactor = 1.0f;

    //Whether or not the puzzle is currently showing
    public bool puzzleShowing = false;

    //Time spent lerping
    public float lerpTime = 2f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowPuzzle();
        }
	}

    //Coroutine to lerp the puzzle into view. As well as scale it.
    public IEnumerator ShowPuzzleCoroutine()
    {

        float step = 0f, miniScale = miniatureFactor, playScale = playFactor;
        Vector3 showPosition = transform.position + Vector3.up * 2;
        Vector3 originalposition = transform.position;
        Vector3 originalScale = transform.localScale;
        Vector3 lerpScale = originalScale * playFactor;
        puzzle.GetComponent<PuzzleManager>().ShowCube();
        while (true)
        {
            //Lerp both the position and the scale
            puzzle.transform.localScale = Vector3.Lerp(originalScale, lerpScale, step);
            puzzle.transform.position = Vector3.Lerp(originalposition, showPosition, step);
            step = Mathf.Clamp01(step + Time.deltaTime / lerpTime);

            //Break the while loop if we have finished our lerp
            if(step >= 1.0f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void ShowPuzzle()
    {
        puzzleShowing = true;
        StartCoroutine(ShowPuzzleCoroutine());
    }
}
