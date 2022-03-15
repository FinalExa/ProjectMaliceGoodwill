using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    [SerializeField] private Character[] playableCharacters;
    [SerializeField] private Character[] enemyCharacters;
    [SerializeField] private bool enemyGoFirst;
    [HideInInspector] public List<Character> turnOrder;
    private int turnIndex;

    private void Start()
    {
        CreateTurnOrder();
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
    }

    private void ComposeList(Character[] arrayToAdd)
    {
        for (int i = 0; i < arrayToAdd.Length; i++)
        {
            turnOrder.Add(arrayToAdd[i]);
        }
    }

    public void GoToNextTurn()
    {
        if (turnIndex + 1 < turnOrder.Count) turnIndex++;
        else turnIndex = 0;
        print(turnIndex);
    }
}
