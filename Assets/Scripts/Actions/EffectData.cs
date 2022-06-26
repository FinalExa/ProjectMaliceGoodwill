using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObjects/EffectData", order = 3)]
public class EffectData : ScriptableObject
{
    public string effectName;
    public bool effectTriggerChance;
    public bool instantaneousEffect;
    public bool effectOverTime;
    public bool effectOverTimeTurns;
    [Header("Value change section")]
    public float HPValueChange;
    public float HPValueChangeSender;
    public float HPValueChangeSensitivity;
    public float BGValueChange;
    public float BGValueChangeSender;
    public float BGValueChangeSensitivity;
    [Header("Effect")]
    public bool stuns;
    public bool barrier;
    public bool barrierHasHits;
    public float barrierHits;
}
