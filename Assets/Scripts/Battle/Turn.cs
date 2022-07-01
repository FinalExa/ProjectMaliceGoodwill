using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    public GameData gameData;
    [HideInInspector] public Character currentCharacter;
    [HideInInspector] public Character target;
    [HideInInspector] public bool multiTargeting;
    [HideInInspector] public bool senderIncluded;
    public List<Character> targets;
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
        targets = new List<Character>();
    }

    private void Update()
    {
        if (currentCharacter != null && !currentCharacter.isLocked && !fightIsOver && !stop) TurnOperations();
        if (stop && Input.GetKeyDown(KeyCode.Return)) ContinueTurn();
    }

    private void TurnOperations()
    {
        if (!currentCharacter.Dead)
        {
            if (!currentCharacter.perdition)
            {
                if (!currentCharacter.characterData.isAI) PlayableCharacterTurn();
                else if (currentCharacter.characterData.isAI) aiTurn.AIStartup(currentCharacter);
            }
            else
            {
                if (!endBattleConditions.perditionCheck) endBattleConditions.perditionCheck = true;
                perditionTurn.PerditionStartup(currentCharacter);
            }
        }
        else PassTurn();
    }

    private void PlayableCharacterTurn()
    {
        populateDropdowns.ActionPopulate();
        stop = true;
        battleText.UpdateBattleText(currentCharacter.characterData.characterStats.characterName + "'s turn.");
    }

    public void ActionDoneOnTarget()
    {
        if (!multiTargeting)
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
        currentCharacter.isLocked = true;
        currentCharacter.passageDone = false;
        turnOrder.turnWait = true;
    }

    private void CreateMultiTargetList()
    {
        print(multiTargetingOption);
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
        else
        {
            foreach (Character enemy in currentCharacter.thisCharacterEnemies) targets.Add(enemy);
            foreach (Character ally in currentCharacter.thisCharacterAllies) targets.Add(ally);
            targets.Add(currentCharacter);
            senderIncluded = false;
        }
        foreach (Character target in targets) actionEffect.UpdateValues(target, currentCharacter, chosenAction, false, senderIncluded);
    }

    private void ActionOnSpectators()
    {
        for (int i = 0; i < turnOrder.turnOrder.Count; i++)
        {
            if ((turnOrder.turnOrder[i] != this || turnOrder.turnOrder[i] != target) && !turnOrder.turnOrder[i].Dead)
            {
                actionEffect.UpdateValues(turnOrder.turnOrder[i], currentCharacter, chosenAction, true, senderIncluded);
                turnOrder.turnOrder[i].UpdateAllBars();
            }
        }
    }

    private void ActionOnSpectatorsMultiTargeting()
    {
        for (int i = 0; i < turnOrder.turnOrder.Count; i++)
        {
            bool isATarget = false;
            for (int y = 0; y < targets.Count; y++)
            {
                if (turnOrder.turnOrder[i] == targets[y])
                {
                    isATarget = true;
                    break;
                }
            }
            if (!isATarget)
            {
                actionEffect.UpdateValues(turnOrder.turnOrder[i], currentCharacter, chosenAction, true, senderIncluded);
            }
        }
    }
}
