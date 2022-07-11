using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetButton : MonoBehaviour
{
    private Turn turn;
    private ActionTargetButtons actionTargetButtons;
    public Character targetToSelect;
    public bool isMultiTarget;
    public string multiTargetToSelect;
    private Image thisImage;
    [SerializeField] private Text thisText;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color badColor;

    private void Awake()
    {
        turn = FindObjectOfType<Turn>();
        actionTargetButtons = FindObjectOfType<ActionTargetButtons>();
        thisImage = this.gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        SetTargetFeedback();
    }

    private void SetTargetFeedback()
    {
        if (targetToSelect != null)
        {
            if (!isMultiTarget) thisText.text = targetToSelect.characterData.characterStats.characterName;
            if ((turn.currentCharacter == targetToSelect || turn.currentCharacter.thisCharacterAllies.Contains(targetToSelect)) && thisImage.color != goodColor) thisImage.color = goodColor;
            if (turn.currentCharacter.thisCharacterEnemies.Contains(targetToSelect) && thisImage.color != badColor) thisImage.color = badColor;
        }
        if (isMultiTarget)
        {
            thisText.text = multiTargetToSelect;
        }
    }

    public void SetSelfClicked()
    {
        if (!isMultiTarget) actionTargetButtons.selectedTarget = targetToSelect;
        else actionTargetButtons.selectedMultiTarget = multiTargetToSelect;
        actionTargetButtons.TargetConfirm();
    }
}
