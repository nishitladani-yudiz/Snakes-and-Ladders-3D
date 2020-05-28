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
    float cTime = 0;
    float amptitude = 0.5f;
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

    IEnumerator Move()
    {
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;

        nodeList[routePosition].RemoveStone(this);

        while(stepsToMove > 0)
        {
            routePosition++;
            Vector3 nextPos = route.nodeList[routePosition].transform.position;
            //Arc Movement
            Vector3 startPos = route.nodeList[routePosition - 1].transform.position;
            while(MoveInArcToNextNode(startPos, nextPos, 4f))  { yield return null; }

            //Straight movement
            //while(MoveToNextNode(nextPos))  { yield return null; }

            yield return new WaitForSeconds(0.1f);

            cTime = 0;
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

        nodeList[routePosition].AddStone(this);

        //Check for a win
        if(doneSteps == nodeList.Count - 1)
        {
            //Report to game manager
            GameManager.instance.ReportWinner();
            yield break;
        }

        //Update the game manager
        GameManager.instance.state = GameManager.States.SWITCH_PLAYER;

        isMoving = false;
    }

    bool MoveToNextNode(Vector3 nextPos)
    {
        return nextPos != (transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime)); 
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 nextPos, float _speed)
    {
        cTime += _speed * Time.deltaTime;
        Vector3 myPosition = Vector3.Lerp(startPos, nextPos, cTime);
        myPosition.y += amptitude * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);

        return nextPos != (transform.position = Vector3.Lerp(transform.position, myPosition, cTime));
    }

    public void MakeTurn(int diceNumber)
    {
        stepsToMove = diceNumber;
        if(doneSteps + stepsToMove < route.nodeList.Count)
        {
            StartCoroutine(Move());
        }
        else
        {
            print("Number is too high");
            GameManager.instance.state = GameManager.States.SWITCH_PLAYER;
        }
    }
}
