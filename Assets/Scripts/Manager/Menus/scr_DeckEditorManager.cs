using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_DeckEditorManager : MonoBehaviour {
    Dictionary<Card_Editor, int> bankCards = new Dictionary<Card_Editor, int>();
    Dictionary<Card_Editor, int> deckCards = new Dictionary<Card_Editor, int>();
    public UnityEngine.UI.Dropdown deckDropdown;
    private int deckSize = 0;
    public UnityEngine.UI.Button newDeckButton;
    public UnityEngine.UI.Button loadDeckButton;
    public UnityEngine.UI.Button saveDeckButton;
    public UnityEngine.UI.Button deleteDeckButton;
    public UnityEngine.UI.InputField newNameField;
    public UnityEngine.UI.Button savePromptButton;
    private string currentDeckName;
    public UnityEngine.UI.Text currentDeckNameText;


    // Use this for initialization
    void Start () {

	}

    private void OnEnable()
    {
        // Add all Bank Cards
        foreach(Inventory_Item item in Database.Instance.GetAllInventoryCards())
        {
            InstantiateCard(item, GameObject.Find("BankContent").transform, Card_Editor.enum_cardArea.Bank);
        }
        ReorganizeCards();

        // Show all Deck Cards from default deck


        // Update deck list
        Database.Instance.decks.UpdateDecks(UpdateDeckList);

        // Reset current deck name
        SetCurrentDeckName("");
    }

    private void OnDisable()
    {
        ClearDeck();
        foreach (Transform card in gameObject.transform.parent.Find("AvailableCards").Find("BankArea").Find("BankContent"))
        {
            Destroy(card.gameObject);
        }
        bankCards.Clear();

    }

    public void MenuButton()
    {
        MenuManager.Instance.GoToPrevious();
    }

    // Update is called once per frame
    void Update () {

	}

    /// <summary>
    /// Instantiates a card from its card data.
    /// </summary>
    /// <param name="card">Data from which to create the card.</param>
    /// <param name="parent">Area where card should be. (DeckContent/BankContent)</param>
    /// <param name="area"></param>
    private Card_Editor InstantiateCard(Inventory_Item item, Transform parent, Card_Editor.enum_cardArea area)
    {
        GameObject newCard = (GameObject)Instantiate(Resources.Load("Prefabs/Cards/Pf_Editor_Card"), parent);
        newCard.GetComponent<Card_Editor>().InitData(item.card, item.count);

        Dictionary<Card_Editor, int> dict = (parent.gameObject.name == "DeckContent") ? deckCards : bankCards;
        newCard.GetComponent<Card_Editor>().area = area;
        dict[newCard.GetComponent<Card_Editor>()] = item.count;
        newCard.GetComponent<Card_Editor>().AddUseCount();
        return newCard.GetComponent<Card_Editor>();
    }

    private void ReorganizeCards()
    {
        //Change height of scrollable content area
        float bankWidth = GameObject.Find("AvailableCards").GetComponent<RectTransform>().sizeDelta.x + GameObject.Find("BankArea").GetComponent<RectTransform>().sizeDelta.x;
        float deckWidth = GameObject.Find("DeckCards").GetComponent<RectTransform>().sizeDelta.x + GameObject.Find("DeckArea").GetComponent<RectTransform>().sizeDelta.x;
        GameObject.Find("BankContent").GetComponent<RectTransform>().sizeDelta = new Vector2(bankWidth, (Mathf.Ceil(bankCards.Count / 3.0f)) * 340 + 20);
        GameObject.Find("DeckContent").GetComponent<RectTransform>().sizeDelta = new Vector2(deckWidth, (Mathf.Ceil(deckCards.Count / 2.0f)) * 340 + 20);

        int posIndex = 0;
        int cardsInRow = (int)bankWidth / 220;
        // Todo - Sort by ID (or something else)
        foreach (Card_Editor card in bankCards.Keys)
        {
            card.transform.localPosition = new Vector3(((posIndex * 220) % (bankWidth-20)) + 20, (posIndex / cardsInRow) *-340 - 20);
            posIndex++;
        }

        posIndex = 0;
        cardsInRow = (int)deckWidth / 220;
        foreach (Card_Editor card in deckCards.Keys)
        {
            card.transform.localPosition = new Vector3(((posIndex * 220) % (deckWidth-20)) + 20, (posIndex / cardsInRow) *-340 - 20);
            posIndex++;
        }

    }

    public void CardClicked(Card_Editor card)
    {
        if(card.area == Card_Editor.enum_cardArea.Bank)
        {
            //Increment&Deincrement card counters
            if(bankCards[card] > 0)
            {
                Card_Editor deckEquivalent = GetOtherCard(Card_Editor.enum_cardArea.Bank, card.data.ID);
                if(deckEquivalent != null && deckCards[deckEquivalent] < 4)
                {
                    bankCards[card]--;
                    card.AddUseCount(1);

                    deckCards[deckEquivalent]++; deckSize++;
                    deckEquivalent.AddUseCount(1);

                } else if (deckEquivalent == null) { //New type of card in the deck, spawn instance
                    bankCards[card]--;
                    card.AddUseCount(1);

                    Card_Editor newCard = Instantiate(card.gameObject, GameObject.Find("DeckContent").transform).GetComponent<Card_Editor>();
                    newCard.GetComponent<Card_Editor>().area = Card_Editor.enum_cardArea.Deck;
                    deckCards[newCard] = 1; deckSize++;
                    newCard.numUsed = deckCards[newCard];
                    newCard.AddUseCount(0);
                }
            }
        }
        else if (card.area == Card_Editor.enum_cardArea.Deck)
        {
            if(deckCards[card] > 0)
            {
                Card_Editor bankEquivalent = GetOtherCard(Card_Editor.enum_cardArea.Deck, card.data.ID);

                //Increment&Deincrement card counters
                bankCards[bankEquivalent]++;

                bankEquivalent.AddUseCount(-1);
                deckCards[card]--; deckSize--;
                card.AddUseCount(-1);

                //No more cards in deck of that type. Destroy instance.
                if (deckCards[card] == 0)
                {
                    deckCards.Remove(card);
                    Destroy(card.gameObject);
                }
            }
        }
        ReorganizeCards();
    }

    Card_Editor GetOtherCard(Card_Editor.enum_cardArea originArea, string ID)
    {
        Dictionary<Card_Editor, int> opposite = (originArea == Card_Editor.enum_cardArea.Bank) ? deckCards : bankCards;
        
        foreach (Card_Editor card in opposite.Keys)
        {
            if (card.data.ID == ID)
                return card;
        }

        return null;
    }

    public void ConfirmNewDeckButton()
    {
        //Assume input field content type validates the name
        string newName = newNameField.text;

        //Ask to save current deck, if applicable. AUTOSAVES TODO
        if(currentDeckName != "" && !SaveDeck(currentDeckName))
            return;

        //Make empty deck
        ResetBankCounts();
        ClearDeck();

        //Save empty deck
        SaveDeck(newName);
        GameObject savePrompt = GameObject.Find("NewDeckPrompt");
        savePrompt.SetActive(false);
        SetCurrentDeckName(newName);
    }

    public void SaveButton()
    {
        SaveDeck(currentDeckName);
    }

    public void SaveInputTextChange()
    {
        if(newNameField.text == "")
        {
            savePromptButton.interactable = false;
        }
        else
        {
            savePromptButton.interactable = true;

            //If a deck already exists with the name
            if(Database.Instance.decks.GetDeckNames().Contains(newNameField.text))
                savePromptButton.gameObject.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "OVERWRITE";
            else
                savePromptButton.gameObject.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "SAVE";
        }
    }

    public void OpenNewDeckPrompt()
    {
        GameObject savePrompt = gameObject.transform.parent.Find("NewDeckPrompt").gameObject;
        savePrompt.SetActive(true);
    }

    public void CloseNewDeckPrompt()
    {
        GameObject savePrompt = GameObject.Find("NewDeckPrompt");
        savePrompt.transform.Find("input_NewDeckName").GetComponent<UnityEngine.UI.InputField>().text = "";
        savePrompt.SetActive(false);
        
    }

    private bool SaveDeck(string name)
    {
        List<scr_Deck_Data.Card_Entry> cards = new List<scr_Deck_Data.Card_Entry>();
        
        // Convert current deck dictionary to card_data list
        foreach(Card_Editor card in deckCards.Keys)
        {
            scr_Deck_Data.Card_Entry cardData = new scr_Deck_Data.Card_Entry();
            cardData.ID = card.GetComponent<Card_Editor>().data.ID;
            cardData.count = deckCards[card];
            cards.Add(cardData);
        }

        // Save with Deck_Data class
        Database.Instance.decks.SaveDeck(cards, name, deckSize, Callback_SaveSuccess);

        return true;
    }

    private void Callback_SaveSuccess(PlayFab.ClientModels.UpdateUserDataResult result)
    {
        UpdateDeckList();
    }

    public void LoadDeck(string deckName = "")
    {
        bool failed = false;

        // Get Deck Name
        if(deckName == "")
            deckName = deckDropdown.options[deckDropdown.value].text;

        // Reset counts on bankCards and destroy current deck
        ResetBankCounts();
        ClearDeck();

        // Load deck_entry list
        scr_Deck_Data.Save_Data savedDeck = Database.Instance.decks.LoadDeck(deckName);

        // Handle deck size and name
        deckSize = savedDeck.deckSize;
        //this.deckName = savedDeck.deckName;

        // Re-populate deckCards
        foreach (scr_Deck_Data.Card_Entry cardEntry in savedDeck.cardList)
        {
            //Get card from database
            Inventory_Item cardItem = Database.Instance.GetInventoryCard(cardEntry.ID);
            if (cardItem == null)
            {
                failed = true; break;
            }

            Card_Editor card = InstantiateCard(cardItem, GameObject.Find("DeckContent").transform, Card_Editor.enum_cardArea.Deck);
            card.AddUseCount(cardEntry.count);
            deckCards[card] = cardEntry.count;
            Card_Editor bankCard = GetOtherCard(Card_Editor.enum_cardArea.Deck, card.data.ID);
            if (bankCard != null)
            {
                bankCards[bankCard] -= cardEntry.count;
                bankCard.AddUseCount(cardEntry.count);
            } else {
                // Bank card does not exist. Hacking is afoot?!
                Debug.Log("Loading deck with cards player does not own!");
                failed = true; break;
            }
        }

        if(failed)
        {
            //Cleanup, should go back to old state of Bank and Deck. But that's annoying.
            ResetBankCounts();
            ClearDeck();
        }

        SetCurrentDeckName(deckName);
        ReorganizeCards();
    }

    public void DeleteDeck()
    {
        string chosenDeck = deckDropdown.options[deckDropdown.value].text;
        Debug.Log(chosenDeck);
        foreach(scr_Deck_Data.Save_Data deck in Database.Instance.decks.decks)
        {
            if (deck.deckName == chosenDeck)
            {
                Debug.Log("Deleting " + chosenDeck);
                Database.Instance.decks.decks.Remove(deck);
                break;
            }
        }
        if (chosenDeck != currentDeckName && currentDeckName != "")
            SaveDeck(currentDeckName);

        Database.Instance.decks.UploadDecks(Callback_SaveSuccess);


        //Update dropdown
        UpdateDeckList();

        Debug.Log("OK");
        //Reset Deck and Bank
        ResetBankCounts();
        ClearDeck();

        //Load another deck if one exists
        if (deckDropdown.interactable)
        {
            string nextDeck = deckDropdown.options[0].text;
            LoadDeck(nextDeck);
        }
        else
        {
            SetCurrentDeckName("");
        }
    }

    private void ResetBankCounts()  // Should probably reload all cards in bank (in case any were deleted by being moved to deck)
    {
        foreach(Card_Editor card in bankCards.Keys)
        {
            SetCardBankCount(card, Database.Instance.GetInventoryCard(card.data.ID).count);
        }
    }

    private void SetCardBankCount(Card_Editor card, int count)
    {
        card.SetBankCount(count);
    }

    /// <summary>
    /// Removes everything in the deck
    /// </summary>
    private void ClearDeck()
    {
        foreach(Card_Editor card in deckCards.Keys)
        {
            Destroy(card.gameObject);
        }
        deckCards.Clear();
        deckSize = 0;
    }

    private void UpdateDeckList()
    {
        //Update dropdown
        if (Database.Instance.decks.GetDeckNames().Count > 0)
        {
            deckDropdown.interactable = true;
            deckDropdown.ClearOptions();
            deckDropdown.AddOptions(Database.Instance.decks.GetDeckNames());
            loadDeckButton.interactable = true;
            deleteDeckButton.interactable = true;
        }
        else
        {
            deckDropdown.interactable = false;
            Debug.Log(deckDropdown.interactable);
            deckDropdown.ClearOptions();
            List<string> noOption = new List<string> { "No Saved Decks Found" };
            deckDropdown.AddOptions(noOption);
            loadDeckButton.interactable = false;
            deleteDeckButton.interactable = false;
        }
    }

    /// <summary>
    /// Updates the deck list dropdown.
    /// </summary>
    private void UpdateDeckList(PlayFab.ClientModels.GetUserDataResult result)
    {
        if(Database.Instance.decks.currentDeckListVersion != result.DataVersion && result.Data["Decks"].Value != null)
            Database.Instance.decks.CreateDeckList(result.Data["Decks"].Value);

        Database.Instance.decks.currentDeckListVersion = result.DataVersion;

        //Update dropdown
        if (Database.Instance.decks.GetDeckNames().Count > 0)
        {
            deckDropdown.interactable = true;
            deckDropdown.ClearOptions();
            deckDropdown.AddOptions(Database.Instance.decks.GetDeckNames());
            loadDeckButton.interactable = true;
            deleteDeckButton.interactable = true;
        } else {
            deckDropdown.interactable = false;
            deckDropdown.ClearOptions();
            List<string> noOption = new List<string> { "No Saved Decks Found" };
            deckDropdown.AddOptions(noOption);
            loadDeckButton.interactable = false;
            deleteDeckButton.interactable = false;
        }
    }

    private void SetCurrentDeckName(string deckName)
    {
        currentDeckName = deckName;
        currentDeckNameText.text = currentDeckName;
        saveDeckButton.interactable = (currentDeckName != "") ? true : false;

        //No deck. Disable interaction of bank cards.
        if(currentDeckName == "")
            foreach(Card_Editor card in bankCards.Keys)
                card.gameObject.GetComponentInChildren<UnityEngine.UI.Button>().interactable = false;
        else
            foreach (Card_Editor card in bankCards.Keys)
                card.gameObject.GetComponentInChildren<UnityEngine.UI.Button>().interactable = true;

    }
}
