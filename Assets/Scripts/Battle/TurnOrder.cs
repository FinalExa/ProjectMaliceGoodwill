using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    public List<Character> playableCharacters;
    public List<Character> enemyCharacters;
    [SerializeField] private bool enemyGoFirst;
    public List<Character> characterOrder;
    public List<Character> characterOrderMemory;
    [SerializeField] private int turnIndex;
    public Character currentCharacter;
    public Character nextCharacter;
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
        characterOrderMemory = new List<Character>();
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
        characterOrderMemory = characterOrder;
        turnIndex = 0;
        SetCurrentCharacter();
    }

    private void SetCurrentCharacter()
    {
        if (nextCharacter == null)
        {
            currentCharacter = characterOrder[turnIndex];
            currentCharacter.isLocked = false;
            currentCharacter.passageDone = false;
            nextCharacter = characterOrder[turnIndex + 1];
        }
        else
        {
            if (!characterOrder.Contains(nextCharacter)) SetNextCharacter();
            currentCharacter = nextCharacter;
            turnIndex = characterOrder.IndexOf(currentCharacter);
            currentCharacter.isLocked = false;
            currentCharacter.passageDone = false;
            SetNextCharacter();
        }
    }

    private void SetNextCharacter()
    {
        int index = characterOrderMemory.IndexOf(currentCharacter);
        for (int i = 0; i < characterOrderMemory.Count; i++)
        {
            if (characterOrder.Contains(characterOrderMemory[index]) && characterOrderMemory[index] != currentCharacter)
            {
                nextCharacter = characterOrderMemory[index];
                break;
            }
            index++;
            if (index >= characterOrderMemory.Count) index = 0;
        }
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
