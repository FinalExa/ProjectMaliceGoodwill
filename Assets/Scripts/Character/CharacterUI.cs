using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Character character;
    public Text MGBar;
    public Text staminaBar;
    public Text mentalBar;

    public void UpdateBar(Text barToUpdate, float barCurrentValue, float barMaxValue)
    {
        barToUpdate.text = barToUpdate.name + ": " + barCurrentValue.ToString() + "/" + barMaxValue.ToString();
    }
}
