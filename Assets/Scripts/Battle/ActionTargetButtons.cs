using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTargetButtons : MonoBehaviour
{
    [SerializeField] private Turn turn;
    [SerializeField] private GameObject targetsParent;
    [SerializeField] private ActionButton actionButtonRef;
    [SerializeField] private TargetButton targetButtonRef;
    private List<ActionButton> actionButtons;
    private List<TargetButton> targetButtons;
    private GameObject buttonParentCanvas;
    [SerializeField] private string buttonParentCanvasTag;
    [SerializeField] private Vector3 buttonStartPos;
    [SerializeField] private Vector3 buttonOffsetToAdd;
    private Vector3 buttonCurrentPos;
    [HideInInspector] public Action selectedAction;
    [HideInInspector] public Character selectedTarget;
    [HideInInspector] public string selectedMultiTarget;
    private List<string> groupNames;

    private void Start()
    {
        groupNames = new List<string>();
        buttonParentCanvas = GameObject.FindGameObjectWithTag(buttonParentCanvasTag);
        actionButtons = new List<ActionButton>();
        targetButtons = new List<TargetButton>();
    }

    public void ActionPopulate()
    {
        buttonCurrentPos = buttonStartPos;
        actionButtons.Clear();
        List<Action> actionsList = new List<Action>();
        Character curCharacter = turn.currentCharacter;
        float MG = curCharacter.characterData.characterStats.BGCurrentValue;
        foreach (CharacterData.CharacterActions characterAction in curCharacter.characterData.characterActions)
        {
            Action action = characterAction.action;
            if (MG > characterAction.BGMinValue && MG <= characterAction.BGMaxValue) actionsList.Add(action);
        }
        foreach (Action action in actionsList)
        {
            ActionButton actionButton = Instantiate(actionButtonRef, buttonParentCanvas.transform);
            actionButton.GetComponent<RectTransform>().position = buttonCurrentPos;
            actionButton.actionToSelect = action;
            actionButtons.Add(actionButton);
            buttonCurrentPos += buttonOffsetToAdd;
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
        buttonCurrentPos = buttonStartPos;
        targetButtons.Clear();
        targetsParent.SetActive(true);
        turn.possibleTargets.Clear();
        if (!turn.chosenAction.targetsGroups) SingleTargetList();
        else
        {
            MultiTargetList();
            MultiTargetButtons();
        }
    }

    private void SingleTargetList()
    {
        List<Character> possibleTargetsNames = new List<Character>();
        possibleTargetsNames.Clear();
        if (turn.chosenAction.canTargetOthers)
        {
            foreach (Character other in turn.turnOrder.characterOrder)
            {
                if (!other.Dead && other != turn.currentCharacter)
                {
                    turn.possibleTargets.Add(other);
                }
            }
        }
        if (turn.chosenAction.canTargetSelf) turn.possibleTargets.Add(turn.currentCharacter);
        foreach (Character character in turn.possibleTargets)
        {
            TargetButton targetButton = Instantiate(targetButtonRef, buttonParentCanvas.transform);
            targetButton.GetComponent<RectTransform>().position = buttonCurrentPos;
            targetButton.targetToSelect = character;
            targetButtons.Add(targetButton);
            buttonCurrentPos += buttonOffsetToAdd;
        }
    }

    private void MultiTargetList()
    {
        groupNames.Clear();
        if (turn.chosenAction.hitsEveryone)
        {
            groupNames.Add("Everyone");
            return;
        }
        if (turn.chosenAction.hitsAllOthers)
        {
            groupNames.Add("Others");
            return;
        }
        if (turn.chosenAction.hitsEnemyGroup) groupNames.Add("Enemies");
        if (turn.chosenAction.hitsAllyGroupSelfExcluded)
        {
            groupNames.Add("Allies");
            return;
        }
        if (turn.chosenAction.hitsAllyGroupSelfIncluded)
        {
            groupNames.Add("Party");
            return;
        }
    }

    private void MultiTargetButtons()
    {
        foreach (string name in groupNames)
        {
            TargetButton targetButton = Instantiate(targetButtonRef, buttonParentCanvas.transform);
            targetButton.GetComponent<RectTransform>().position = buttonCurrentPos;
            targetButton.multiTargetToSelect = name;
            targetButton.isMultiTarget = true;
            targetButtons.Add(targetButton);
            buttonCurrentPos += buttonOffsetToAdd;
        }
    }

    public void TargetConfirm()
    {
        if (!turn.chosenAction.targetsGroups) { turn.target = selectedTarget; }
        else turn.multiTargetingOption = selectedMultiTarget;
        RemoveTargetButtons();
        targetButtons.Clear();
        turn.ActionDoneOnTarget();
        targetsParent.SetActive(false);
        turn.PassTurn();
    }

    public void RemoveTargetButtons()
    {
        foreach (TargetButton targetButton in targetButtons) GameObject.Destroy(targetButton.gameObject);
    }

    public void TurnAllOff()
    {
        targetsParent.SetActive(false);
    }

    public void BackToAction()
    {
        TurnAllOff();
        RemoveTargetButtons();
        ActionPopulate();
    }
}
