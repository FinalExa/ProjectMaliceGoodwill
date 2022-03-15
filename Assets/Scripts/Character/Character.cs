using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Action<bool> passTurn;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Action[] characterActions;
    [SerializeField] private bool isAI;
    [HideInInspector] public bool isLocked;

    private void Start()
    {
        characterStats.SetStatsStartup();
    }

    private void Update()
    {
        if (!isLocked) ThisCharacterTurn();
    }

    private void ThisCharacterTurn()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAI)
        {
            print(characterStats.characterName + " does Action!");
            PassTurn();
        }
        else if (isAI)
        {
            print(characterStats.characterName + " does Action!");
            PassTurn();
        }
    }

    private void PassTurn()
    {
        isLocked = true;
        passTurn(true);
    }
}
