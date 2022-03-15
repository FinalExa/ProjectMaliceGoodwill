using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Action[] characterActions;

    private void Start()
    {
        characterStats.SetStatsStartup();
    }
}
