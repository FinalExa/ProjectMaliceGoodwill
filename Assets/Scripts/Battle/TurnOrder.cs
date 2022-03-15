using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    [SerializeField] private Character[] playableCharacters;
    [SerializeField] private Character[] enemyCharacters;
    private List<Character> turnOrder;
    [SerializeField] private bool enemyGoFirst;

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
    }

    private void ComposeList(Character[] arrayToAdd)
    {
        for (int i = 0; i < arrayToAdd.Length; i++)
        {
            turnOrder.Add(arrayToAdd[i]);
            print(arrayToAdd[i]);
        }
    }
}
