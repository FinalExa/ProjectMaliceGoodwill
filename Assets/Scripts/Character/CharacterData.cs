using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 2)]
public class CharacterData : ScriptableObject
{
    public CharacterStats characterStats;
    public CharacterOpinions[] characterOpinions;
    public Action[] characterActions;
    [Header("AI Section")]
    public bool isAI;
    public CharacterData[] AITargetPreference;
    public AIActionSequences[] actionSequences;
}