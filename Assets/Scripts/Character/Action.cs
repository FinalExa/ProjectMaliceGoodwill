using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action", order = 1)]
public class Action : ScriptableObject
{
    public Type type;
    public float severity;
    public float severitySpectator;
    public float staminaDamage;
    public float staminaDamageSpectator;
    public float mentalDamage;
    public float mentalDamageSpectator;
    public float MGRangeMin;
    public float MGRangeMax;
}
