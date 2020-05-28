using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;
    bool hasLanded;
    bool thrown;

    Vector3 initPosition;
    
    public DiceSides[] diceSides;
    public int diceValue;
    void Start()
    {
        initPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        if(rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;

            //Side Value Check
            SideValueCheck();
        }
        else if(rb.IsSleeping() && hasLanded && diceValue == 0) 
        {
            //Roll agaain
            RollAgain();
        }
    }

    void RollAgain()
    {
        Reset();
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(Random.Range(0,500), Random.Range(0,500), Random.Range(0,500));
    }

    public void RollDice()
    {
        //Reset  Dice
        Reset();

        if(!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0,500), Random.Range(0,500), Random.Range(0,500));
        }
        else if (thrown && hasLanded)
        {
            //reset
            Reset();
        }
    }

    void Reset()
    {
        transform.position = initPosition;
        rb.isKinematic = false;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    void SideValueCheck()
    {
        diceValue = 0;
        foreach(DiceSides side in diceSides)
        {
            if(side.OnGround())
            {
                diceValue = side.sideValue;
                //Send result to game manager
                GameManager.instance.RolledNumber(diceValue);
            }
        }
    }
}
