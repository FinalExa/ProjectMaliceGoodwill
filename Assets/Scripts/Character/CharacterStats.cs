using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats
{
    public string characterName;
    public float SAStartPoint;
    [HideInInspector] public float SACurrentValue;
    public float maxStamina;
    [HideInInspector] public float currentStamina;
    public float maxMental;
    [HideInInspector] public float currentMental;

    public void SetStatsStartup()
    {
        SACurrentValue = SAStartPoint;
        currentStamina = maxStamina;
        currentMental = maxMental;
    }
}
