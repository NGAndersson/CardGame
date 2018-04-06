using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_DeckSelectionManager : MonoBehaviour {
    public List<UnityEngine.UI.Dropdown> deckDropdowns = new List<UnityEngine.UI.Dropdown>();
    public UnityEngine.UI.Button playButton;
    public scr_GameManager gameSession;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        // Update deck list
        Database.Instance.decks.UpdateDecks(Callback_DeckListUpdated);
        
    }

    private void OnDisable()
    {
        
    }

    private void Callback_DeckListUpdated(PlayFab.ClientModels.GetUserDataResult result)
    {
        if (Database.Instance.decks.currentDeckListVersion != result.DataVersion)
            Database.Instance.decks.CreateDeckList(result.Data["Decks"].Value);

        Database.Instance.decks.currentDeckListVersion = result.DataVersion;

        foreach (UnityEngine.UI.Dropdown deckDropdown in deckDropdowns)
        {
            //Update dropdown
            if (Database.Instance.decks.decks.Count > 0)
            {
                deckDropdown.interactable = true;
                deckDropdown.ClearOptions();
                List<string> deckNames = new List<string>();
                foreach (scr_Deck_Data.Save_Data deck in Database.Instance.decks.decks)
                    deckNames.Add(deck.deckName);
                deckDropdown.AddOptions(deckNames);
                playButton.interactable = true;
            }
            else
            {
                deckDropdown.interactable = false;
                deckDropdown.ClearOptions();
                List<string> noOption = new List<string> { "No Saved Decks Found" };
                deckDropdown.AddOptions(noOption);
                playButton.interactable = false;
            }
        }
    }

    public void PlayButton()
    {
        int playerNumber = 1;
        foreach (UnityEngine.UI.Dropdown deckDropdown in deckDropdowns)
        {
            struct_PlayerData playerData;
            playerData.username = "Player #" + playerNumber.ToString();
            playerNumber++;
            playerData.deckName = deckDropdown.options[deckDropdown.value].text;
            gameSession.InitPlayer(playerData);
        }
        MenuManager.Instance.GoToScreen("GamePlay");
    }

    public void MenuButton(string screen)
    {
        MenuManager.Instance.GoToScreen(screen);
    }
}
