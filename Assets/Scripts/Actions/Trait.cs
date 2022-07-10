using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait
{
    [HideInInspector] public enum CharacterTrait { AGGRESSIVE, PROTECTIVE, CALM, VENGEFUL, CARING, TALKATIVE, DIRECT };
    public CharacterTrait characterTrait;
    public Action[] traitActions;
}
