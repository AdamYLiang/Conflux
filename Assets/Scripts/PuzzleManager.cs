using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {


    public List<bool> receiverCompletion;
    public bool finished = false;
	
	// Update is called once per frame
	void Update () {
        finished = CheckAllReceiver();


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
