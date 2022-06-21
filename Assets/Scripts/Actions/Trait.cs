using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait
{
    [HideInInspector] public enum CharacterTrait { AGGRESSIVE, PROTECTIVE, COMPOSED, VENGEFUL, CARING, TALKATIVE, DIRECT, RESPECTFUL };
    public CharacterTrait characterTrait;
    public Action[] traitActions;
}
