using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateDropdowns : MonoBehaviour
{
    [SerializeField] private Turn turn;
    [SerializeField] private GameObject targetsParent;
    [SerializeField] private Dropdown targetsDropdown;
    [SerializeField] private ActionButton actionButtonRef;
    private List<ActionButton> actionButtons;
    private GameObject actionsButtonParentCanvas;
    [SerializeField] private string actionsButtonParentCanvasTag;
    [SerializeField] private Vector3 actionButtonStartPos;
    [SerializeField] private Vector3 actionButtonOffsetToAdd;
    private Vector3 actionButtonCurrentPos;
    public Action selectedAction;

    private void Start()
    {
        actionsButtonParentCanvas = GameObject.FindGameObjectWithTag(actionsButtonParentCanvasTag);
        actionButtons = new List<ActionButton>();
    }

    public void ActionPopulate()
    {
        actionButtonCurrentPos = actionButtonStartPos;
        actionButtons.Clear();
        List<Action> actionsList = new List<Action>();
        Character curCharacter = turn.currentCharacter;
        float MG = curCharacter.characterData.characterStats.BGCurrentValue;
        foreach (CharacterData.CharacterActions characterAction in curCharacter.characterData.characterActions)
        {
            Action action = characterAction.action;
            if (MG >= characterAction.BGMinValue && MG <= characterAction.BGMaxValue) actionsList.Add(action);
        }
        foreach (Action action in actionsList)
        {
            ActionButton actionButton = Instantiate(actionButtonRef, actionsButtonParentCanvas.transform);
            actionButton.GetComponent<RectTransform>().position = actionButtonCurrentPos;
            actionButton.actionToActivate = action;
            actionButtons.Add(actionButton);
            actionButtonCurrentPos += actionButtonOffsetToAdd;
        }
    }

    public void ActionConfirm()
    {
        turn.chosenAction = selectedAction;
        foreach (ActionButton actionButton in actionButtons)
        {
            GameObject.Destroy(actionButton.gameObject);
        }
        actionButtons.Clear();
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
            foreach (Character other in turn.turnOrder.characterOrder)
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
        EndTargeting(possibleTargetsNames);
    }

    private void MultiTargetList()
    {
        List<string> groupNames = new List<string>();
        groupNames.Clear();
        if (turn.chosenAction.hitsEveryone)
        {
            groupNames.Add("Everyone");
            EndTargeting(groupNames);
            return;
        }
        if (turn.chosenAction.hitsAllOthers)
        {
            groupNames.Add("Others");
            EndTargeting(groupNames);
            return;
        }
        if (turn.chosenAction.hitsEnemyGroup) groupNames.Add("Enemies");
        if (turn.chosenAction.hitsAllyGroupSelfExcluded)
        {
            groupNames.Add("Allies");
            EndTargeting(groupNames);
            return;
        }
        if (turn.chosenAction.hitsAllyGroupSelfIncluded)
        {
            groupNames.Add("Party");
            EndTargeting(groupNames);
            return;
        }
        EndTargeting(groupNames);
    }

    private void EndTargeting(List<string> targetsList)
    {
        targetsDropdown.ClearOptions();
        targetsDropdown.AddOptions(targetsList);
    }

    public void TargetConfirm()
    {
        if (!turn.chosenAction.targetsGroups) turn.target = turn.possibleTargets[targetsDropdown.value];
        else turn.multiTargetingOption = targetsDropdown.options[targetsDropdown.value].text;
        turn.ActionDoneOnTarget();
        targetsParent.SetActive(false);
        turn.PassTurn();
    }

    public void TurnAllOff()
    {
        targetsParent.SetActive(false);
    }

    public void BackToAction()
    {
        targetsParent.SetActive(false);
        ActionPopulate();
    }
}
