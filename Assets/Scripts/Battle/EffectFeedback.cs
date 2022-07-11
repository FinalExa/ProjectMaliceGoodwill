using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectFeedback : MonoBehaviour
{
    [HideInInspector] public Effect effect;
    [SerializeField] private GameObject container;
    [SerializeField] private Text textToShow;

    private void Update()
    {
        SetText();
    }

    public void SetText()
    {
        if (effect.effectData != null) textToShow.text = effect.effectData.effectDescription + "\nRemaining Duration: " + effect.timeLeft;
        else textToShow.text = string.Empty;
    }

    public void EffectDescriptionSet(bool setActive)
    {
        container.SetActive(setActive);
    }
}
