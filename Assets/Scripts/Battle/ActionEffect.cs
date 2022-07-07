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
            if (!senderIncluded && target == sender) EffectsCheck(sender, sender, chosenAction, coeff, chosenAction.severity, 0f);
            else if (target != sender || (senderIncluded && target == sender))
            {
                EffectsCheck(target, sender, chosenAction, coeff, chosenAction.severity, chosenAction.hpValueChange);
            }
            if (!chosenAction.targetsGroups) turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " receives " + chosenAction.actionName + " from " + turn.currentCharacter.characterData.characterStats.characterName + "!");
            else turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " uses " + chosenAction.actionName + " on " + groupAttackName);
            turn.StopTurn();
        }
        else if (chosenAction.isSeen) EffectsCheck(target, sender, chosenAction, coeff, chosenAction.severitySpectator, 0f);
        if (targetInfo.currentHP <= 0) target.SetDead();
        if (targetInfo.BGCurrentValue == 0) target.EnterPerdition();
        else if (targetInfo.BGCurrentValue == gameData.BGMaxValue) target.CheckGood();
        target.UpdateAllBars();
    }

    private void TargetEffectRollAndAdd(Character target, Character sender, Action chosenAction)
    {
        EffectData data = chosenAction.actionEffect;
        bool check = false;
        if (data.givesDamageBarrier || data.givesGlobalBarrier)
        {
            foreach (Effect effect in target.appliedEffects)
            {
                if (effect.effectData.givesDamageBarrier || effect.effectData.givesGlobalBarrier)
                {
                    check = true;
                    break;
                }
            }
        }
        if (!check)
        {
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
                Effect effectToAdd = new Effect(this, data, target, sender, data.instantaneousEffect, turns, duration, chosenAction.type.actionType, chosenAction);
                target.appliedEffects.Add(effectToAdd);
            }
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

    public void EffectsCheck(Character target, Character sender, Action chosenAction, float coeff, float BGValue, float HPValue)
    {

        foreach (Effect effect in target.appliedEffects)
        {
            if (!target.isShieldedFromDamage || !target.isShieldedFromEverything)
            {
                if (effect.effectData.givesDamageBarrier)
                {
                    if (HPValue < 0) HPValue = 0f;
                    if (target.isShieldedFromSeverity && coeff < 0) BGValue = 0f;
                }
                else if (effect.effectData.givesGlobalBarrier)
                {
                    HPValue = 0f;
                    if (target.isShieldedFromSeverity) BGValue = 0f;
                }
                break;
            }
        }
        UpdateCharacterValues(target, sender, chosenAction, coeff, BGValue, HPValue);
        foreach (Effect effect in target.appliedEffects)
        {
            if (effect.effectData.effectTimeDecreasesOnInteraction) effect.DecreaseEffectTime();
        }
    }

    public void UpdateCharacterValues(Character target, Character sender, Action chosenAction, float coeff, float BGValue, float HPValue)
    {
        CharacterStats targetStats = target.characterData.characterStats;
        if (!target.perdition)
        {
            targetStats.BGCurrentValue += BGValue * coeff * target.BGMultiplier;
            targetStats.BGCurrentValue = Mathf.Clamp(targetStats.BGCurrentValue, gameData.BGMinValue, gameData.BGMaxValue);
        }
        targetStats.currentHP += HPValue * target.HPMultiplier;
        targetStats.currentHP = Mathf.Clamp(targetStats.currentHP, 0, targetStats.maxHP);
        if (chosenAction.hasEffect) TargetEffectRollAndAdd(target, sender, chosenAction);
    }
}
