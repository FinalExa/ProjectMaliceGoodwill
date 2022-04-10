using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MonoBehaviour
{
    [SerializeField] private Turn turn;
    private Character AIToControl;

    public void AIStartup(Character aiReceived)
    {
        AIToControl = aiReceived;
        print(AIToControl.characterData.characterStats.characterName + " does Action!");
        turn.currentCharacter.PassTurn();
    }
}
