using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    [HideInInspector] public Character currentCharacter;
    [HideInInspector] public Character target;
    [HideInInspector] public List<Character> possibleTargets = new List<Character>();
    [HideInInspector] public Action chosenAction;
    [HideInInspector] public bool stop;
    public TurnOrder turnOrder;
    public PopulateDropdowns populateDropdowns;
    [SerializeField] private ActionEffect actionEffect;
    [SerializeField] private AITurn AITurn;
    [SerializeField] private PerditionTurn perditionTurn;
    [SerializeField] private EndBattleConditions endBattleConditions;
    public Type.ActionType[] types;
    [HideInInspector] public bool fightIsOver;

    private void Update()
    {
        if (currentCharacter != null && !currentCharacter.isLocked && !fightIsOver) TurnOperations();
    }

    private void TurnOperations()
    {
        endBattleConditions.CheckForVictoryConditions();
        if (!fightIsOver) CheckForTurnToPlay();
    }

    private void CheckForTurnToPlay()
    {
        if (!currentCharacter.incapacitated)
        {
            if (!currentCharacter.perdition)
            {
                if (!currentCharacter.characterData.isAI && !stop) PlayableCharacterTurn();
                else if (currentCharacter.characterData.isAI) AITurn.AIStartup(currentCharacter);
            }
            else perditionTurn.PerditionStartup(currentCharacter);
        }
        else currentCharacter.PassTurn();
    }

    private void PlayableCharacterTurn()
    {
        populateDropdowns.ActionPopulate();
        stop = true;
        print(currentCharacter.characterData.characterStats.characterName + "'s turn.");
    }

    public void ActionDoneOnTarget()
    {
        actionEffect.UpdateValues(target, chosenAction, false);
        target.UpdateAllBars();
        ActionOnSpectators();
    }

    private void ActionOnSpectators()
    {
        for (int i = 0; i < turnOrder.turnOrder.Count; i++)
        {
            if ((turnOrder.turnOrder[i] != this || turnOrder.turnOrder[i] != target) && !turnOrder.turnOrder[i].incapacitated)
            {
                actionEffect.UpdateValues(turnOrder.turnOrder[i], chosenAction, true);
                turnOrder.turnOrder[i].UpdateAllBars();
            }
        }
    }
}
