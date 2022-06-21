using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterUI characterUI;
    private Turn turn;
    public GameObject turnIndicator;
    public GameObject perditionSymbol;
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
        perditionSymbol.SetActive(false);
        characterData.characterStats.SetStatsStartup();
        UpdateAllBars();
    }

    private void Update()
    {
        TurnCheck();
    }

    public void EnterPerdition()
    {
        perdition = true;
        perditionSymbol.SetActive(true);
    }

    private void TurnCheck()
    {
        if (!isLocked && !passageDone)
        {
            if (!incapacitated) ThisCharacterTurn();
            else
            {
                turn.ContinueTurn();
                turn.PassTurn();
            }
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
