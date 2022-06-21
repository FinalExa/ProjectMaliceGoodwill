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

    private void Awake()
    {
        turn = this.gameObject.GetComponent<Turn>();
    }
    private void Start()
    {
        availableActions = new List<Action>();
        availableTargets = new List<Character>();
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
        GetPossibleTargets();
        chosenTarget = availableTargets[RandomizeIndex(availableTargets.Count)];
        turn.chosenAction = chosenAction;
        turn.target = chosenTarget;
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
        foreach (Character character in turn.turnOrder.turnOrder)
        {
            if (character == curCharacter && chosenAction.canTargetSelf) availableTargets.Add(character);
            if (!curCharacter.characterData.isAI)
            {
                if (character != curCharacter && !character.characterData.isAI && chosenAction.canTargetAllies) availableTargets.Add(character);
                if (character != curCharacter && character.characterData.isAI && chosenAction.canTargetEnemies) availableTargets.Add(character);
            }
            else
            {
                if (character != curCharacter && character.characterData.isAI && chosenAction.canTargetAllies) availableTargets.Add(character);
                if (character != curCharacter && !character.characterData.isAI && chosenAction.canTargetEnemies) availableTargets.Add(character);
            }
        }
    }
}
