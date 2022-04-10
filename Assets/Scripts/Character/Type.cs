using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Type
{
    [HideInInspector] public enum ActionType { VIOLENCE, PROTECTION, MANIPULATION, MOCKERY, CHARITY };
    public ActionType actionType;
    [HideInInspector] public enum ActionOpinion { SEVERELY_NEGATIVE, NEGATIVE, NEUTRAL, POSITIVE, SEVERELY_POSITIVE };
}
