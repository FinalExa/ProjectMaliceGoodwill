using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateDropdowns : MonoBehaviour
{
    [SerializeField] private Turn turn;
    [SerializeField] private GameObject intentionsParent;
    [SerializeField] private Dropdown intentionsDropdown;
    [SerializeField] private GameObject actionsParent;
    [SerializeField] private Dropdown actionsDropdown;
    [SerializeField] private GameObject targetsParent;
    [SerializeField] private Dropdown targetsDropdown;

    public void IntentionsPopulate()
    {
        intentionsParent.SetActive(true);
        List<string> intentionsList = new List<string>();
        Character curCharacter = turn.currentCharacter;
        float MGValue = curCharacter.characterData.characterStats.MGCurrentValue;
        foreach (Type.Intention intention in curCharacter.characterData.characterIntentions)
        {
            if (MGValue <= intention.mgMaxRange && MGValue >= intention.mgMinRange) intentionsList.Add(intention.intention.ToString());
        }
        intentionsDropdown.ClearOptions();
        intentionsDropdown.AddOptions(intentionsList);
    }
    public void IntentionConfirm()
    {
        foreach (Type.Intention intention in turn.currentCharacter.characterData.characterIntentions)
        {
            if (intention.intention.ToString() == intentionsDropdown.options[intentionsDropdown.value].text)
            {
                turn.chosenIntention = intention;
                break;
            }
        }
        actionsParent.SetActive(false);
        ActionPopulate();
    }
    public void ActionPopulate()
    {
        actionsParent.SetActive(true);
        List<string> actionsList = new List<string>();
        Character curCharacter = turn.currentCharacter;
        Type.Intention characterIntention = turn.chosenIntention;
        foreach (Type.Intention intention in curCharacter.characterData.characterIntentions)
        {
            if (characterIntention.intention == intention.intention)
            {
                foreach (Action action in intention.intentionActions)
                {
                    actionsList.Add(action.actionName);
                }
                break;
            }
        }
        actionsDropdown.ClearOptions();
        actionsDropdown.AddOptions(actionsList);
    }

    public void ActionConfirm()
    {
        foreach (Action action in turn.chosenIntention.intentionActions)
        {
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
        if (turn.chosenAction.canTargetEnemies)
        {
            foreach (Character enemy in turn.turnOrder.enemyCharacters)
            {
                if (!enemy.incapacitated)
                {
                    turn.possibleTargets.Add(enemy);
                    possibleTargetsNames.Add(enemy.characterData.characterStats.characterName);
                }
            }
        }
        if (turn.chosenAction.canTargetAllies)
        {
            for (int i = 0; i < turn.turnOrder.playableCharacters.Length; i++)
            {
                if (turn.turnOrder.playableCharacters[i] != turn.currentCharacter && !turn.turnOrder.playableCharacters[i].incapacitated)
                {
                    turn.possibleTargets.Add(turn.turnOrder.playableCharacters[i]);
                    possibleTargetsNames.Add(turn.turnOrder.playableCharacters[i].characterData.characterStats.characterName);
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
        turn.stop = false;
        turn.currentCharacter.PassTurn();
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

    public void BackToIntention()
    {
        actionsParent.SetActive(false);
        IntentionsPopulate();
    }
}
