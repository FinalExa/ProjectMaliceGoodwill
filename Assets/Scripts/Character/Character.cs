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
    [HideInInspector] public bool hasToPassTurn;
    [HideInInspector] public bool isShieldedFromDamage;
    [HideInInspector] public bool isShieldedFromEverything;
    [HideInInspector] public bool isShieldedFromSeverity;
    [HideInInspector] public float BGMultiplier;
    [HideInInspector] public float HPMultiplier;

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
            if (hasToPassTurn) CharacterEndTurn();
            else
            {
                if (characterData.characterStats.currentHP <= 0) SetDead();
                if (!Dead) ThisCharacterTurn();
                else CharacterEndTurn();
            }
        }
    }

    private void CharacterEndTurn()
    {
        hasToPassTurn = false;
        turn.ContinueTurn();
        turn.PassTurn();
    }

    private void ThisCharacterTurn()
    {
        ValueChangeSensitivity();
        turn.currentCharacter = this;
        passageDone = true;
        turnIndicator.SetActive(true);
    }

    private void ValueChangeSensitivity()
    {
        float hpMultiplier = 1f;
        float bgMultiplier = 1f;
        foreach (Effect effect in appliedEffects)
        {
            if (effect.effectData.setsSensitivity)
            {
                hpMultiplier *= effect.effectData.HPValueChangeSensitivity / 100f;
                bgMultiplier *= effect.effectData.BGValueChangeSensitivity / 100f;
            }
        }
        HPMultiplier = hpMultiplier;
        BGMultiplier = bgMultiplier;
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
            foreach (Character enemy in turn.turnOrder.enemyCharacters) thisCharacterEnemies.Add(enemy);
        }
        else
        {
            foreach (Character ally in turn.turnOrder.enemyCharacters) if (ally != this) thisCharacterAllies.Add(ally);
            foreach (Character enemy in turn.turnOrder.playableCharacters) thisCharacterEnemies.Add(enemy);
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
