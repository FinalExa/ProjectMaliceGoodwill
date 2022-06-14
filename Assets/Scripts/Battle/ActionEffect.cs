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
            UpdateCharacterValues(targetInfo, coeff, chosenAction.severity, chosenAction.staminaValueChange, chosenAction.mentalValueChange);
            print(target.characterData.characterStats.characterName + " receives " + chosenAction.actionName + "!");
        }
        else if (chosenAction.isSeen) UpdateCharacterValues(targetInfo, coeff, chosenAction.severitySpectator, 0f, 0f);
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

    private void UpdateCharacterValues(CharacterStats target, float coeff, float SAValue, float staminaValue, float mentalValue)
    {
        target.SACurrentValue += SAValue * coeff;
        target.SACurrentValue = Mathf.Clamp(target.SACurrentValue, gameData.SAMinValue, gameData.SAMaxValue);
        target.currentStamina += staminaValue;
        target.currentStamina = Mathf.Clamp(target.currentStamina, 0, target.maxStamina);
        target.currentMental += mentalValue;
        target.currentMental = Mathf.Clamp(target.currentMental, 0, target.maxMental);
    }
}
