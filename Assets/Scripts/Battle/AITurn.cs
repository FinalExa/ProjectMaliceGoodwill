using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MonoBehaviour
{
    [SerializeField] private Turn turn;
    private Character aiToControl;
    private List<AITurnData> aiTurnData;
    private int thisAIId;
    private bool failedToExecuteAction;

    private void Start()
    {
        ComposeAITurnData();
    }

    private void ComposeAITurnData()
    {
        aiTurnData = new List<AITurnData>();
        foreach (Character enemy in turn.turnOrder.enemyCharacters)
        {
            AITurnData aiInfo = new AITurnData();
            aiInfo.AIName = enemy.characterData.characterStats.characterName;
            aiInfo.AIActionIndex = 0;
            aiInfo.sequence = enemy.characterData.actionSequences;
            for (int i = 0; i < aiInfo.sequence.Length; i++) aiInfo.sequence[i].actionOrderIndex = 0;
            aiTurnData.Add(aiInfo);
        }
    }

    public void AIStartup(Character aiReceived)
    {
        aiToControl = aiReceived;
        failedToExecuteAction = false;
        GoodCheck();
        if (!aiToControl.fullGoodAI)
        {
            FindAIID();
            AssignActionIndex();
            AssignTarget();
            ExecuteAction();
        }
        turn.PassTurn();
    }

    private void GoodCheck()
    {
        if (aiToControl.characterData.characterStats.BGCurrentValue == turn.gameData.BGMaxValue) aiToControl.fullGoodAI = true;
        else aiToControl.fullGoodAI = false;
    }

    private void FindAIID()
    {
        for (int i = 0; i < aiTurnData.Count; i++)
        {
            if (aiTurnData[i].AIName == aiToControl.characterData.characterStats.characterName)
            {
                thisAIId = i;
                break;
            }
        }
    }

    private void AssignActionIndex()
    {
        int index = 0;
        AIActionSequences[] actSeq = aiTurnData[thisAIId].sequence;
        CharacterStats stats = aiToControl.characterData.characterStats;
        for (int i = 0; i < actSeq.Length; i++)
        {
            if (stats.BGCurrentValue <= actSeq[i].MGMaxRange && stats.BGCurrentValue > actSeq[i].MGMinRange)
            {
                index = i;
                break;
            }
        }
        if (index != aiTurnData[thisAIId].AIActionIndex)
        {
            aiTurnData[thisAIId].AIActionIndex = index;
            aiTurnData[thisAIId].sequence[index].actionOrderIndex = 0;
        }
        turn.chosenAction = actSeq[index].actionOrderRange[actSeq[index].actionOrderIndex].actionToDo;
    }

    private void AssignTarget()
    {
        if (!turn.chosenAction.targetsGroups) SingleTargeting();
        else MultiTargeting();
    }

    private void SingleTargeting()
    {
        List<Character> availableTargetsList = new List<Character>();
        List<Character> listToCheck = new List<Character>();
        availableTargetsList.Clear();
        bool selfIncluded = false;
        if (turn.chosenAction.canTargetSelf) selfIncluded = true;
        if (turn.chosenAction.type.actionType == Type.ActionType.BAD) listToCheck = aiToControl.thisCharacterEnemies;
        else listToCheck = aiToControl.thisCharacterAllies;
        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (!listToCheck[i].Dead && (listToCheck[i] != aiToControl && !selfIncluded)) availableTargetsList.Add(listToCheck[i]);
        }
        if (availableTargetsList.Count >= 1)
        {
            int rand = Random.Range(0, availableTargetsList.Count);
            turn.target = availableTargetsList[rand];
        }
        else failedToExecuteAction = true;
    }

    private void MultiTargeting()
    {
        AIActionSequences[] actSeq = aiTurnData[thisAIId].sequence;
        int index = aiTurnData[thisAIId].AIActionIndex;
        turn.multiTargetingOption = actSeq[index].actionOrderRange[actSeq[index].actionOrderIndex].actionGroupTargeted.ToString();
    }

    private void ExecuteAction()
    {
        AdvanceActionOrderIndex();
        if (!failedToExecuteAction) turn.ActionDoneOnTarget();
        else
        {
            turn.battleText.UpdateBattleText(aiToControl.characterData.characterStats.characterName + " failed to perform " + turn.chosenAction.actionName + "!");
            turn.PassTurn();
        }
    }

    private void AdvanceActionOrderIndex()
    {
        AIActionSequences[] actSeq = aiTurnData[thisAIId].sequence;
        int index = aiTurnData[thisAIId].AIActionIndex;
        ActionOrderRange[] actionOrder = actSeq[index].actionOrderRange;
        if (actSeq[index].actionOrderIndex + 1 >= actionOrder.Length) actSeq[index].actionOrderIndex = 0;
        else actSeq[index].actionOrderIndex++;
    }
}
