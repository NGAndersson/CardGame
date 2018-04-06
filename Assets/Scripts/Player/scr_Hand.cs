using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Hand : MonoBehaviour {
    List<GameObject> hand = new List<GameObject>();

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawHand (scr_Deck deck)
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCard(deck);
        }
    }

    public void DrawCard (scr_Deck deck)
    {
        if (deck.cards.Count > 0)
        {
            //Get the card from the deck
            GameObject newCard = deck.cards[0];
            newCard.SetActive(true);
            deck.cards.RemoveAt(0);
            newCard.transform.SetParent(gameObject.transform); //Set card to be child of hand
            hand.Add(newCard);
            RepositionCards();
        }
        else
        {
            // Lose? Out of cards in deck!
        }
    }

    public void ResolveCard(Card_Game card)
    {
        // Remove and reposition hand
        hand.Remove(card.gameObject);
        Destroy(card.gameObject);
        RepositionCards();
    }

    void RepositionCards ()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            GameObject card = hand[i];
            card.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0);
            card.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
            card.transform.localScale = new Vector3(0.4f, 0.4f);

            int spacing = Mathf.Min(200, 950/hand.Count);
            card.transform.localPosition = new Vector3((i - (hand.Count-1) / 2.0f) * spacing, 0); // Division might not work
        }
    }

    public void CostDisable(uint manaPool)
    {
        foreach(GameObject cardObj in hand)
        {
            bool canAfford;
            if (cardObj.GetComponent<Card_Game>().data.Cost <= manaPool)
                canAfford = true;
            else canAfford = false;
            cardObj.GetComponentInChildren<UnityEngine.UI.Button>().interactable = canAfford;
        }
    }
}
