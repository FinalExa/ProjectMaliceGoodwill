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
    [HideInInspector] public Character thisCharacter;
    [Header("Generate First Time Section - DO NOT CLICK OTHERWISE")]
    public bool generateCharacterData;

    private void OnValidate()
    {
        if (generateCharacterData)
        {
            List<CharacterOpinions> opinions = new List<CharacterOpinions>();
            Turn turn = FindObjectOfType<Turn>();
            for (int i = 0; i < turn.types.Length; i++)
            {
                CharacterOpinions opinion = new CharacterOpinions();
                opinion.actionType = turn.types[i];
                opinion.actionTypeOpinion = Type.ActionOpinion.NEGATIVE;
                opinions.Add(opinion);
            }
            characterOpinions = opinions.ToArray();
            generateCharacterData = false;
        }
    }
}