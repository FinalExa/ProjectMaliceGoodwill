using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    public GameData gameData;
    [HideInInspector] public Character currentCharacter;
    [HideInInspector] public Character target;
    [HideInInspector] public List<Character> possibleTargets = new List<Character>();
    [HideInInspector] public Action chosenAction;
    [HideInInspector] public bool stop;
    [HideInInspector] public TurnOrder turnOrder;
    [HideInInspector] public PopulateDropdowns populateDropdowns;
    private ActionEffect actionEffect;
    private AITurn aiTurn;
    private PerditionTurn perditionTurn;
    private EndBattleConditions endBattleConditions;
    [HideInInspector] public bool fightIsOver;

    private void Awake()
    {
        turnOrder = this.gameObject.GetComponent<TurnOrder>();
        populateDropdowns = this.gameObject.GetComponent<PopulateDropdowns>();
        actionEffect = this.gameObject.GetComponent<ActionEffect>();
        aiTurn = this.gameObject.GetComponent<AITurn>();
        perditionTurn = this.gameObject.GetComponent<PerditionTurn>();
        endBattleConditions = this.gameObject.GetComponent<EndBattleConditions>();
    }

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
                else if (currentCharacter.characterData.isAI) aiTurn.AIStartup(currentCharacter);
            }
            else perditionTurn.PerditionStartup(currentCharacter);
        }
        else PassTurn();
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

    public void PassTurn()
    {
        currentCharacter.isLocked = true;
        currentCharacter.passageDone = false;
        turnOrder.turnWait = true;
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
