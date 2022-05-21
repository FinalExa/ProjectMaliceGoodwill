using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 2)]
public class CharacterData : ScriptableObject
{
    public CharacterStats characterStats;
    public CharacterOpinions[] characterOpinions;
    public Type.Intention[] characterIntentions;
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
            List<Type.Intention> intentions = new List<Type.Intention>();
            List<CharacterOpinions> opinions = new List<CharacterOpinions>();
            Turn turn = FindObjectOfType<Turn>();
            for (int i = 0; i < turn.intentions.Length; i++)
            {
                CharacterOpinions opinion = new CharacterOpinions();
                opinion.actionType = turn.intentions[i].intention;
                opinion.actionTypeOpinion = Type.ActionOpinion.NEUTRAL;
                opinions.Add(opinion);
                intentions.Add(turn.intentions[i]);
            }
            characterOpinions = opinions.ToArray();
            characterIntentions = intentions.ToArray();
            generateCharacterData = false;
        }
    }
}