using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] nodes;
    public List<Transform> nodeList = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        FillNodes();
    }

    void FillNodes()
    {
        nodeList.Clear();
        nodes = GetComponentsInChildren<Transform>();

        int num = -1;
        foreach(Transform child in nodes)
        {
            Node n = child.GetComponent<Node>();
            if(child != this.transform && n != null)
            {
                num++;
                nodeList.Add(child);
                child.gameObject.name = "field " + num;
                
                // Fill in node id to the node

                n.SetNodeID(num);
            }
        }
    }

    void OnDrawGizmos()
    {
        FillNodes();
        
        for(int i = 0; i < nodeList.Count; i++)
        {
            Vector3 start = nodeList[i].position;
            if(i > 0)
            {
                Vector3 previous = nodeList[i - 1].position;
                Debug.DrawLine(previous, start, Color.green);
            }
        }
    }

}
