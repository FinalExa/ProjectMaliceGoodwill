using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    public List<Character> playableCharacters;
    public List<Character> enemyCharacters;
    [SerializeField] private bool enemyGoFirst;
    public List<Character> characterOrder;
    [SerializeField] private int turnIndex;
    public Character currentCharacter;
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
        characterOrder = new List<Character>();
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
        SetCurrentCharacter();
    }

    private void SetCurrentCharacter()
    {
        currentCharacter = characterOrder[turnIndex];
        currentCharacter.isLocked = false;
        currentCharacter.passageDone = false;
    }

    private void ComposeList(List<Character> arrayToAdd)
    {
        for (int i = 0; i < arrayToAdd.Count; i++)
        {
            arrayToAdd[i].isLocked = true;
            characterOrder.Add(arrayToAdd[i]);
        }
    }

    private void TurnWaitTimer()
    {
        if (turnWaitTime > 0) turnWaitTime -= Time.deltaTime;
        else
        {
            currentCharacter.isLocked = true;
            if (turnIndex + 1 < characterOrder.Count) turnIndex++;
            else turnIndex = 0;
            SetCurrentCharacter();
            turnWaitTime = turnWaitTimer;
            turnWait = false;
        }
    }
}
