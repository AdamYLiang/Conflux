using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNodeInfo : MonoBehaviour {

    public PuzzleManager pm;

    void Start()
    {
        if(transform.parent.parent != null)
        {
            pm = transform.parent.parent.GetComponent<PuzzleManager>();
        }
    }
}
