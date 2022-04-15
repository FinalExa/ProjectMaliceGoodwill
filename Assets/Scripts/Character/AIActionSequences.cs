using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIActionSequences
{
    public float MGMinRange;
    public float MGMaxRange;
    public Action[] actionOrderInThatRange;
    public int actionOrderIndex;
}
