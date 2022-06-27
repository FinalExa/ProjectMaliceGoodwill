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
    public void UpdateValues(Character target, Character sender, Action chosenAction, bool isSpectator, bool senderIncluded)
    {
        CharacterStats targetInfo = target.characterData.characterStats;
        float coeff = CalculateCoeff(target, chosenAction.type.actionType);
        if (!isSpectator)
        {
            UpdateCharacterValues(target, coeff, chosenAction.severity, chosenAction.hpValueChange, true);
            if (!senderIncluded) UpdateCharacterValues(sender, coeff, chosenAction.severity, 0f, false);
            turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " receives " + chosenAction.actionName + " by " + turn.currentCharacter.characterData.characterStats.characterName + "!");
            turn.StopTurn();
        }
        else if (chosenAction.isSeen) UpdateCharacterValues(target, coeff, chosenAction.severitySpectator, 0f, false);
        if (targetInfo.currentHP <= 0) target.SetDead();
        if (targetInfo.BGCurrentValue == 0) target.EnterPerdition();
        target.UpdateAllBars();
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

    private void UpdateCharacterValues(Character target, float coeff, float SAValue, float hpValue, bool applyEffect)
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
