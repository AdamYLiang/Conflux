using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitcher : MonoBehaviour {

    public List<GameObject> puzzles;

    private GameObject currentPuzzle;
    private int index = 0;

	// Use this for initialization
	void Start () {
        currentPuzzle = puzzles[index];
        currentPuzzle.GetComponent<PuzzleManager>().hidden = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        PuzzleManager pm = currentPuzzle.GetComponent<PuzzleManager>();
        if (pm.finished)
        {
            pm.hidden = true;
            index++;
            currentPuzzle = puzzles[index];
            currentPuzzle.GetComponent<PuzzleManager>().hidden = false;
        }
	}
}
