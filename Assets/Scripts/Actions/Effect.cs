using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public Effect(ActionEffect actionEffectReference, EffectData data, Character effectTarget, Character effectSender, bool instantaneous, bool overTime, float duration, Type.ActionType actionType, Action actionOrigin)
    {
        actionEffect = actionEffectReference;
        effectData = data;
        target = effectTarget;
        sender = effectSender;
        effectType = actionType;
        origin = actionOrigin;
        if (instantaneous) ExecuteEffect();
        if (data.givesDamageBarrier) target.isShieldedFromDamage = true;
        else if (data.givesGlobalBarrier) target.isShieldedFromEverything = true;
        if (data.barrierProtectsFromBGChange) target.isShieldedFromSeverity = true;
        if (overTime) timeLeft = duration;
        else RemoveEffect();
    }
    public EffectData effectData;
    public bool canBeRemoved;
    public float timeLeft;
    public Type.ActionType effectType;
    private ActionEffect actionEffect;
    private Character target;
    private Character sender;
    private Action origin;
    public void ExecuteEffect()
    {
        if (effectData.inflictsStun) target.hasToPassTurn = true;
        if (effectData.changesValuesOnTarget)
        {
            float coeff = actionEffect.CalculateCoeff(effectType);
            actionEffect.UpdateCharacterValues(target, sender, origin, coeff, effectData.BGValueChange, effectData.HPValueChange);
        }
        if (effectData.changesValuesOnSender)
        {
            float coeff = actionEffect.CalculateCoeff(effectType);
            actionEffect.UpdateCharacterValues(target, sender, origin, coeff, effectData.BGValueChangeSender, effectData.HPValueChangeSender);
        }
        if (effectData.effectOverTime) DecreaseEffectTime();
    }

    public void DecreaseEffectTime()
    {
        timeLeft--;
        if (timeLeft <= 0)
        {
            ShieldEnded();
            RemoveEffect();
        }
    }

    private void RemoveEffect()
    {
        if (effectData.givesDamageBarrier) target.isShieldedFromDamage = false;
        if (effectData.givesGlobalBarrier) target.isShieldedFromEverything = false;
        target.appliedEffects.Remove(this);
    }

    public void ShieldEnded()
    {
        if (effectData.givesDamageBarrier)
        {

        }
    }
}
