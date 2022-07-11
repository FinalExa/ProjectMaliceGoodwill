using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    private ActionTargetButtons actionTargetButtons;
    public Action actionToSelect;
    private Image thisImage;
    [SerializeField] private Text thisText;
    [SerializeField] private Text thisDescriptionText;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color badColor;
    [SerializeField] private GameObject actionDescription;

    private void Awake()
    {
        actionTargetButtons = FindObjectOfType<ActionTargetButtons>();
        thisImage = this.gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        ActionDescriptionSet(false);
    }

    private void Update()
    {
        SetActionFeedback();
    }

    private void SetActionFeedback()
    {
        if (actionToSelect != null)
        {
            thisText.text = actionToSelect.actionName;
            thisDescriptionText.text = actionToSelect.actionDescription;
            if (actionToSelect.type.actionType == Type.ActionType.GOOD && thisImage.color != goodColor) thisImage.color = goodColor;
            if (actionToSelect.type.actionType == Type.ActionType.BAD && thisImage.color != badColor) thisImage.color = badColor;
        }
    }

    public void SetSelfClicked()
    {
        actionTargetButtons.selectedAction = actionToSelect;
        actionTargetButtons.ActionConfirm();
    }

    public void ActionDescriptionSet(bool setActive)
    {
        actionDescription.SetActive(setActive);
    }
}
