using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MonoBehaviour
{
    [SerializeField] private Turn turn;
    private Character aiToControl;
    private List<AITurnData> aiTurnData;
    private int thisAIId;

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
            if (stats.BGCurrentValue <= actSeq[i].MGMaxRange && stats.BGCurrentValue >= actSeq[i].MGMinRange)
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
    }

    private void AssignTarget()
    {
        List<Character> availableTargetsList = new List<Character>();
        availableTargetsList.Clear();
        for (int i = 0; i < turn.turnOrder.playableCharacters.Count; i++)
        {
            if (!turn.turnOrder.playableCharacters[i].Dead) availableTargetsList.Add(turn.turnOrder.playableCharacters[i]);
        }
        int rand = Random.Range(0, availableTargetsList.Count);
        turn.target = availableTargetsList[rand];
    }

    private void ExecuteAction()
    {
        AIActionSequences[] actSeq = aiTurnData[thisAIId].sequence;
        int index = aiTurnData[thisAIId].AIActionIndex;
        Action[] actionOrder = actSeq[index].actionOrderInThatRange;
        turn.chosenAction = actSeq[index].actionOrderInThatRange[actSeq[index].actionOrderIndex];
        if (actSeq[index].actionOrderIndex + 1 >= actionOrder.Length) actSeq[index].actionOrderIndex = 0;
        else actSeq[index].actionOrderIndex++;
        turn.ActionDoneOnTarget();
    }
}
