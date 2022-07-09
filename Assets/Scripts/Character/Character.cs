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
    [SerializeField] private GameObject goodSymbol;
    public CharacterData characterData;
    [HideInInspector] public bool Dead { get; private set; }
    [HideInInspector] public bool Good { get; private set; }
    [HideInInspector] public bool perdition;
    [HideInInspector] public bool fullGoodAI;
    [HideInInspector] public bool passageDone;
    [HideInInspector] public bool isLocked;
    [HideInInspector] public List<Character> thisCharacterAllies;
    [HideInInspector] public List<Character> thisCharacterEnemies;
    public Effect overTimeEffect;
    public bool hasToPassTurn;
    public bool effectDoneForThisTurn;
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
        turn.turnOrder.characterOrder.Remove(this);
        if (!characterData.isAI) turn.turnOrder.playableCharacters.Remove(this);
        else turn.turnOrder.enemyCharacters.Remove(this);
    }

    public void CheckGood()
    {
        if (characterData.isAI)
        {
            Good = true;
            goodSymbol.SetActive(true);
            turn.turnOrder.characterOrder.Remove(this);
            turn.turnOrder.enemyCharacters.Remove(this);
        }
    }

    private void TurnCheck()
    {
        if (overTimeEffect.effectData != null && overTimeEffect.canBeRemoved) overTimeEffect.effectData = null;
        if (isLocked && hasToPassTurn) hasToPassTurn = false;
        if (isLocked && effectDoneForThisTurn) effectDoneForThisTurn = false;
        if (Dead && perditionSymbol.activeSelf) perditionSymbol.SetActive(false);
        if (!isLocked && !passageDone)
        {
            ThisCharacterTurn();
            if (characterData.characterStats.currentHP <= 0) SetDead();
        }
    }

    private void ThisCharacterTurn()
    {
        passageDone = true;
        turnIndicator.SetActive(true);
    }

    public void ValueChangeSensitivity()
    {
        float hpMultiplier = 1f;
        float bgMultiplier = 1f;
        if (overTimeEffect.effectData != null && overTimeEffect.effectData.setsSensitivity)
        {
            hpMultiplier *= overTimeEffect.effectData.HPValueChangeSensitivity / 100f;
            bgMultiplier *= overTimeEffect.effectData.BGValueChangeSensitivity / 100f;
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
        goodSymbol.SetActive(false);
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
    }

    public void CharacterApplyEffects()
    {
        if ((overTimeEffect.effectData.effectOverTime && !overTimeEffect.effectData.effectTimeDecreasesOnDamage && !overTimeEffect.effectData.effectTimeDecreasesOnInteraction))
        {
            overTimeEffect.ExecuteEffect();
            if (overTimeEffect.canBeRemoved)
            {
                overTimeEffect.effectData = null;
                overTimeEffect.canBeRemoved = false;
            }
        }
    }
}
