using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AITurnData
{
    public string AIName;
    public int AIActionIndex;
    public List<CharacterData> targetOrder;
    public AIActionSequences[] sequence;
}
