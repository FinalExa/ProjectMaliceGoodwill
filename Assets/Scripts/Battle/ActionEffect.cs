using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect : MonoBehaviour
{
    private Turn turn;
    private GameData gameData;
    [HideInInspector] public string groupAttackName;
    private void Awake()
    {
        turn = this.gameObject.GetComponent<Turn>();
        gameData = turn.gameData;
    }

    public void UpdateValues(Character target, Character sender, Action chosenAction, bool isSpectator, bool senderIncluded)
    {
        CharacterStats targetInfo = target.characterData.characterStats;
        float coeff = CalculateCoeff(chosenAction.type.actionType);
        if (!isSpectator)
        {
            if (!senderIncluded && target == sender) UpdateCharacterValues(sender, coeff, chosenAction.severity, 0f);
            else if (target != sender || (senderIncluded && target == sender))
            {
                UpdateCharacterValues(target, coeff, chosenAction.severity, chosenAction.hpValueChange);
                if (chosenAction.hasEffect) TargetEffectRollAndAdd(target, sender, chosenAction);
            }
            if (!chosenAction.targetsGroups) turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " receives " + chosenAction.actionName + " from " + turn.currentCharacter.characterData.characterStats.characterName + "!");
            else turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " uses " + chosenAction.actionName + " on " + groupAttackName);
            turn.StopTurn();
        }
        else if (chosenAction.isSeen) UpdateCharacterValues(target, coeff, chosenAction.severitySpectator, 0f);
        if (targetInfo.currentHP <= 0) target.SetDead();
        if (targetInfo.BGCurrentValue == 0) target.EnterPerdition();
        target.UpdateAllBars();
    }

    private void TargetEffectRollAndAdd(Character target, Character sender, Action chosenAction)
    {
        EffectData data = chosenAction.actionEffect;
        float roll = Random.Range(1f, 100f);
        if (roll <= data.effectTriggerChance)
        {
            bool turns = false;
            float duration = 0f;
            if (data.effectOverTime || data.effectTimeDecreasesOnDamage || data.effectTimeDecreasesOnInteraction)
            {
                turns = true;
                duration = data.effectTurns;
            }
            Effect effectToAdd = new Effect(this, data, target, sender, data.instantaneousEffect, turns, duration, chosenAction.type.actionType);
            target.appliedEffects.Add(effectToAdd);
        }
    }

    public float CalculateCoeff(Type.ActionType actionType)
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

    public void UpdateCharacterValues(Character target, float coeff, float BGValue, float hpValue)
    {

        CharacterStats targetStats = target.characterData.characterStats;
        if (!target.perdition)
        {
            targetStats.BGCurrentValue += BGValue * coeff * target.BGMultiplier;
            targetStats.BGCurrentValue = Mathf.Clamp(targetStats.BGCurrentValue, gameData.BGMinValue, gameData.BGMaxValue);
        }
        targetStats.currentHP += hpValue * target.HPMultiplier;
        targetStats.currentHP = Mathf.Clamp(targetStats.currentHP, 0, targetStats.maxHP);
    }
}
