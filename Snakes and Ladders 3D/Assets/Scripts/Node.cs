using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int nodeID;              // Position in the route
    public Text numberText;
    public Node connectedNode;      // Ladder or Snake



    public void SetNodeID(int _nodeID)
    {
        nodeID = _nodeID;
        if(numberText != null)
        {
            numberText.text = nodeID.ToString();
        }
    }

    void OnDrawGizmos()
    {
        if(connectedNode != null)
        {
            Color col = Color.white;

            col = (connectedNode.nodeID > nodeID) ? Color.blue : Color.red;
            Debug.DrawLine(transform.position, connectedNode.transform.position, col);
        }
    }
}
