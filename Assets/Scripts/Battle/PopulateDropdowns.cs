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
        if (!turn.chosenAction.targetsGroups) SingleTargetList();
        else MultiTargetList();
    }

    private void SingleTargetList()
    {
        List<string> possibleTargetsNames = new List<string>();
        possibleTargetsNames.Clear();
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
        EndTargeting(possibleTargetsNames, false);
    }

    private void MultiTargetList()
    {
        List<string> groupNames = new List<string>();
        groupNames.Clear();
        if (turn.chosenAction.hitsEveryone)
        {
            groupNames.Add("Everyone");
            EndTargeting(groupNames, true);
            return;
        }
        if (turn.chosenAction.hitsAllOthers)
        {
            groupNames.Add("Others");
            EndTargeting(groupNames, true);
            return;
        }
        if (turn.chosenAction.hitsEnemyGroup) groupNames.Add("Enemies");
        if (turn.chosenAction.hitsAllyGroupSelfExcluded)
        {
            groupNames.Add("Allies");
            EndTargeting(groupNames, true);
            return;
        }
        if (turn.chosenAction.hitsAllyGroupSelfIncluded)
        {
            groupNames.Add("Party");
            EndTargeting(groupNames, true);
            return;
        }
        EndTargeting(groupNames, true);
    }

    private void EndTargeting(List<string> targetsList, bool isMultiTargeting)
    {
        targetsDropdown.ClearOptions();
        targetsDropdown.AddOptions(targetsList);
        turn.multiTargeting = isMultiTargeting;
    }

    public void TargetConfirm()
    {
        if (!turn.multiTargeting) turn.target = turn.possibleTargets[targetsDropdown.value];
        else turn.multiTargetingOption = targetsDropdown.options[targetsDropdown.value].text;
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
