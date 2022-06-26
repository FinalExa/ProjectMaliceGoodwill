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
    [HideInInspector] public List<Character> thisCharacterAllies;
    [HideInInspector] public List<Character> thisCharacterEnemies;
    [HideInInspector] public List<Effect> appliedEffects;

    private void Awake()
    {
        turn = FindObjectOfType<Turn>();
        characterData.thisCharacter = this;
    }

    private void Start()
    {
        characterData.characterStats.SetStatsStartup();
        CharacterSymbolsOff();
        CreateThisCharacterLists();
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
        deathSymbol.SetActive(true);
        turn.turnOrder.turnOrder.Remove(this);
        if (!characterData.isAI) turn.turnOrder.playableCharacters.Remove(this);
        else turn.turnOrder.playableCharacters.Remove(this);
    }

    private void TurnCheck()
    {
        if (Dead && perditionSymbol.activeSelf) perditionSymbol.SetActive(false);
        if (!isLocked && !passageDone)
        {
            CharacterApplyEffects();
            if (!Dead)
            {
                if (characterData.characterStats.currentHP <= 0) SetDead();
                else ThisCharacterTurn();
            }
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

    private void CharacterSymbolsOff()
    {
        turnIndicator.SetActive(false);
        perditionSymbol.SetActive(false);
        deathSymbol.SetActive(false);
    }

    private void CreateThisCharacterLists()
    {
        thisCharacterAllies = new List<Character>();
        thisCharacterAllies.Clear();
        thisCharacterEnemies = new List<Character>();
        thisCharacterEnemies.Clear();
        if (!characterData.isAI)
        {
            foreach (Character ally in turn.turnOrder.playableCharacters) if (ally != this) thisCharacterAllies.Add(ally);
            foreach (Character enemy in turn.turnOrder.enemyCharacters) thisCharacterAllies.Add(enemy);
        }
        else
        {
            foreach (Character ally in turn.turnOrder.enemyCharacters) if (ally != this) thisCharacterAllies.Add(ally);
            foreach (Character enemy in turn.turnOrder.playableCharacters) thisCharacterAllies.Add(enemy);
        }
        appliedEffects = new List<Effect>();
        appliedEffects.Clear();
    }

    private void CharacterApplyEffects()
    {
        if (!Dead)
        {
            foreach (Effect effect in appliedEffects)
            {
                effect.ExecuteEffect();
                if (effect.canBeRemoved) appliedEffects.Remove(effect);
            }
        }
    }
}
