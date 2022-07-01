using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIActionSequences
{
    public float MGMinRange;
    public float MGMaxRange;
    public ActionOrderRange[] actionOrderRange;
    [HideInInspector] public int actionOrderIndex;
}
[System.Serializable]
public class ActionOrderRange
{
    public Action actionToDo;
    public enum ActionGroupTargeted { None, Enemies, Allies, Party, Others, Everyone }
    public ActionGroupTargeted actionGroupTargeted;
}
