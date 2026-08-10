using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : ManagerBase
{
	protected int _maxHandSize;

	public int handSize;
	public Sprite cardBack;
	public CardDeck deckPrefab;
	public GameObject roundManagerPrefab;

	public PlayerPosition Dealer { get; protected set; }
	public CardDeck Deck { get; protected set; }
	public ICollection<Hand> Hands { get; protected set; }
	public string GameMode { get; protected set; }
	public int RoundNumber { get; protected set; } = 1;

	private Scoring _scoring;

	public virtual void Awake()
	{
		// Create New Scoring Singleton
		_scoring ??= ServiceLocator.GetSingleton<Scoring>();

		// Create Hands
		Hands = FindFirstObjectByType<Canvas>().GetComponentsInChildren<Hand>();

		// GameMode is determined by the settings/pre-game game mode selection
		GameMode = ServiceLocator.GetSingleton<Settings>().gameMode;

		// GameMode determines hand size and maximum hand size
		switch (GameMode)
		{
			case "Tutorial":
				handSize = 5;
				_maxHandSize = 5;
				break;
			default:
				handSize = 1;
				_maxHandSize = 5;
				break;
		}

		Deck = Instantiate(deckPrefab, new Vector3(360, 173), Quaternion.identity, FindFirstObjectByType<Canvas>().transform);
		Deck.transform.localScale = new Vector3(0.75f, 0.75f);
		Deck.name = deckPrefab.name;

		ServiceLocator.GetSingleton<Scoring>().NewRound();
	}

	public ICollection<Hand> GetHands() => Hands;
	public Hand GetPlayerHand() => Hands.First(h => h.IsPlayer);

	public void UpdateDealer(int dealerId)
	{
		Dealer = (PlayerPosition)dealerId;
		Deck.UpdateDealer(Dealer);
	}

	public void UpdateLabel(int playerId)
	{
		var scoring = ServiceLocator.GetSingleton<Scoring>();
		var tricksWon = scoring.Tricks[playerId];
		var bid = scoring.Bids[playerId];

		var textObject = Hands.First(h => h.Id == playerId).GetComponentInChildren<TextMeshProUGUI>();
		textObject.text = $"{(PlayerPosition)playerId} - {tricksWon}/{bid}";

		if (tricksWon > bid)
		{
			textObject.color = Color.red;
		}

		if (tricksWon == bid)
		{
			textObject.color = Color.green;
		}

		if (tricksWon == bid - 1)
		{
			textObject.color = Color.yellow;
		}
	}


	// Should only be called at end of game, or when user quits game from submenu
	public void AtEndOfGame()
	{
		// Tidyup / await any user input


		// Destroy dependencies
		ServiceLocator.DestroySingleton<Scoring>();

		// Return to Main Menu
		SceneManager.LoadScene("MainMenu");
	}
}
