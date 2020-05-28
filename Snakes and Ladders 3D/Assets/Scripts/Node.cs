using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int nodeID;              // Position in the route
    public Text numberText;
    public Node connectedNode;      // Ladder or Snake
    List<Stone> stoneList = new List<Stone>();



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


    public void AddStone(Stone stone)
    {
        stoneList.Add(stone);
        //Rearrange
        ReArrangeStones();
    }

    public void RemoveStone(Stone stone)
    {
        stoneList.Remove(stone);
        //Rearrange
        ReArrangeStones();
    }

    void ReArrangeStones()
    {
        if(stoneList.Count > 1)
        {
            int squareSize = Mathf.CeilToInt(Mathf.Sqrt(stoneList.Count));
            int stone = -1;
            for(int i = 0; i < squareSize; i++)
            {
                for(int j = 0; j < squareSize; j++)
                {
                    stone++;
                    if(stone > stoneList.Count - 1)
                    {
                        break;
                    }
                    Vector3 newPos = transform.position + new Vector3(-0.25f + i * 0.5f, 0, -0.25f + j * 0.5f);
                    stoneList[stone].transform.position = newPos;
                }
            }
        }
        else if(stoneList.Count == 1)
        {
            stoneList[0].transform.position = transform.position;
        }
    }

}
