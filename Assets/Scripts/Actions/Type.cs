using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Type
{
    [HideInInspector] public enum ActionType { SELFISHNESS, ALTRUISM };
    public ActionType actionType;
}
