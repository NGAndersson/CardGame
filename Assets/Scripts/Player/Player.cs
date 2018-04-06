using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    scr_Hand hand;
    scr_Deck deck;
    UnityEngine.UI.Text nameTag;
    public UnityEngine.UI.Text healthText;
    public string playerName;
    public int health = 20;
    public uint manaPool = 1;
    public uint manaPoolMax = 1;
    public uint turnNumber = 0;
    UnityEngine.UI.Text manaPoolText;
    bool firstTurn = true;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitGameObjects()
    {
        nameTag = ((GameObject)Instantiate(Resources.Load("Prefabs/Player/PlayerNameText"), gameObject.transform)).GetComponent<UnityEngine.UI.Text>();
        nameTag.text = playerName;

        hand = ((GameObject)Instantiate(Resources.Load("Prefabs/Player/Hand"), gameObject.transform)).GetComponent<scr_Hand>();

        manaPoolText = ((GameObject)Instantiate(Resources.Load("Prefabs/Player/PlayerManaPool"), gameObject.transform)).GetComponent<UnityEngine.UI.Text>();
        manaPoolText.text = manaPool.ToString();
        
    }

    public void NewTurn()
    {
        if(firstTurn == true) //First turn
        {
            InitGameObjects();
            firstTurn = false;
        }

        //Refresh Mana Pool
        SetManaPool(manaPoolMax);
        manaPool = manaPoolMax;

        //Disable Cards Player Can't Afford
        hand.CostDisable(manaPool);
    }

    public void EndTurn()
    {
        //Increase Max Mana Pool
        manaPoolMax++;
    }

    public void CreateDeck(string deckName)
    {
        deck = ((GameObject)Instantiate(Resources.Load("Prefabs/Player/Deck"), gameObject.transform)).GetComponent<scr_Deck>();
        deck.Init(deckName);

        deck.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(DrawCard);
    }

    public void DrawHand()
    {
        hand.DrawHand(deck);
        hand.CostDisable(manaPool);
    }

    public void DrawCard()
    {
        hand.DrawCard(deck);
        hand.CostDisable(manaPool);
    }

    public void ShuffleDeck()
    {
        deck.Shuffle();
    }

    //Handling the self-aspects of the card (costs, etc)
    public void ResolveCard(Card_Game card)
    {
        //Subtract Mana
        SetManaPool(manaPool - (uint)card.data.Cost);
        hand.CostDisable(manaPool);

        //Send card down the pipeline
        hand.ResolveCard(card);
    }

    public int TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        healthText.text = playerName + " Health: " + health;

        return health;
    }

    public void SetManaPool(uint curMana)
    {
        manaPool = curMana;
        manaPoolText.text = manaPool.ToString();
    }
}
