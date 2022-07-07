using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 2)]
public class CharacterData : ScriptableObject
{
    public CharacterStats characterStats;
    public Trait.CharacterTrait[] characterTraits;
    [System.Serializable]
    public struct CharacterActions
    {
        public Action action;
        public int BGMinValue;
        public int BGMaxValue;
    }
    public CharacterActions[] characterActions;
    [Header("AI Section")]
    public bool isAI;
    public AIActionSequences[] actionSequences;
    [HideInInspector] public Character thisCharacter;
    [Header("Generate Character Moves")]
    public GameData gameData;
    public bool generateCharacterActions;

    private void OnValidate()
    {
        characterStats.characterName = name;
        if (generateCharacterActions)
        {
            List<CharacterActions> characterActionsList = new List<CharacterActions>();
            characterActionsList.Clear();
            foreach (Trait characterTrait in gameData.traits)
            {
                for (int i = 0; i < characterTraits.Length; i++)
                {
                    if (characterTrait.characterTrait == characterTraits[i])
                    {
                        foreach (Action action in characterTrait.traitActions)
                        {
                            CharacterActions characterActions = new CharacterActions();
                            characterActions.action = action;
                            characterActions.BGMaxValue = gameData.BGMaxValue;
                            characterActionsList.Add(characterActions);
                        }
                        break;
                    }
                }
            }
            characterActions = characterActionsList.ToArray();
            generateCharacterActions = false;
        }
    }
}