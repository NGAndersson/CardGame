using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Deck : MonoBehaviour {
    public List<GameObject> cards;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(string deckName)
    {
        Object masterCard = Resources.Load("Prefabs/Cards/Pf_Card_Game") as GameObject;
        scr_Deck_Data.Save_Data deckSave = Database.Instance.decks.LoadDeck(deckName);
        
        // Make cards
        foreach (scr_Deck_Data.Card_Entry card in deckSave.cardList)
        {
            //Get Card Data from ID in savefile
            Card_Data cardData = Database.Instance.GetInventoryCard(card.ID).card;

            //Create correct number of cards
            for(int i = 0; i < card.count; i++)
            {
                GameObject cardCopy = Instantiate(masterCard) as GameObject;
                cardCopy.GetComponent<Card_Game>().InitData(cardData);

                cardCopy.transform.SetParent(gameObject.transform);
                cardCopy.SetActive(false);
                cards.Add(cardCopy);
            }
        }
    }

    public void Shuffle ()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
}
