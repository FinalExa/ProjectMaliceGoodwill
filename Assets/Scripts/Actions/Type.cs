using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Type
{
    [HideInInspector] public enum ActionType { ANGER, SADNESS, HAPPINESS, PANIC, ANXIETY, APATHY, CALMNESS, IRRITATION, ATTENTIVENESS, DEPRESSION, HELPFULNESS, EMPATHY };
    public ActionType actionType;
    [System.Serializable]
    public struct Intention
    {
        public ActionType intention;
        public int mgMinRange;
        public int mgMaxRange;
        public Action[] intentionActions;
        public int magicActionCooldown;
        [HideInInspector] public int magicActionCooldownCount;
    }
    [HideInInspector] public enum ActionOpinion { SEVERELY_NEGATIVE, NEGATIVE, NEUTRAL, POSITIVE, SEVERELY_POSITIVE };
}
