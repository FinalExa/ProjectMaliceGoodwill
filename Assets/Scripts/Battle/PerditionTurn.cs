using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerditionTurn : MonoBehaviour
{
    [SerializeField] private Turn turn;
    private Character curCharacter;
    private List<Action> availableActions;
    private Action chosenAction;
    private List<Character> availableTargets;
    private Character chosenTarget;
    private void Start()
    {
        availableActions = new List<Action>();
        availableTargets = new List<Character>();
    }

    public void PerditionStartup(Character thisCharacter)
    {
        curCharacter = thisCharacter;
        print(curCharacter.characterData.characterStats.characterName + " enters perdition!");
        GetPossibleActions();
        chosenAction = availableActions[RandomizeIndex(availableActions.Count)];
        GetPossibleTargets();
        chosenTarget = availableTargets[RandomizeIndex(availableTargets.Count)];
        turn.chosenAction = chosenAction;
        turn.target = chosenTarget;
        turn.ActionDoneOnTarget();
        curCharacter.PassTurn();
    }

    private void GetPossibleActions()
    {
        availableActions.Clear();
        foreach (Action action in curCharacter.characterData.characterActions)
        {
            if (action.mgMinRange == 0 && !action.isMagic) availableActions.Add(action);
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
