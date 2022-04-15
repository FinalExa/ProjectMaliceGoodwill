using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerditionTurn : MonoBehaviour
{
    Character curCharacter;

    public void PerditionStartup(Character thisCharacter)
    {
        curCharacter = thisCharacter;
        print(curCharacter.characterData.characterStats.characterName + " enters perdition!");
        curCharacter.PassTurn();
    }
}
