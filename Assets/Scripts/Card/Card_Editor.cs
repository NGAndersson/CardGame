using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Editor : Card {
    public enum enum_cardArea
    {
        Bank,
        Deck
    };

    public enum_cardArea area = enum_cardArea.Bank;
    public int numBank = 0;
    public int numUsed = 0;

    // Use this for initialization
    public override void Start() {
        scr_DeckEditorManager manager = GameObject.Find("DeckEditorManager").GetComponent<scr_DeckEditorManager>();
        gameObject.transform.Find("Button").gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { manager.CardClicked(this);});

    }
	
	// Update is called once per frame
	public override void Update () {
		
	}

    public void InitData(Card_Data data, int count)
    {
        base.InitData(data);
        numBank = count;
        AddUseCount(0);
    }
    
    /// <summary>
    /// Adds an amount to numUsed while subtracting same from numOwned.
    /// </summary>
    /// <param name="change">Amount to add to numUsed</param>
    /// <returns>The number of cards used</returns>
    public int AddUseCount(int change = 0)
    {
        numUsed += change;
        numBank -= change;
        gameObject.transform.Find("Num").GetComponent<UnityEngine.UI.Text>().text = (area == enum_cardArea.Deck) ? numUsed.ToString() + "/4" : numBank.ToString();

        return numUsed;
    }

    /// <summary>
    /// Sets the bank card count to a specific number
    /// DOES NOT TAKE INTO ACCOUNT NUMBER OF CARDS IN DECK
    /// </summary>
    /// <param name="num">Number to set bank count to</param>
    public void SetBankCount(int num = 0)
    {
        numBank = num;
        gameObject.transform.Find("Num").GetComponent<UnityEngine.UI.Text>().text = (area == enum_cardArea.Deck) ? numUsed.ToString() + "/4" : numBank.ToString();
    }
}
