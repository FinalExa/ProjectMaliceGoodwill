using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/Action", order = 1)]
public class Action : ScriptableObject
{
    public string actionName;
    public Type type;
    public float severity;
    public float severitySpectator;
    public float hpValueChange;
    public bool canTargetEnemies;
    public bool canTargetAllies;
    public bool canTargetSelf;
    public bool isSeen;

    private void OnValidate()
    {
        actionName = name;
    }
}
