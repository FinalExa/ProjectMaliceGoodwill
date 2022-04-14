using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect : MonoBehaviour
{
    [SerializeField] private EndBattleConditions endBattleConditions;
    [SerializeField] private float severelyNegativeCoeff;
    [SerializeField] private float negativeCoeff;
    [SerializeField] private float neutralCoeff;
    [SerializeField] private float positiveCoeff;
    [SerializeField] private float severelyPositiveCoeff;

    public void UpdateValues(Character target, Action chosenAction, bool isSpectator)
    {
        CharacterStats targetInfo = target.characterData.characterStats;
        float coeff = CalculateCoeff(target, chosenAction.type);
        if (!isSpectator) UpdateCharacterValues(targetInfo, coeff, chosenAction.severity, chosenAction.staminaDamage, chosenAction.mentalDamage);
        else if (chosenAction.isSeen) UpdateCharacterValues(targetInfo, coeff, chosenAction.severitySpectator, chosenAction.staminaDamageSpectator, chosenAction.mentalDamageSpectator);
        if (targetInfo.currentStamina <= 0 || targetInfo.currentMental <= 0)
        {
            target.incapacitated = true;
            endBattleConditions.CheckForVictoryConditions(target);
        }
    }

    private float CalculateCoeff(Character target, Type[] actionTypes)
    {
        float coeff = 0f;
        foreach (Type actionType in actionTypes)
        {
            coeff += EvaluateActionReaction(target.characterData, actionType.actionType);
        }
        return coeff;
    }

    private void UpdateCharacterValues(CharacterStats target, float coeff, float MGValue, float staminaValue, float mentalValue)
    {
        target.MGCurrentValue += MGValue * coeff;
        target.MGCurrentValue = Mathf.Clamp(target.MGCurrentValue, target.MGMinLimit, target.MGMaxLimit);
        target.currentStamina -= staminaValue;
        target.currentStamina = Mathf.Clamp(target.currentStamina, 0, target.maxStamina);
        target.currentMental -= mentalValue;
        target.currentMental = Mathf.Clamp(target.currentMental, 0, target.maxMental);
    }

    private float EvaluateActionReaction(CharacterData characterData, Type.ActionType actionReceivedType)
    {
        float chosenCoeff = neutralCoeff;
        foreach (CharacterOpinions opinion in characterData.characterOpinions)
        {
            if (opinion.actionType == actionReceivedType)
            {
                chosenCoeff = AssignCoeff(opinion);
                break;
            }
        }
        return chosenCoeff;
    }

    private float AssignCoeff(CharacterOpinions opinions)
    {
        float coeff;
        switch (opinions.actionTypeOpinion)
        {
            case Type.ActionOpinion.SEVERELY_NEGATIVE:
                coeff = severelyNegativeCoeff;
                break;
            case Type.ActionOpinion.NEGATIVE:
                coeff = negativeCoeff;
                break;
            case Type.ActionOpinion.NEUTRAL:
                coeff = neutralCoeff;
                break;
            case Type.ActionOpinion.POSITIVE:
                coeff = positiveCoeff;
                break;
            case Type.ActionOpinion.SEVERELY_POSITIVE:
                coeff = severelyPositiveCoeff;
                break;
            default:
                coeff = neutralCoeff;
                break;
        }
        return coeff;
    }
}
