using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterUI characterUI;
    [SerializeField] private GameObject turnIndicator;
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
        turnIndicator.SetActive(false);
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
            else
            {
                turnIndicator.SetActive(false);
                turn.ContinueTurn();
                turn.PassTurn();
            }
        }
    }

    private void PerditionStopCheck()
    {
        if (perdition && characterData.characterStats.BGCurrentValue > 0)
        {
            perdition = false;
        }
    }

    private void ThisCharacterTurn()
    {
        turn.currentCharacter = this;
        passageDone = true;
        turnIndicator.SetActive(true);
    }

    public void UpdateAllBars()
    {
        characterUI.UpdateBar(characterUI.BGBar, characterData.characterStats.BGCurrentValue);
        characterUI.UpdateBar(characterUI.HPBar, characterData.characterStats.currentHP);
    }
}
