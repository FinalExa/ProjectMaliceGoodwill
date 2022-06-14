using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    public Character[] playableCharacters;
    public Character[] enemyCharacters;
    [SerializeField] private bool enemyGoFirst;
    [HideInInspector] public List<Character> turnOrder;
    private int turnIndex;

    [SerializeField] private float turnWaitTimer;
    private float turnWaitTime;
    public bool turnWait;

    private void Start()
    {
        turnWaitTime = turnWaitTimer;
        CreateTurnOrder();
    }

    private void Update()
    {
        if (turnWait) TurnWaitTimer();
    }

    private void CreateTurnOrder()
    {
        turnOrder = new List<Character>();
        if (!enemyGoFirst)
        {
            ComposeList(playableCharacters);
            ComposeList(enemyCharacters);
        }
        else
        {
            ComposeList(enemyCharacters);
            ComposeList(playableCharacters);
        }
        turnIndex = 0;
        turnOrder[turnIndex].isLocked = false;
    }

    private void ComposeList(Character[] arrayToAdd)
    {
        for (int i = 0; i < arrayToAdd.Length; i++)
        {
            arrayToAdd[i].isLocked = true;
            turnOrder.Add(arrayToAdd[i]);
        }
    }

    private void TurnWaitTimer()
    {
        if (turnWaitTime > 0) turnWaitTime -= Time.deltaTime;
        else
        {
            if (turnIndex + 1 < turnOrder.Count) turnIndex++;
            else turnIndex = 0;
            turnOrder[turnIndex].isLocked = false;
            turnWaitTime = turnWaitTimer;
            turnWait = false;
        }
    }
}
