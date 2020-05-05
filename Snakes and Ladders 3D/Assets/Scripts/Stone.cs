using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Route route;
    public List<Node> nodeList = new List<Node>();
    int routePosition;
    int stoneID;    //Player ID
    float speed = 8f;
    int stepsToMove;
    int doneSteps;
    bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform c in route.nodeList)
        {
            Node n = c.GetComponentInChildren<Node>();
            if(n != null)
            {
                nodeList.Add(n);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug Only
        if(Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            //Roll the dice
            stepsToMove = Random.Range(1,7);
            print("Dice Rolled : " + stepsToMove);

            if(doneSteps + stepsToMove < route.nodeList.Count)
            {
                StartCoroutine(Move());
            }
            else
            {
                print("Number is too high");
            }
        }
    }

    IEnumerator Move()
    {
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;

        while(stepsToMove > 0)
        {
            routePosition++;
            Vector3 nextPos = route.nodeList[routePosition].transform.position;

            while(MoveToNextNode(nextPos))  { yield return null; }

            yield return new WaitForSeconds(0.1f);
            stepsToMove--;
            doneSteps++;
        }

        yield return new WaitForSeconds(0.1f);

        // snake and ladder movement

        if(nodeList[routePosition].connectedNode != null)
        {
            int conNodeID = nodeList[routePosition].connectedNode.nodeID;
            Vector3 nextPos = nodeList[routePosition].connectedNode.transform.position;

            while(MoveToNextNode(nextPos)) { yield return null; }
            doneSteps = conNodeID;
            routePosition = conNodeID;

        }


        isMoving = false;
    }

    bool MoveToNextNode(Vector3 nextPos)
    {
        return nextPos != (transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime)); 
    }
}
