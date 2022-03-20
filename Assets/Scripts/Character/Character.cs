using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Action<bool> passTurn;
    [SerializeField] private CharacterUI characterUI;
    private Turn turn;
    public CharacterStats characterStats;
    public Action[] characterActions;
    public bool isAI;
    [HideInInspector] public bool passageDone;
    [HideInInspector] public bool isLocked;
    [HideInInspector] public bool incapacitated;

    private void Awake()
    {
        turn = FindObjectOfType<Turn>();
    }

    private void Start()
    {
        characterStats.SetStatsStartup();
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
        characterUI.UpdateBar(characterUI.MGBar, characterStats.MGCurrentValue, characterStats.MGMaxLimit);
        characterUI.UpdateBar(characterUI.staminaBar, characterStats.currentStamina, characterStats.maxStamina);
        characterUI.UpdateBar(characterUI.mentalBar, characterStats.currentMental, characterStats.maxMental);
    }
}
