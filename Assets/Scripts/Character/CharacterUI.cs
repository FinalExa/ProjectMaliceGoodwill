using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    private Character character;
    [SerializeField] private GameData gameData;
    public Text thisName;
    public Slider BGBar;
    public Slider HPBar;
    public SpriteRenderer characterSprite;

    private void Awake()
    {
        character = this.gameObject.GetComponent<Character>();
    }

    private void Start()
    {
        InitializeCharacterUI();
    }

    public void UpdateBar(Slider barToUpdate, float barCurrentValue)
    {
        barToUpdate.value = barCurrentValue;
    }

    private void InitializeCharacterUI()
    {
        thisName.text = character.characterData.characterStats.characterName;
        characterSprite.color = character.characterData.characterStats.characterColor;
        BGBar.minValue = gameData.BGMinValue;
        BGBar.maxValue = gameData.BGMaxValue;
        BGBar.value = character.characterData.characterStats.BGCurrentValue;
        HPBar.minValue = 0;
        HPBar.maxValue = character.characterData.characterStats.maxHP;
        HPBar.value = character.characterData.characterStats.currentHP;
    }
}
