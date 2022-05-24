using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Type
{
    [HideInInspector] public enum ActionType { MALICE, GOODWILL };
    public ActionType actionType;
    [HideInInspector] public enum ActionOpinion { NEGATIVE, POSITIVE };
}
