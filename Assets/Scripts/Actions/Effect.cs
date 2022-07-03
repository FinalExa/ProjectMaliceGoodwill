using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public Effect(ActionEffect actionEffectReference, EffectData data, Character effectTarget, Character effectSender, bool instantaneous, bool overTime, float duration, Type.ActionType actionType)
    {
        actionEffect = actionEffectReference;
        effectData = data;
        target = effectTarget;
        sender = effectSender;
        effectType = actionType;
        if (instantaneous) ExecuteEffect();
        if (overTime) timeLeft = duration;
        if (data.givesBarrier) target.isShielded = true;
        else RemoveEffect();
    }
    public EffectData effectData;
    public bool canBeRemoved;
    public float timeLeft;
    public Type.ActionType effectType;
    private ActionEffect actionEffect;
    private Character target;
    private Character sender;
    public void ExecuteEffect()
    {
        if (effectData.inflictsStun) target.hasToPassTurn = true;
        if (effectData.changesValuesOnTarget)
        {
            float coeff = actionEffect.CalculateCoeff(effectType);
            actionEffect.UpdateCharacterValues(target, coeff, effectData.BGValueChange, effectData.HPValueChange);
        }
        if (effectData.changesValuesOnSender)
        {
            float coeff = actionEffect.CalculateCoeff(effectType);
            actionEffect.UpdateCharacterValues(sender, coeff, effectData.BGValueChangeSender, effectData.HPValueChangeSender);
        }
    }

    public void DecreaseEffectTime()
    {
        timeLeft--;
        if (timeLeft <= 0) RemoveEffect();
    }

    private void RemoveEffect()
    {
        if (effectData.givesBarrier) target.isShielded = false;
        target.appliedEffects.Remove(this);
    }
}
