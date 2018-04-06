using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Deck_Data {
    [System.Serializable]
    public class Card_Entry
    {
        public string ID;
        public int count;
    };

    [System.Serializable]
    public class Save_Data
    {
        public string deckName;
        public int deckSize;
        public List<Card_Entry> cardList;
    }

    [System.Serializable]
    public class Save_DataTemp
    {
        public List<Save_Data> decks;
    }

    public uint currentDeckListVersion;
    public List<Save_Data> decks = new List<Save_Data>();



    public int SaveDeck(List<Card_Entry> cards, string deckName, int deckSize, System.Action<PlayFab.ClientModels.UpdateUserDataResult> successCallback)
    {
        bool existing = false;
        //Check for existing deck to overwrite
        for(int i = 0; i < decks.Count; i++)
        {
            if(decks[i].deckName == deckName)
            {
                decks[i].deckSize = deckSize;
                decks[i].deckName = deckName;
                decks[i].cardList = cards;
                existing = true;
            }
        }
        //New deck
        if(!existing)
            decks.Add(new Save_Data { deckName = deckName, deckSize = deckSize, cardList = cards });

        UploadDecks(successCallback);
        
        return 1;
    }

    public void UploadDecks(System.Action<PlayFab.ClientModels.UpdateUserDataResult> successCallback)
    {
        Save_DataTemp temp = new Save_DataTemp()
        {
            decks = decks
        };
        string jsonString = JsonUtility.ToJson(temp);

        //Upload all decks to PlayFab User Data
        Online.Instance.UpdateUserData("Decks", jsonString, successCallback);
    }

    public Save_Data LoadDeck(string deckName)
    {
        // Return correct deck
        foreach(Save_Data deck in decks)
        {
            if (deck.deckName == deckName)
                return deck;
        }
        return null;


        /*
        System.IO.StreamReader reader = new System.IO.StreamReader("Assets/Resources/Decks/" + deckName + ".json"); // Change to saving in appdata or something
        string jsonString = reader.ReadToEnd();
        saveObj = JsonUtility.FromJson<Save_Data>(jsonString);
        reader.Close();
        return saveObj;
        */
    }

    public void UpdateDecks(System.Action<PlayFab.ClientModels.GetUserDataResult> successCallback)
    {
        Online.Instance.GetUserData("Decks", currentDeckListVersion, successCallback);
    }

    public List<string> GetDeckNames()
    {
        List<string> names = new List<string>();
        foreach(Save_Data deck in decks)
        {
            names.Add(deck.deckName);
        }
        return names;
    }

    public void CreateDeckList(string json)
    {
        decks = JsonUtility.FromJson<Save_DataTemp>(json).decks;
    }
}
