using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action", order = 1)]
public class Action : ScriptableObject
{
    public string actionName;
    public Type type;
    [Header("Values Change")]
    public float severity;
    public float severitySpectator;
    public float hpValueChange;
    [Header("Targeting")]
    public bool canTargetOthers;
    public bool canTargetSelf;
    public bool targetsGroups;
    public bool hitsEnemyGroup;
    public bool hitsAllyGroupSelfExcluded;
    public bool hitsAllyGroupSelfIncluded;
    public bool hitsAllOthers;
    public bool hitsEveryone;
    public bool isSeen;
    [Header("Effect")]
    public bool hasEffect;
    public EffectData actionEffect;

    private void OnValidate()
    {
        actionName = name;
    }
}
