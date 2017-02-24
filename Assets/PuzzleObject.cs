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

    protected Vector3 miniScale, playScale;
    protected bool lerping = false;

    public SteamVR_Controller.Device mainController;
    public SteamVR_TrackedObject trackedObj;

    public GameObject gameManager;
    protected GameManager gm;


    // Use this for initialization
    void Start () {
        puzzle.GetComponent<PuzzleManager>().HideCube();
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManager>();
        miniScale = puzzle.transform.localScale * miniatureFactor;
        playScale = puzzle.transform.localScale * playFactor;
    }

    //Coroutine to lerp the puzzle back to the device.
    public IEnumerator HidePuzzleCoroutine()
    {
        //puzzle.GetComponent<PuzzleManager>().hidden = false;
        float step = 0f;
        Vector3 showPosition = transform.position + Vector3.up * 1;
        Vector3 originalposition = transform.position;
        lerping = true;
        while (true)
        {
            //Lerp both the position and the scale
            puzzle.transform.localScale = Vector3.Lerp(playScale, miniScale, step);
            puzzle.transform.position = Vector3.Lerp(showPosition, originalposition, step);
            step = Mathf.Clamp01(step + Time.deltaTime / lerpTime);

            //Break the while loop if we have finished our lerp
            if (step >= 1.0f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        puzzle.GetComponent<PuzzleManager>().HideCube();
        lerping = false;
    }

    //Coroutine to lerp the puzzle into view. As well as scale it.
    public IEnumerator ShowPuzzleCoroutine()
    {
        //puzzle.GetComponent<PuzzleManager>().hidden = false;
        float step = 0f;
        Vector3 showPosition = transform.position + Vector3.up * 1;
        Vector3 originalposition = transform.position;
        puzzle.GetComponent<PuzzleManager>().ShowCube();
        lerping = true;
        while (true)
        {
            //Lerp both the position and the scale
            puzzle.transform.localScale = Vector3.Lerp(miniScale , playScale, step);
            puzzle.transform.position = Vector3.Lerp(originalposition, showPosition, step);
            step = Mathf.Clamp01(step + Time.deltaTime / lerpTime);

            //Break the while loop if we have finished our lerp
            if(step >= 1.0f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        lerping = false;
    }

    public void HidePuzzle()
    {
        puzzleShowing = false;
        StartCoroutine(HidePuzzleCoroutine());
    }

    public void ShowPuzzle()
    {
        puzzleShowing = true;
        StartCoroutine(ShowPuzzleCoroutine());
    }

    void OnTriggerStay(Collider col)
    {
        //Debug.Log(col.name);

        if(gm.leftController != null)
        {
            if (gm.leftController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("Pressing");
            }

            if (gm.leftController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) &&
            col.transform.parent == gm.controller1)
            {
                if (!lerping)
                {
                    if (!puzzleShowing)
                    {
                        ShowPuzzle();
                        puzzleShowing = true;
                    }
                    else
                    {
                        HidePuzzle();
                        puzzleShowing = false;
                    }

                }
            }
        }
       
        if(gm.rightController != null)
        {
            if (gm.rightController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) &&
                col.transform.parent.gameObject == gm.controller2)
            {
                Debug.Log("Right");
                if (!lerping)
                {
                    if (!puzzleShowing)
                    {
                        ShowPuzzle();
                        puzzleShowing = true;
                    }
                    else
                    {
                        HidePuzzle();
                        puzzleShowing = false;
                    }

                }
            }
        }
      

        //Controller2 is the right controller
        
    }
}
