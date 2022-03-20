using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public Character currentCharacter;
    [SerializeField] private TurnOrder turnOrder;

    private void Update()
    {
        if (currentCharacter != null) TurnOperations();
    }

    private void TurnOperations()
    {
        if (Input.GetKeyDown(KeyCode.F) && !currentCharacter.isAI)
        {
            print(currentCharacter.characterStats.characterName + " does Action!");
            currentCharacter.PassTurn();
        }
        else if (currentCharacter.isAI)
        {
            print(currentCharacter.characterStats.characterName + " does Action!");
            currentCharacter.PassTurn();
        }
    }
}
