using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    [HideInInspector] public Character currentCharacter;
    [SerializeField] private TurnOrder turnOrder;
    [SerializeField] private GameObject actionsParent;
    [SerializeField] private Dropdown actionsDropdown;
    [SerializeField] private GameObject targetsParent;
    [SerializeField] private Dropdown targetsDropdown;
    private List<Character> possibleTargets = new List<Character>();
    private Action chosenAction;
    private Character target;
    private bool stop;

    private void Update()
    {
        if (currentCharacter != null && !currentCharacter.isLocked) TurnOperations();
    }

    private void TurnOperations()
    {
        if (!currentCharacter.isAI && !stop) PlayableCharacterTurn();
        else if (currentCharacter.isAI) AITurn();
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

    private void AITurn()
    {
        print(currentCharacter.characterStats.characterName + " does Action!");
        currentCharacter.PassTurn();
    }

    private void ActionPopulate()
    {
        actionsParent.SetActive(true);
        List<string> actionsList = new List<string>();
        foreach (Action action in currentCharacter.characterActions) actionsList.Add(action.actionName);
        actionsDropdown.ClearOptions();
        actionsDropdown.AddOptions(actionsList);
    }

    public void ActionConfirm()
    {
        chosenAction = currentCharacter.characterActions[actionsDropdown.value];
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
                    possibleTargetsNames.Add(enemy.characterStats.characterName);
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
                    possibleTargetsNames.Add(turnOrder.playableCharacters[i].characterStats.characterName);
                }
            }
        }
        if (chosenAction.canHitSelf)
        {
            possibleTargets.Add(currentCharacter);
            possibleTargetsNames.Add(currentCharacter.characterStats.characterName);
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
        CharacterStats targetInfo = target.characterStats;
        targetInfo.MGCurrentValue -= chosenAction.severity;
        targetInfo.currentStamina -= chosenAction.staminaDamage;
        targetInfo.currentMental -= chosenAction.mentalDamage;
        target.UpdateAllBars();
        ActionOnSpectators();
        if (targetInfo.currentStamina <= 0 || targetInfo.currentMental <= 0 || targetInfo.MGCurrentValue <= 0) target.incapacitated = true;
    }

    private void ActionOnSpectators()
    {
        for (int i = 0; i < turnOrder.turnOrder.Count; i++)
        {
            if (turnOrder.turnOrder[i] != this || turnOrder.turnOrder[i] != target || !turnOrder.turnOrder[i].incapacitated)
            {
                CharacterStats targetInfo = turnOrder.turnOrder[i].characterStats;
                targetInfo.MGCurrentValue -= chosenAction.severitySpectator;
                targetInfo.currentStamina -= chosenAction.staminaDamageSpectator;
                targetInfo.currentMental -= chosenAction.mentalDamageSpectator;
                turnOrder.turnOrder[i].UpdateAllBars();
            }
        }
    }
}
