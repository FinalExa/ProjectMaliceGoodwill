using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 2)]
public class CharacterData : ScriptableObject
{
    public CharacterStats characterStats;
    [System.Serializable]
    public struct CharacterActions
    {
        public Action action;
        public int SAMinValue;
        public int SAMaxValue;
    }
    public CharacterActions[] characterActions;
    [Header("AI Section")]
    public bool isAI;
    public CharacterData[] AITargetPreference;
    public AIActionSequences[] actionSequences;
    [HideInInspector] public Character thisCharacter;
    [Header("Generate First Time Section - DO NOT CLICK OTHERWISE")]
    public bool generateCharacterData;
}