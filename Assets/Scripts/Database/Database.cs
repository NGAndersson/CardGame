using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enum_loadingState {
    Database,
    Inventory,
    Done
}

public class Database {
    //Singleton
    private static Database instance;
    private Database() { }
    public static Database Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new Database();
            }
            return instance;
        }
    }

    public enum_loadingState loadingState;
    public TextAsset cardsJson;
    //private Database_Cards db_cardDatabase;
    private Dictionary<string, Card_Data> cardDatabase = new Dictionary<string, Card_Data>();
    private Dictionary<string, Inventory_Item> cardInventory = new Dictionary<string, Inventory_Item>();

    public scr_Deck_Data decks = new scr_Deck_Data();

    public Dictionary<string, int> userCurrency = new Dictionary<string, int>();

    public void InitPlayer()
    {
        DownloadCardDatabase();
    }

    public Inventory_Item GetInventoryCard(string ID)
    {
        if(cardInventory.ContainsKey(ID))
        {
            return cardInventory[ID];
        }
        
        return null;
    }

    public Card_Data GetDatabaseCard(string ID)
    {
        if (cardDatabase.ContainsKey(ID))
        {
            return cardDatabase[ID];
        }

        Debug.LogError("Tried to get a card from cardDatabase that doesn't exist! (wrong ID)");
        return null;
    }

    public List<Inventory_Item> GetAllInventoryCards()
    {
        List<Inventory_Item> cards = new List<Inventory_Item>();
        foreach(Inventory_Item item in cardInventory.Values)
            cards.Add(item);
        return cards;
    }

    public void DownloadCardDatabase()
    {
        loadingState = enum_loadingState.Database;
        cardDatabase.Clear();
        Online.Instance.GetCatalog("Cards", Callback_DownloadCardDatabase_Success);
    }

    private void Callback_DownloadCardDatabase_Success(PlayFab.ClientModels.GetCatalogItemsResult result)
    {
        foreach(PlayFab.ClientModels.CatalogItem card in result.Catalog)
        {
            cardDatabase[card.ItemId] = JsonUtility.FromJson<Card_Data>(card.CustomData);
            cardDatabase[card.ItemId].ID = card.ItemId;
        }

        DownloadAllInventoryCards();
    }

    public void DownloadAllInventoryCards()
    {
        loadingState = enum_loadingState.Inventory;
        Online.Instance.GetUserInventory(Callback_GetInventory_Success);
    }

    private void Callback_GetInventory_Success(PlayFab.ClientModels.GetUserInventoryResult result)
    {
        cardInventory.Clear();
        foreach (PlayFab.ClientModels.ItemInstance item in result.Inventory)
        { 
            if (cardDatabase.ContainsKey(item.ItemId))
            {
                //Create new inventory item if card does not already exist
                if (!cardInventory.ContainsKey(item.ItemId))
                {
                    cardInventory[item.ItemId] = new Inventory_Item() { card = cardDatabase[item.ItemId] };
                    cardInventory[item.ItemId].card.ID = item.ItemId;
                    cardInventory[item.ItemId].count = (int)item.RemainingUses;
                }
            }
        }
        loadingState = enum_loadingState.Done;

        //Update User Currency
        userCurrency = result.VirtualCurrency;
    }
}
