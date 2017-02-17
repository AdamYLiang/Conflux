using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionNOde : MonoBehaviour {

    public bool connected = false;

    ConnectedInfo info;

    void Start()
    {
        if (GetComponent<ConnectedInfo>() != null)
        {
            info = GetComponent<ConnectedInfo>();
        }
    }
}
