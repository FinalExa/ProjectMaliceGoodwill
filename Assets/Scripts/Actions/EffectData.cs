using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObjects/EffectData", order = 3)]
public class EffectData : ScriptableObject
{
    public string effectName;
    public string effectDescription;
    public float effectTriggerChance;
    public bool instantaneousEffect;
    public bool effectOverTime;
    public float effectTurns;
    public bool effectTimeDecreasesOnDamage;
    public bool effectTimeDecreasesOnInteraction;
    [Header("Value change section")]
    public bool changesValuesOnTarget;
    public float HPValueChange;
    public float BGValueChange;
    public bool setsSensitivity;
    public float HPValueChangeSensitivity;
    public float BGValueChangeSensitivity;
    public bool changesValuesOnSender;
    public float HPValueChangeSender;
    public float BGValueChangeSender;
    [Header("Effect")]
    public bool inflictsStun;
    public bool givesDamageBarrier;
    public bool givesGlobalBarrier;
    public bool barrierProtectsFromBGChange;

    private void OnValidate()
    {
        effectName = name;
    }
}
