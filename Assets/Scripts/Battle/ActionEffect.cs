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
            if (!senderIncluded && target == sender)
            {
                print("there");
                EffectsCheck(sender, sender, chosenAction, coeff, chosenAction.severity, 0f, isSpectator);
            }
            else if (target != sender || (senderIncluded && target == sender)) EffectsCheck(target, sender, chosenAction, coeff, chosenAction.severity, chosenAction.hpValueChange, isSpectator);
            if (!chosenAction.targetsGroups) turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " receives " + chosenAction.actionName + " from " + turn.currentCharacter.characterData.characterStats.characterName + "!");
            else turn.battleText.UpdateBattleText(target.characterData.characterStats.characterName + " uses " + chosenAction.actionName + " on " + groupAttackName);
            turn.StopTurn();
        }
        else if (chosenAction.isSeen && isSpectator) EffectsCheck(target, sender, chosenAction, coeff, chosenAction.severitySpectator, 0f, isSpectator);
        if (targetInfo.currentHP <= 0) target.SetDead();
        if (targetInfo.BGCurrentValue == 0) target.EnterPerdition();
        else if (targetInfo.BGCurrentValue == gameData.BGMaxValue) target.CheckGood();
        target.UpdateAllBars();
    }

    private void TargetEffectRollAndAdd(Character target, Character sender, Action chosenAction)
    {
        EffectData data = chosenAction.actionEffect;
        bool check = false;
        if ((data.effectOverTime && target.overTimeEffect.effectData == null) || (data.instantaneousEffect)) check = true;
        if (check)
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
                Effect effectToAdd = new Effect(this, data, target, sender, turns, duration, chosenAction);
                if (data.effectOverTime) target.overTimeEffect = effectToAdd;
                if (effectToAdd.effectData.instantaneousEffect) effectToAdd.ExecuteEffect();
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

    public void EffectsCheck(Character target, Character sender, Action chosenAction, float coeff, float BGValue, float HPValue, bool spectator)
    {
        bool damageTaken = false;
        if (HPValue < 0f) damageTaken = true;
        Effect effectToCheck = target.overTimeEffect;
        if (effectToCheck.effectData != null)
        {
            if (effectToCheck.effectData.givesDamageBarrier || effectToCheck.effectData.givesGlobalBarrier)
            {
                if (effectToCheck.effectData.givesDamageBarrier)
                {
                    if (HPValue < 0) HPValue = 0f;
                    if (target.isShieldedFromSeverity && coeff < 0) BGValue = 0f;
                }
                else if (effectToCheck.effectData.givesGlobalBarrier)
                {
                    HPValue = 0f;
                    if (target.isShieldedFromSeverity) BGValue = 0f;
                }
            }
        }
        UpdateCharacterValues(target, sender, chosenAction, coeff, BGValue, HPValue, spectator);
        if (effectToCheck.effectData != null && !spectator &&
            (target.overTimeEffect.effectData.effectTimeDecreasesOnInteraction ||
            (target.overTimeEffect.effectData.effectTimeDecreasesOnDamage && damageTaken)))
        {
            target.overTimeEffect.ExecuteEffect();
            target.overTimeEffect.DecreaseEffectTime();
        }
    }

    public void UpdateCharacterValues(Character target, Character sender, Action chosenAction, float coeff, float BGValue, float HPValue, bool dontRollEffect)
    {
        CharacterStats targetStats = target.characterData.characterStats;
        target.ValueChangeSensitivity();
        if (!target.perdition)
        {
            targetStats.BGCurrentValue += BGValue * coeff * target.BGMultiplier;
            targetStats.BGCurrentValue = Mathf.Clamp(targetStats.BGCurrentValue, gameData.BGMinValue, gameData.BGMaxValue);
        }
        targetStats.currentHP += HPValue * target.HPMultiplier;
        targetStats.currentHP = Mathf.Clamp(targetStats.currentHP, 0, targetStats.maxHP);
        if (chosenAction.hasEffect && chosenAction.actionEffect != null && !dontRollEffect) TargetEffectRollAndAdd(target, sender, chosenAction);
        target.UpdateAllBars();
    }
}
