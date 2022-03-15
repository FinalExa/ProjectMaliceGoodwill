using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats
{
    public string characterName;
    public float MGMinLimit;
    public float MGMaxLimit;
    public float MGStartPoint;
    [HideInInspector] public float MGCurrentValue;
    public float maxStamina;
    [HideInInspector] public float currentStamina;
    public float maxMental;
    [HideInInspector] public float currentMental;

    public void SetStatsStartup()
    {
        MGCurrentValue = MGStartPoint;
        currentStamina = maxStamina;
        currentMental = maxMental;
    }
}
