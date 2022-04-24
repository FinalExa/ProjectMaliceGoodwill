using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Type
{
    [HideInInspector] public enum ActionType { TYPE1, TYPE2, TYPE3, TYPE4, TYPE5, TYPE6 };
    public ActionType actionType;
    [HideInInspector] public enum ActionOpinion { SEVERELY_NEGATIVE, NEGATIVE, NEUTRAL, POSITIVE, SEVERELY_POSITIVE };
}
