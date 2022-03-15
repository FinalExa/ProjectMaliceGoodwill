using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Type
{
    [HideInInspector] public enum ACTIONTYPE { VIOLENCE, PROTECTION, MANIPULATION, MOCKERY, CHARITY };
    public ACTIONTYPE actionType;
}
