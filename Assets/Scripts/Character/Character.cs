using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Action<bool> passTurn;
    [SerializeField] private CharacterUI characterUI;
    private Turn turn;
    public CharacterData characterData;
    [HideInInspector] public bool incapacitated;
    [HideInInspector] public bool perdition;
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
        if (!isLocked && !passageDone)
        {
            if (!incapacitated) ThisCharacterTurn();
            else PassTurn();
        }
    }

    private void ThisCharacterTurn()
    {
        turn.currentCharacter = this;
        passageDone = true;
    }

    public void PassTurn()
    {
        isLocked = true;
        passageDone = false;
        passTurn(true);
    }

    public void UpdateAllBars()
    {
        characterUI.UpdateBar(characterUI.MGBar, characterData.characterStats.MGCurrentValue, characterData.characterStats.MGMaxLimit);
        characterUI.UpdateBar(characterUI.staminaBar, characterData.characterStats.currentStamina, characterData.characterStats.maxStamina);
        characterUI.UpdateBar(characterUI.mentalBar, characterData.characterStats.currentMental, characterData.characterStats.maxMental);
    }
}
