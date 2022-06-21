using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    public GameData gameData;
    [HideInInspector] public Character currentCharacter;
    [HideInInspector] public Character target;
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
    }

    private void Update()
    {
        if (currentCharacter != null && !currentCharacter.isLocked && !fightIsOver && !stop) TurnOperations();
        if (stop && Input.GetKeyDown(KeyCode.Return)) ContinueTurn();
    }

    private void TurnOperations()
    {
        if (!currentCharacter.incapacitated)
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
        actionEffect.UpdateValues(target, chosenAction, false);
        target.UpdateAllBars();
        ActionOnSpectators();
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
        endBattleConditions.CheckForVictoryConditions();
        currentCharacter.isLocked = true;
        currentCharacter.passageDone = false;
        turnOrder.turnWait = true;
    }

    private void ActionOnSpectators()
    {
        for (int i = 0; i < turnOrder.turnOrder.Count; i++)
        {
            if ((turnOrder.turnOrder[i] != this || turnOrder.turnOrder[i] != target) && !turnOrder.turnOrder[i].incapacitated)
            {
                actionEffect.UpdateValues(turnOrder.turnOrder[i], chosenAction, true);
                turnOrder.turnOrder[i].UpdateAllBars();
            }
        }
    }
}
