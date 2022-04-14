using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattleConditions : MonoBehaviour
{
    [SerializeField] private Turn turn;
    [SerializeField] private TurnOrder turnOrder;

    public void CheckForVictoryConditions(Character target)
    {
        if (target.characterData.isAI)
        {
            bool enemiesDown = true;
            foreach (Character enemy in turnOrder.enemyCharacters)
            {
                if (!enemy.characterData.incapacitated)
                {
                    enemiesDown = false;
                    break;
                }
            }
            if (enemiesDown)
            {
                turn.fightIsOver = true;
                print("Victory!");
            }
        }
        else
        {

        }
    }
}
