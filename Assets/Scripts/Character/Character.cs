using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterUI characterUI;
    private Turn turn;
    public GameObject turnIndicator;
    [SerializeField] private GameObject perditionSymbol;
    [SerializeField] private GameObject deathSymbol;
    public CharacterData characterData;
    [HideInInspector] public bool Dead { get; private set; }
    [HideInInspector] public bool perdition;
    [HideInInspector] public bool fullGoodAI;
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
        deathSymbol.SetActive(false);
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

    public void SetDead()
    {
        Dead = true;
        perditionSymbol.SetActive(false);
        deathSymbol.SetActive(true);
        turn.turnOrder.turnOrder.Remove(this);
    }

    private void TurnCheck()
    {
        if (characterData.characterStats.currentHP <= 0 && !Dead) SetDead();
        if (!isLocked && !passageDone)
        {
            if (!Dead) ThisCharacterTurn();
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
