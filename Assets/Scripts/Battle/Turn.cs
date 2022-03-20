using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public Character currentCharacter;
    [SerializeField] private TurnOrder turnOrder;

    private void Update()
    {
        if (currentCharacter != null && !currentCharacter.isLocked) TurnOperations();
    }

    private void TurnOperations()
    {
        if (Input.GetKeyDown(KeyCode.F) && !currentCharacter.isAI) PlayableCharacterTurn();
        else if (currentCharacter.isAI) AITurn();
    }

    private void PlayableCharacterTurn()
    {
        print(currentCharacter.characterStats.characterName + " does Action!");
        currentCharacter.PassTurn();
    }

    private void AITurn()
    {
        print(currentCharacter.characterStats.characterName + " does Action!");
        currentCharacter.PassTurn();
    }
}
