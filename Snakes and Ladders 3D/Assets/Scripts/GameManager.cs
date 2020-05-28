using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int activePlayer;
    int diceNumber;

    

    [System.Serializable]
    public class Player
    {
        public string playerName;
        public Stone stone;
        public GameObject rollDiceButton;


        public enum PlayerTypes
        {
            CPU,
            HUMAN
        }
        public PlayerTypes playerType;
    }

    public List<Player> playerList = new List<Player>();

    public enum States
    {
        WAITING,
        ROLL_DICE,
        SWITCH_PLAYER
    }

    public States state;
    public Dice dice;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //ActivateButton(false);
        DeactivateAllButton();

        activePlayer = Random.Range(0,playerList.Count);
        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " starts first!");
    }

    void Update()
    {
        if(playerList[activePlayer].playerType == Player.PlayerTypes.CPU)
        {
            switch (state)
            {
                case States.WAITING:
                    {
                        // Idle 
                    }
                    break;
                case States.ROLL_DICE:
                    {
                        StartCoroutine(RollDiceDelay());
                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    {
                        activePlayer++;
                        activePlayer %= playerList.Count;
                        //info box
                        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " 's Turn!");
                        state = States.ROLL_DICE;
                    }
                    break;
            }
        }


        if(playerList[activePlayer].playerType == Player.PlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.WAITING:
                    {
                        // Idle 
                    }
                    break;
                case States.ROLL_DICE:
                    {
                        ActivateSpecificButton(true);
                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    {
                        activePlayer++;
                        activePlayer %= playerList.Count;
                        //info box
                        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " 's Turn!");
                        state = States.ROLL_DICE;
                    }
                    break;
            }
        }
    }

    IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(2);
        //diceNumber = Random.Range(1,7);
        Debug.Log(diceNumber);

        //Physical roll dice
        dice.RollDice();
        
    }

    public void RolledNumber(int _diceNumber)
    {
        diceNumber = _diceNumber;
        //info box
        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " has rolled a " + diceNumber);

        //Make a turn
        playerList[activePlayer].stone.MakeTurn(diceNumber);
    }

    /*void ActivateButton(bool on)
    {
        rollDiceButton.SetActive(on);
    }*/

    public void HumanRollDice()
    {
        //ActivateButton(false);
        ActivateSpecificButton(false);
        StartCoroutine(RollDiceDelay());
    }
    void ActivateSpecificButton(bool on)
    {
        playerList[activePlayer].rollDiceButton.SetActive(on);
    }

    void DeactivateAllButton()
    {
        for(int i = 0; i < playerList.Count; i++)
        {
            playerList[i].rollDiceButton.SetActive(false);
        }
    }

    public void ReportWinner()
    {
        //Show Win Stuff
        Debug.Log(playerList[activePlayer].playerName + "has won this round");
    }
}
