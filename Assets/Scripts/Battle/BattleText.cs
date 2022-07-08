using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleText : MonoBehaviour
{
    public bool inDialogue;
    private Turn turn;
    [SerializeField] private Text battleText;
    private List<string> textQueue;

    private void Awake()
    {
        turn = this.gameObject.GetComponent<Turn>();
    }

    private void Start()
    {
        textQueue = new List<string>();
    }

    public void AddItemToTextList(string itemToAdd)
    {
        textQueue.Add(itemToAdd);
    }

    public void UpdateBattleText(string textToShow)
    {
        battleText.text = textToShow;
    }
}
