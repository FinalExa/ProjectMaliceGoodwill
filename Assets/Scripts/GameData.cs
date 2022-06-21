using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
public class GameData : ScriptableObject
{
    public GameTypes[] gameTypes;
    public Trait[] traits;
    public int BGMinValue;
    public int BGMaxValue;
}