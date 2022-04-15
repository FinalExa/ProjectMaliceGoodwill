using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MonoBehaviour
{
    [SerializeField] private Turn turn;
    private Character aiToControl;
    [SerializeField] private List<AITurnData> aiTurnData;
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
            aiInfo.targetOrder = new List<CharacterData>();
            aiInfo.sequence = enemy.characterData.actionSequences;
            for (int i = 0; i < aiInfo.sequence.Length; i++) aiInfo.sequence[i].actionOrderIndex = 0;
            foreach (CharacterData target in enemy.characterData.AITargetPreference) aiInfo.targetOrder.Add(target);
            aiTurnData.Add(aiInfo);
        }
    }

    public void AIStartup(Character aiReceived)
    {
        aiToControl = aiReceived;
        FindAIID();
        AssignActionIndex();
        AssignTarget();
        ExecuteAction();
        turn.currentCharacter.PassTurn();
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
            if (stats.MGCurrentValue <= actSeq[i].MGMaxRange && stats.MGCurrentValue >= actSeq[i].MGMinRange)
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
        if (aiTurnData[thisAIId].targetOrder[0].thisCharacter.incapacitated)
        {
            CharacterData tempCharacterData = aiTurnData[thisAIId].targetOrder[0];
            aiTurnData[thisAIId].targetOrder.RemoveAt(0);
            aiTurnData[thisAIId].targetOrder.Add(tempCharacterData);
        }
        foreach (Character character in turn.turnOrder.playableCharacters)
        {
            if (character.characterData.characterStats.characterName == aiTurnData[thisAIId].targetOrder[0].characterStats.characterName)
            {
                turn.target = character;
                break;
            }
        }
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
