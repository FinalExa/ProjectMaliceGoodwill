using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    [HideInInspector] public Character currentCharacter;
    [SerializeField] private TurnOrder turnOrder;
    [SerializeField] private ActionEffect actionEffect;
    [SerializeField] private AITurn AITurn;
    [SerializeField] private GameObject actionsParent;
    [SerializeField] private Dropdown actionsDropdown;
    [SerializeField] private GameObject targetsParent;
    [SerializeField] private Dropdown targetsDropdown;
    private List<Character> possibleTargets = new List<Character>();
    private Action chosenAction;
    private Character target;
    private bool stop;
    [HideInInspector] public bool fightIsOver;

    private void Update()
    {
        if (currentCharacter != null && !currentCharacter.isLocked && !fightIsOver) TurnOperations();
    }

    private void TurnOperations()
    {
        if (!currentCharacter.characterData.isAI && !stop) PlayableCharacterTurn();
        else if (currentCharacter.characterData.isAI) AITurn.AIStartup(currentCharacter);
    }

    private void PlayableCharacterTurn()
    {
        if (!currentCharacter.incapacitated)
        {
            ActionPopulate();
            stop = true;
        }
        else currentCharacter.PassTurn();
    }

    private void ActionPopulate()
    {
        actionsParent.SetActive(true);
        List<string> actionsList = new List<string>();
        foreach (Action action in currentCharacter.characterData.characterActions) actionsList.Add(action.actionName);
        actionsDropdown.ClearOptions();
        actionsDropdown.AddOptions(actionsList);
    }

    public void ActionConfirm()
    {
        chosenAction = currentCharacter.characterData.characterActions[actionsDropdown.value];
        actionsParent.SetActive(false);
        TargetPopulate();
    }

    private void TargetPopulate()
    {
        targetsParent.SetActive(true);
        possibleTargets.Clear();
        List<string> possibleTargetsNames = new List<string>();
        if (chosenAction.canHitEnemies)
        {
            foreach (Character enemy in turnOrder.enemyCharacters)
            {
                if (!enemy.incapacitated)
                {
                    possibleTargets.Add(enemy);
                    possibleTargetsNames.Add(enemy.characterData.characterStats.characterName);
                }
            }
        }
        if (chosenAction.canHitAllies)
        {
            for (int i = 0; i < turnOrder.playableCharacters.Length; i++)
            {
                if (turnOrder.playableCharacters[i] != currentCharacter && !turnOrder.playableCharacters[i].incapacitated)
                {
                    possibleTargets.Add(turnOrder.playableCharacters[i]);
                    possibleTargetsNames.Add(turnOrder.playableCharacters[i].characterData.characterStats.characterName);
                }
            }
        }
        if (chosenAction.canHitSelf)
        {
            possibleTargets.Add(currentCharacter);
            possibleTargetsNames.Add(currentCharacter.characterData.characterStats.characterName);
        }
        targetsDropdown.ClearOptions();
        targetsDropdown.AddOptions(possibleTargetsNames);
    }

    public void TargetConfirm()
    {
        target = possibleTargets[targetsDropdown.value];
        ActionDoneOnTarget();
        targetsParent.SetActive(false);
        stop = false;
        currentCharacter.PassTurn();
    }

    private void ActionDoneOnTarget()
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
