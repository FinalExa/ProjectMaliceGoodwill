using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private EndBattleConditions endBattleConditions;
    [SerializeField] private float negativeCoeff;
    [SerializeField] private float positiveCoeff;
    private void Awake()
    {
        gameData = FindObjectOfType<Turn>().gameData;
    }
    public void UpdateValues(Character target, Action chosenAction, bool isSpectator)
    {
        CharacterStats targetInfo = target.characterData.characterStats;
        float coeff = CalculateCoeff(target, chosenAction.type.actionType);
        if (!isSpectator)
        {
            UpdateCharacterValues(target, coeff, chosenAction.severity, chosenAction.staminaValueChange, chosenAction.mentalValueChange);
            print(target.characterData.characterStats.characterName + " receives " + chosenAction.actionName + "!");
        }
        else if (chosenAction.isSeen) UpdateCharacterValues(target, coeff, chosenAction.severitySpectator, 0f, 0f);
        if (targetInfo.currentStamina <= 0 || targetInfo.currentMental <= 0) target.incapacitated = true;
        if (targetInfo.SACurrentValue == 0) target.perdition = true;
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

    private void UpdateCharacterValues(Character target, float coeff, float SAValue, float staminaValue, float mentalValue)
    {
        CharacterStats targetStats = target.characterData.characterStats;
        if (!target.perdition)
        {
            targetStats.SACurrentValue += SAValue * coeff;
            targetStats.SACurrentValue = Mathf.Clamp(targetStats.SACurrentValue, gameData.SAMinValue, gameData.SAMaxValue);
        }
        targetStats.currentStamina += staminaValue;
        targetStats.currentStamina = Mathf.Clamp(targetStats.currentStamina, 0, targetStats.maxStamina);
        targetStats.currentMental += mentalValue;
        targetStats.currentMental = Mathf.Clamp(targetStats.currentMental, 0, targetStats.maxMental);
    }
}
