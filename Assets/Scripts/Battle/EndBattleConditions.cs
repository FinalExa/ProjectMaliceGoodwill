using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattleConditions : MonoBehaviour
{
    private Turn turn;
    private TurnOrder turnOrder;
    private List<Character> playables;
    private List<Character> enemies;
    private bool stopThis;
    private bool enemiesDownCondition;
    private bool enemiesGoodwillCondition;
    private bool alliesDownCondition;
    private bool alliesPerditionCondition;
    private bool allPlayableInPerditionCondition;

    private void Awake()
    {
        turn = this.gameObject.GetComponent<Turn>();
        turnOrder = turn.turnOrder;
    }

    private void Start()
    {
        playables = turnOrder.playableCharacters;
        enemies = turnOrder.enemyCharacters;
    }

    private void Update()
    {
        if (!stopThis && (enemiesDownCondition || enemiesGoodwillCondition) && !alliesPerditionCondition) Victory();
        if (!stopThis && (alliesDownCondition || allPlayableInPerditionCondition)) Defeat();
    }

    public void CheckForVictoryConditions()
    {
        EnemiesDownCondition();
        EnemiesGoodCondition();
        AlliesPerditionCondition();
        AlliesDownCondition();
        AllPlayableInPerditionCondition();
    }
    private void EnemiesDownCondition()
    {
        bool enemiesDown = true;
        foreach (Character enemy in enemies)
        {
            if (!enemy.Dead)
            {
                enemiesDown = false;
                break;
            }
        }
        if (enemiesDown) enemiesDownCondition = true;
    }
    private void AlliesDownCondition()
    {
        bool alliesDown = true;
        foreach (Character pc in playables)
        {
            if (!pc.Dead)
            {
                alliesDown = false;
                break;
            }
        }
        if (alliesDown) alliesDownCondition = true;
    }
    private void AllPlayableInPerditionCondition()
    {
        int count = 0;
        foreach (Character pc in playables)
        {
            if (pc.perdition)
            {
                count++;
            }
        }
        if (count == turnOrder.playableCharacters.Count) allPlayableInPerditionCondition = true;
    }
    private void AlliesPerditionCondition()
    {
        bool alliesPerdition = false;
        foreach (Character pc in playables)
        {
            if (pc.perdition)
            {
                alliesPerdition = true;
                break;
            }
        }
        if (alliesPerdition) alliesPerditionCondition = true;
        else alliesPerditionCondition = false;
    }
    private void EnemiesGoodCondition()
    {
        int count = 0;
        foreach (Character enemy in enemies)
        {
            if (enemy.fullGoodAI) count++;
        }
        if (count == turnOrder.enemyCharacters.Count) enemiesGoodwillCondition = true;
    }

    private void Victory()
    {
        turn.populateDropdowns.TurnAllOff();
        turn.fightIsOver = true;
        stopThis = true;
        if (enemiesDownCondition) turn.battleText.UpdateBattleText("Victory! All enemies killed!");
        if (enemiesGoodwillCondition) turn.battleText.UpdateBattleText("Victory! Enemies calmed!");
    }

    private void Defeat()
    {
        turn.populateDropdowns.TurnAllOff();
        turn.fightIsOver = true;
        stopThis = true;
        if (alliesDownCondition) turn.battleText.UpdateBattleText("Defeat! Party is dead!");
        if (allPlayableInPerditionCondition) turn.battleText.UpdateBattleText("Defeat! Party in perdition!");
    }
}
