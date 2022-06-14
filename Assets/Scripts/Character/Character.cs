using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterUI characterUI;
    private Turn turn;
    public CharacterData characterData;
    [HideInInspector] public bool incapacitated;
    [HideInInspector] public bool perdition;
    [HideInInspector] public bool fullGoodwillAI;
    [HideInInspector] public bool passageDone;
    [HideInInspector] public bool isLocked;

    private void Awake()
    {
        turn = FindObjectOfType<Turn>();
        characterData.thisCharacter = this;
    }

    private void Start()
    {
        characterData.characterStats.SetStatsStartup();
        UpdateAllBars();
    }

    private void Update()
    {
        TurnCheck();
        PerditionStopCheck();
    }

    private void TurnCheck()
    {
        if (!isLocked && !passageDone)
        {
            if (!incapacitated) ThisCharacterTurn();
            else turn.PassTurn();
        }
    }

    private void PerditionStopCheck()
    {
        if (perdition && characterData.characterStats.SACurrentValue > 0)
        {
            perdition = false;
        }
    }

    private void ThisCharacterTurn()
    {
        turn.currentCharacter = this;
        passageDone = true;
    }

    public void UpdateAllBars()
    {
        characterUI.UpdateBar(characterUI.MGBar, characterData.characterStats.SACurrentValue, turn.gameData.SAMaxValue);
        characterUI.UpdateBar(characterUI.staminaBar, characterData.characterStats.currentStamina, characterData.characterStats.maxStamina);
        characterUI.UpdateBar(characterUI.mentalBar, characterData.characterStats.currentMental, characterData.characterStats.maxMental);
    }
}
