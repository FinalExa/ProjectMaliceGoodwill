using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerditionTurn : MonoBehaviour
{
    private Turn turn;
    private Character curCharacter;
    private List<Action> availableActions;
    private Action chosenAction;
    private List<Character> availableTargets;
    private Character chosenTarget;
    private List<string> chosenTargets;
    private string chosenGroupTarget;

    private void Awake()
    {
        turn = this.gameObject.GetComponent<Turn>();
    }
    private void Start()
    {
        availableActions = new List<Action>();
        availableTargets = new List<Character>();
    }

    private void Update()
    {
        if (curCharacter != null && curCharacter.characterData.characterStats.currentHP <= 0) curCharacter.SetDead();
    }

    public void PerditionStartup(Character thisCharacter)
    {
        curCharacter = thisCharacter;
        turn.battleText.UpdateBattleText(curCharacter.characterData.characterStats.characterName + " enters perdition!");
        turn.StopTurn(this);
    }

    public void PerditionContinueTurn()
    {
        GetPossibleActions();
        chosenAction = availableActions[RandomizeIndex(availableActions.Count)];
        turn.chosenAction = chosenAction;
        if (!chosenAction.targetsGroups)
        {
            GetPossibleTargets();
            chosenTarget = availableTargets[RandomizeIndex(availableTargets.Count)];
            turn.target = chosenTarget;
        }
        else
        {
            GetPossibleMultiTargets();
            chosenGroupTarget = chosenTargets[RandomizeIndex(chosenTargets.Count)];
            turn.multiTargetingOption = chosenGroupTarget;
        }
        turn.ActionDoneOnTarget();
        turn.PassTurn();
    }

    private void GetPossibleActions()
    {
        availableActions.Clear();
        foreach (CharacterData.CharacterActions characterAction in curCharacter.characterData.characterActions)
        {
            if (characterAction.BGMinValue == 0) availableActions.Add(characterAction.action);
        }
    }

    private int RandomizeIndex(int maxNumber)
    {
        int randomIndexValue = Random.Range(0, maxNumber);
        return randomIndexValue;
    }

    private void GetPossibleTargets()
    {
        availableTargets.Clear();
        foreach (Character character in turn.turnOrder.characterOrder)
        {
            if (character == curCharacter && chosenAction.canTargetSelf) availableTargets.Add(character);
            if (character != curCharacter && chosenAction.canTargetOthers) availableTargets.Add(character);
        }
    }
    private void GetPossibleMultiTargets()
    {
        chosenTargets.Clear();
        if (turn.chosenAction.hitsEveryone)
        {
            chosenTargets.Add("Everyone");
            return;
        }
        if (turn.chosenAction.hitsAllOthers)
        {
            chosenTargets.Add("Others");
            return;
        }
        if (turn.chosenAction.hitsEnemyGroup) chosenTargets.Add("Enemies");
        if (turn.chosenAction.hitsAllyGroupSelfExcluded)
        {
            chosenTargets.Add("Allies");
            return;
        }
        if (turn.chosenAction.hitsAllyGroupSelfIncluded)
        {
            chosenTargets.Add("Party");
            return;
        }
    }
}
