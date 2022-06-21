using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleText : MonoBehaviour
{
    [SerializeField] private Text battleText;

    public void UpdateBattleText(string textToShow)
    {
        battleText.text = textToShow;
    }
}
