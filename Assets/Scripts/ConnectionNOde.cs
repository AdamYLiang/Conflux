using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionNOde : MonoBehaviour {

    public bool connected = false;

    public enum NodeType { Cube, Other };
    public NodeType type;

    ConnectedInfo info;

    void Start()
    {
        if (GetComponent<ConnectedInfo>() != null)
        {
            info = GetComponent<ConnectedInfo>();
        }
    }
}
