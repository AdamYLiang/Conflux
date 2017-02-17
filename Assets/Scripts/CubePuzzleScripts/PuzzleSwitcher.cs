using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitcher : MonoBehaviour {

    public List<GameObject> puzzles;

    private GameObject currentPuzzle;
    private int index = 0;
    private bool allDone = false;

	// Use this for initialization
	void Start () {
        currentPuzzle = puzzles[index];
        currentPuzzle.GetComponent<PuzzleManager>().hidden = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!allDone)
        {
            PuzzleManager pm = currentPuzzle.GetComponent<PuzzleManager>();
            if (pm.finished)
            {
                //pm.GetComponent<AudioSource>().Play();
                StartCoroutine(pm.HidePuzzle());
                index++;
                if (index < puzzles.Count)
                {
                    currentPuzzle = puzzles[index];
                    currentPuzzle.GetComponent<PuzzleManager>().hidden = false;
                }
                else
                {
                    allDone = true;
                }
            }
        }
        
	}
}
