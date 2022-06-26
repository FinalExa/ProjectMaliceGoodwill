using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateDropdowns : MonoBehaviour
{
    [SerializeField] private Turn turn;
    [SerializeField] private GameObject actionsParent;
    [SerializeField] private Dropdown actionsDropdown;
    [SerializeField] private GameObject targetsParent;
    [SerializeField] private Dropdown targetsDropdown;

    public void ActionPopulate()
    {
        actionsParent.SetActive(true);
        List<string> actionsList = new List<string>();
        Character curCharacter = turn.currentCharacter;
        float MG = curCharacter.characterData.characterStats.BGCurrentValue;
        foreach (CharacterData.CharacterActions characterAction in curCharacter.characterData.characterActions)
        {
            Action action = characterAction.action;
            if (MG >= characterAction.BGMinValue && MG <= characterAction.BGMaxValue) actionsList.Add(action.actionName);
        }
        actionsDropdown.ClearOptions();
        actionsDropdown.AddOptions(actionsList);
    }

    public void ActionConfirm()
    {
        foreach (CharacterData.CharacterActions characterAction in turn.currentCharacter.characterData.characterActions)
        {
            Action action = characterAction.action;
            if (action.actionName == actionsDropdown.options[actionsDropdown.value].text)
            {
                turn.chosenAction = action;
                break;
            }
        }
        actionsParent.SetActive(false);
        TargetPopulate();
    }

    private void TargetPopulate()
    {
        targetsParent.SetActive(true);
        turn.possibleTargets.Clear();
        List<string> possibleTargetsNames = new List<string>();
        if (turn.chosenAction.canTargetOthers)
        {
            foreach (Character other in turn.turnOrder.turnOrder)
            {
                if (!other.Dead && other != turn.currentCharacter)
                {
                    turn.possibleTargets.Add(other);
                    possibleTargetsNames.Add(other.characterData.characterStats.characterName);
                }
            }
        }
        if (turn.chosenAction.canTargetSelf)
        {
            turn.possibleTargets.Add(turn.currentCharacter);
            possibleTargetsNames.Add(turn.currentCharacter.characterData.characterStats.characterName);
        }
        targetsDropdown.ClearOptions();
        targetsDropdown.AddOptions(possibleTargetsNames);
    }

    public void TargetConfirm()
    {
        turn.target = turn.possibleTargets[targetsDropdown.value];
        turn.ActionDoneOnTarget();
        targetsParent.SetActive(false);
        turn.PassTurn();
    }

    public void TurnAllOff()
    {
        actionsParent.SetActive(false);
        targetsParent.SetActive(false);
    }

    public void BackToAction()
    {
        targetsParent.SetActive(false);
        ActionPopulate();
    }
}
