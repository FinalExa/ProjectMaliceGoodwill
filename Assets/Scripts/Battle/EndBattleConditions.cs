using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattleConditions : MonoBehaviour
{
    [SerializeField] private Turn turn;
    [SerializeField] private TurnOrder turnOrder;
    [HideInInspector] public bool perditionCheck;
    private bool stopThis;
    private bool enemiesDownCondition;
    private bool enemiesGoodwillCondition;
    private bool alliesDownCondition;
    private bool alliesPerditionCondition;
    private bool allPlayableInPerditionCondition;

    private void Start()
    {
        perditionCheck = false;
    }

    private void Update()
    {
        if (!stopThis && (enemiesDownCondition || enemiesGoodwillCondition) && !alliesPerditionCondition) Victory();
        if (!stopThis && (alliesDownCondition || allPlayableInPerditionCondition)) Defeat();
    }

    public void CheckForVictoryConditions()
    {
        EnemiesDownCondition();
        EnemiesAltruismCondition();
        AlliesPerditionCondition();
        AlliesDownCondition();
        AllPlayableInPerditionCondition();
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
        if (enemiesDown) enemiesDownCondition = true;
    }
    private void AlliesDownCondition()
    {
        bool alliesDown = true;
        foreach (Character pc in turnOrder.playableCharacters)
        {
            if (!pc.incapacitated)
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
        foreach (Character pc in turnOrder.playableCharacters)
        {
            if (pc.perdition)
            {
                count++;
            }
        }
        if (count == turnOrder.playableCharacters.Length) allPlayableInPerditionCondition = true;
    }
    private void AlliesPerditionCondition()
    {
        bool alliesPerdition = false;
        foreach (Character pc in turnOrder.playableCharacters)
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
    private void EnemiesAltruismCondition()
    {
        if (perditionCheck) return;
        int count = 0;
        foreach (Character enemy in turnOrder.enemyCharacters)
        {
            if (enemy.fullGoodAI) count++;
        }
        if (count == turnOrder.enemyCharacters.Length) enemiesGoodwillCondition = true;
    }

    private void Victory()
    {
        turn.populateDropdowns.TurnAllOff();
        turn.fightIsOver = true;
        stopThis = true;
        print("Victory!");
        if (enemiesDownCondition) print("All enemies knocked out");
        if (enemiesGoodwillCondition) print("All enemies altruism maxed out");
    }

    private void Defeat()
    {
        turn.populateDropdowns.TurnAllOff();
        turn.fightIsOver = true;
        stopThis = true;
        print("Defeat!");
        if (alliesDownCondition) print("All playable characters down!");
        if (allPlayableInPerditionCondition) print("All playable characters in perdition!");
    }
}
