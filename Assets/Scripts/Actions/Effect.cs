using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public Effect(ActionEffect actionEffectReference, EffectData data, Character effectTarget, Character effectSender, bool overTime, float duration, Action actionOrigin)
    {
        canBeRemoved = false;
        actionEffect = actionEffectReference;
        effectData = data;
        target = effectTarget;
        sender = effectSender;
        effectType = actionOrigin.type.actionType;
        origin = actionOrigin;
        if (data.givesDamageBarrier) target.isShieldedFromDamage = true;
        else if (data.givesGlobalBarrier) target.isShieldedFromEverything = true;
        if (data.barrierProtectsFromBGChange) target.isShieldedFromSeverity = true;
        if (overTime) timeLeft = duration;
        else timeLeft = 0;
    }
    public EffectData effectData;
    public bool canBeRemoved;
    public float timeLeft;
    public Type.ActionType effectType;
    public ActionEffect actionEffect;
    public Character target;
    public Character sender;
    public Action origin;
    public void ExecuteEffect()
    {
        if (effectData.inflictsStun) target.hasToPassTurn = true;
        if (effectData.changesValuesOnTarget)
        {
            float coeff = actionEffect.CalculateCoeff(effectType);
            actionEffect.UpdateCharacterValues(target, sender, origin, coeff, effectData.BGValueChange, effectData.HPValueChange, true);
        }
        if (effectData.changesValuesOnSender)
        {
            float coeff = actionEffect.CalculateCoeff(effectType);
            actionEffect.UpdateCharacterValues(sender, sender, origin, coeff, effectData.BGValueChangeSender, effectData.HPValueChangeSender, true);
        }
        if ((effectData.effectOverTime && (!effectData.effectTimeDecreasesOnDamage && !effectData.effectTimeDecreasesOnInteraction)) || effectData.instantaneousEffect) DecreaseEffectTime();
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
        if (effectData.effectOverTime) canBeRemoved = true;
    }

    public void ShieldEnded()
    {
        if (effectData.givesDamageBarrier) target.isShieldedFromDamage = false;
        else if (effectData.givesGlobalBarrier) target.isShieldedFromEverything = false;
        if (effectData.barrierProtectsFromBGChange) target.isShieldedFromSeverity = false;
    }
}
