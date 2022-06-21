using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect : MonoBehaviour
{
    private Turn turn;
    private GameData gameData;
    private void Awake()
    {
        turn = this.gameObject.GetComponent<Turn>();
        gameData = turn.gameData;
    }
    public void UpdateValues(Character target, Action chosenAction, bool isSpectator)
    {
        CharacterStats targetInfo = target.characterData.characterStats;
        float coeff = CalculateCoeff(target, chosenAction.type.actionType);
        if (!isSpectator)
        {
            UpdateCharacterValues(target, coeff, chosenAction.severity, chosenAction.hpValueChange);
            turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " receives " + chosenAction.actionName + " by " + turn.currentCharacter.characterData.characterStats.characterName + "!");
            turn.StopTurn();
        }
        else if (chosenAction.isSeen) UpdateCharacterValues(target, coeff, chosenAction.severitySpectator, 0f);
        if (targetInfo.maxHP <= 0) target.incapacitated = true;
        if (targetInfo.BGCurrentValue == 0) target.EnterPerdition();
    }

    private float CalculateCoeff(Character target, Type.ActionType actionType)
    {
        float coeff = 0f;
        foreach (GameTypes gameTypes in gameData.gameTypes)
        {
            if (gameTypes.actionType == actionType)
            {
                return gameTypes.actionTypeCoefficient;
            }
        }
        return coeff;
    }

    private void UpdateCharacterValues(Character target, float coeff, float SAValue, float hpValue)
    {
        CharacterStats targetStats = target.characterData.characterStats;
        if (!target.perdition)
        {
            targetStats.BGCurrentValue += SAValue * coeff;
            targetStats.BGCurrentValue = Mathf.Clamp(targetStats.BGCurrentValue, gameData.BGMinValue, gameData.BGMaxValue);
        }
        targetStats.currentHP += hpValue;
        targetStats.currentHP = Mathf.Clamp(targetStats.currentHP, 0, targetStats.maxHP);
    }
}
