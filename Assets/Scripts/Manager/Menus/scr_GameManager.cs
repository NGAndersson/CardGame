using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_GameManager : MonoBehaviour {
    enum Phase
    {
        NewTurn,
        Draw,
        Play,
        EndTurn
    };
    Phase currentPhase = Phase.NewTurn;

    List<Player> players = new List<Player>();
    uint numPlayers = 0;
    uint currentPlayer = 0;
    List<UnityEngine.UI.Text> playerHealthTexts = new List<UnityEngine.UI.Text>();

	// Use this for initialization
	void Start () {

    }

    public void MenuButtonBack()
    {
        MenuManager.Instance.GoToPrevious();
    }

    private void OnEnable()
    {
        for (int i = 1; i < players.Count; i++)
            players[i].gameObject.SetActive(false);
        players[0].NewTurn();
    }

    private void OnDisable()
    {
        foreach(Player player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();
        currentPlayer = 0;
        foreach (UnityEngine.UI.Text healthText in playerHealthTexts)
            Destroy(healthText.gameObject);
        playerHealthTexts.Clear();
    }

    public void InitPlayer(struct_PlayerData playerData)
    {
        Player newPlayer = ((GameObject)Instantiate(Resources.Load("Prefabs/Player/Player"), gameObject.transform.parent)).GetComponent<Player>();
        newPlayer.GetComponent<Player>().playerName = playerData.username;
        newPlayer.GetComponent<Player>().CreateDeck(playerData.deckName);
        newPlayer.GetComponent<Player>().ShuffleDeck();

        numPlayers++;
        players.Add(newPlayer);

        GameObject playerHealthText = ((GameObject)Instantiate(Resources.Load("Prefabs/Player/PlayerHealthText"), gameObject.transform));
        playerHealthText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, -20 + (-100 * (players.Count - 1)));
        playerHealthTexts.Add(playerHealthText.GetComponent<UnityEngine.UI.Text>());
        playerHealthTexts[players.Count-1].text = newPlayer.playerName + " Health: " + newPlayer.GetComponent<Player>().health;
        newPlayer.healthText = playerHealthText.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PlayerSwitch ()
    {
        players[(int)currentPlayer].EndTurn();
        players[(int)currentPlayer].gameObject.SetActive(false);
        currentPlayer = ((currentPlayer + 1) % numPlayers);
        players[(int)currentPlayer].gameObject.SetActive(true);
        players[(int)currentPlayer].NewTurn();
    }

    //Handles interactions between players (damage, etc)
    public void ResolveCard(Card_Game card)
    {
        //Damage
        int newHealth = players[((int)currentPlayer + 1) % 2].TakeDamage(card.data.AP + card.data.TP);
        if(newHealth <= 0)
        {
            SetWinner(currentPlayer);
        }
        //Send card down the pipeline
        players[(int)currentPlayer].ResolveCard(card);
    }

    void SetWinner(uint player)
    {
        MenuManager.Instance.GoToScreen("DeckSelection");
    }
}
