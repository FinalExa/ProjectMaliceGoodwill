using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharacterStats
{
    public string characterName;
    public Color characterColor;
    public float BGStartPoint;
    [HideInInspector] public float BGCurrentValue;
    public float maxHP;
    [HideInInspector] public float currentHP;

    public void SetStatsStartup()
    {
        BGCurrentValue = BGStartPoint;
        currentHP = maxHP;
    }
}
