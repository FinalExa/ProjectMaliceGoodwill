using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattleConditions : MonoBehaviour
{
    [SerializeField] private Turn turn;
    [SerializeField] private TurnOrder turnOrder;

    public void CheckForVictoryConditions()
    {
        EnemiesDownCondition();
    }
    private void EnemiesDownCondition()
    {
        bool enemiesDown = true;
        foreach (Character enemy in turnOrder.enemyCharacters)
        {
            if (!enemy.incapacitated)
            {
                enemiesDown = false;
                break;
            }
        }
        if (enemiesDown) Victory();
    }

    private void Victory()
    {
        turn.populateDropdowns.TurnAllOff();
        turn.fightIsOver = true;
        print("Victory!");
    }
}
