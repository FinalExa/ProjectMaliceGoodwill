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
        Type.ActionType actionType = chosenAction.type.actionType;
        float coeff = EvaluateActionReaction(target.characterData, actionType);
        if (!isSpectator)
        {
            targetInfo.MGCurrentValue += chosenAction.severity * coeff;
            targetInfo.currentStamina -= chosenAction.staminaDamage;
            targetInfo.currentMental -= chosenAction.mentalDamage;
        }
        else
        {
            targetInfo.MGCurrentValue += chosenAction.severitySpectator * coeff;
            targetInfo.currentStamina -= chosenAction.staminaDamageSpectator;
            targetInfo.currentMental -= chosenAction.mentalDamageSpectator;
        }
        if (targetInfo.currentStamina <= 0 || targetInfo.currentMental <= 0)
        {
            target.incapacitated = true;
            endBattleConditions.CheckForVictoryConditions(target);
        }
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
