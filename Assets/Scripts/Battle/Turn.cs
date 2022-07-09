using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    public GameData gameData;
    [HideInInspector] public Character currentCharacter;
    [HideInInspector] public Character target;
    [HideInInspector] public bool senderIncluded;
    [HideInInspector] public List<Character> targets;
    [HideInInspector] public string multiTargetingOption;
    [HideInInspector] public List<Character> possibleTargets = new List<Character>();
    [HideInInspector] public Action chosenAction;
    [HideInInspector] public bool stop;
    [HideInInspector] public TurnOrder turnOrder;
    [HideInInspector] public PopulateDropdowns populateDropdowns;
    [HideInInspector] public BattleText battleText;
    [SerializeField] private GameObject continueDialogueButton;
    private ActionEffect actionEffect;
    private AITurn aiTurn;
    private PerditionTurn perditionTurn;
    private EndBattleConditions endBattleConditions;
    [HideInInspector] public bool fightIsOver;
    private PerditionTurn perditionCharacterForContinuing;
    private bool perditionContinueCheck;

    private void Awake()
    {
        turnOrder = this.gameObject.GetComponent<TurnOrder>();
        populateDropdowns = this.gameObject.GetComponent<PopulateDropdowns>();
        actionEffect = this.gameObject.GetComponent<ActionEffect>();
        aiTurn = this.gameObject.GetComponent<AITurn>();
        perditionTurn = this.gameObject.GetComponent<PerditionTurn>();
        endBattleConditions = this.gameObject.GetComponent<EndBattleConditions>();
        battleText = this.gameObject.GetComponent<BattleText>();
    }

    private void Start()
    {
        continueDialogueButton.SetActive(false);
        battleText.UpdateBattleText("Battle Start");
        targets = new List<Character>();
    }

    private void Update()
    {
        SetCurrentCharacter();
        if (!fightIsOver && !stop) TurnOperations();
        if (stop && Input.GetKeyDown(KeyCode.Return)) ContinueTurn();
    }

    private void SetCurrentCharacter()
    {
        if (currentCharacter != turnOrder.currentCharacter)
        {
            currentCharacter = turnOrder.currentCharacter;
            StopTurn();
        }
    }

    private void TurnOperations()
    {
        currentCharacter.CharacterApplyEffects();
        if (currentCharacter.hasToPassTurn)
        {
            PassTurn();
            return;
        }
        if (!currentCharacter.Dead)
        {
            if (!currentCharacter.perdition)
            {
                if (!currentCharacter.characterData.isAI) PlayableCharacterTurn();
                else
                {
                    aiTurn.AIStartup(currentCharacter);
                }
            }
            else perditionTurn.PerditionStartup(currentCharacter);
            stop = true;
        }
        else PassTurn();
    }

    private void PlayableCharacterTurn()
    {
        populateDropdowns.ActionPopulate();
        battleText.UpdateBattleText(currentCharacter.characterData.characterStats.characterName + "'s turn.");
    }

    public void ActionDoneOnTarget()
    {
        if (!chosenAction.targetsGroups)
        {
            if (target == currentCharacter) senderIncluded = true;
            else senderIncluded = false;
            actionEffect.UpdateValues(target, currentCharacter, chosenAction, false, senderIncluded);
            ActionOnSpectators();
        }
        else
        {
            CreateMultiTargetList();
            ActionOnSpectatorsMultiTargeting();
            targets.Clear();
        }
    }

    public void ContinueTurn()
    {
        stop = false;
        continueDialogueButton.SetActive(false);
        if (perditionContinueCheck)
        {
            perditionCharacterForContinuing.PerditionContinueTurn();
            perditionContinueCheck = false;
        }
    }

    public void StopTurn()
    {
        stop = true;
        continueDialogueButton.SetActive(true);
    }

    public void StopTurn(PerditionTurn perditionCharacter)
    {
        stop = true;
        continueDialogueButton.SetActive(true);
        perditionCharacterForContinuing = perditionCharacter;
        perditionContinueCheck = true;
    }

    public void PassTurn()
    {
        currentCharacter.turnIndicator.SetActive(false);
        endBattleConditions.CheckForVictoryConditions();
        populateDropdowns.TurnAllOff();
        turnOrder.turnWait = true;
    }

    private void CreateMultiTargetList()
    {
        if (multiTargetingOption == "Enemies")
        {
            foreach (Character enemy in currentCharacter.thisCharacterEnemies) targets.Add(enemy);
            senderIncluded = false;
        }
        else if (multiTargetingOption == "Allies")
        {
            foreach (Character ally in currentCharacter.thisCharacterAllies) targets.Add(ally);
            senderIncluded = false;
        }
        else if (multiTargetingOption == "Party")
        {
            foreach (Character ally in currentCharacter.thisCharacterAllies) targets.Add(ally);
            targets.Add(currentCharacter);
            senderIncluded = true;
        }
        else if (multiTargetingOption == "Others")
        {
            foreach (Character enemy in currentCharacter.thisCharacterEnemies) targets.Add(enemy);
            foreach (Character ally in currentCharacter.thisCharacterAllies) targets.Add(ally);
            senderIncluded = false;
        }
        else if (multiTargetingOption == "Everyone")
        {
            foreach (Character enemy in currentCharacter.thisCharacterEnemies) targets.Add(enemy);
            foreach (Character ally in currentCharacter.thisCharacterAllies) targets.Add(ally);
            targets.Add(currentCharacter);
            senderIncluded = true;
        }
        actionEffect.groupAttackName = multiTargetingOption;
        foreach (Character target in targets) actionEffect.UpdateValues(target, currentCharacter, chosenAction, false, senderIncluded);
    }

    private void ActionOnSpectators()
    {
        for (int i = 0; i < turnOrder.characterOrder.Count; i++)
        {
            if ((turnOrder.characterOrder[i] != this || turnOrder.characterOrder[i] != target) && !turnOrder.characterOrder[i].Dead)
            {
                actionEffect.UpdateValues(turnOrder.characterOrder[i], currentCharacter, chosenAction, true, senderIncluded);
            }
        }
    }

    private void ActionOnSpectatorsMultiTargeting()
    {
        for (int i = 0; i < turnOrder.characterOrder.Count; i++)
        {
            bool isATarget = false;
            for (int y = 0; y < targets.Count; y++)
            {
                if (turnOrder.characterOrder[i] == targets[y])
                {
                    isATarget = true;
                    break;
                }
            }
            if (!isATarget)
            {
                actionEffect.UpdateValues(turnOrder.characterOrder[i], currentCharacter, chosenAction, true, senderIncluded);
            }
        }
    }
}
