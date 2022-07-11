using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    private PopulateDropdowns populateDropdowns;
    public Action actionToActivate;
    private Image thisImage;
    [SerializeField] private Text thisText;
    [SerializeField] private Text thisDescriptionText;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color badColor;
    [SerializeField] private GameObject actionDescription;

    private void Awake()
    {
        populateDropdowns = FindObjectOfType<PopulateDropdowns>();
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
        if (actionToActivate != null)
        {
            thisText.text = actionToActivate.actionName;
            thisDescriptionText.text = actionToActivate.actionDescription;
            if (actionToActivate.type.actionType == Type.ActionType.GOOD && thisImage.color != goodColor) thisImage.color = goodColor;
            if (actionToActivate.type.actionType == Type.ActionType.BAD && thisImage.color != badColor) thisImage.color = badColor;
        }
    }

    public void SetSelfClicked()
    {
        populateDropdowns.selectedAction = actionToActivate;
        populateDropdowns.ActionConfirm();
    }

    public void ActionDescriptionSet(bool setActive)
    {
        actionDescription.SetActive(setActive);
    }
}
